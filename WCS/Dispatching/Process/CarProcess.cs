using System;
using System.Collections.Generic;
using System.Text;
using MCP;
using System.Data;
using IDAL;

namespace Dispatching.Process
{
    public class CarProcess : AbstractProcess
    {
        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            try
            {
                object obj = ObjectUtil.GetObject(stateItem.State);
                if (obj == null)
                    return;
                if (obj.ToString() == "48" || obj.ToString() == "0")
                    return;

                
                string carNo = stateItem.ItemName.Substring(0, 2);

                Logger.Info("小车" + carNo + "接到入库确认请求");
                string CraneNo = "01";
                if (carNo == "02")
                    CraneNo = "02";

                BLL.BLLBase bll = new BLL.BLLBase();
                //判断堆垛机状态是否可用

                //查找有无请求状态(state='1')的任务，如没有则认为是空托盘回库
                string filter = string.Format("WCS_Task.State='1' and WCS_TASK.TaskType='11' and WCS_TASK.CarNo='{0}'", carNo);
                DataParameter[] parameter = new DataParameter[] { new DataParameter("{0}", filter) };
                DataTable dt = bll.FillDataTable("WCS.SelectTask", parameter);

                int rows = 0;
                if (dt.Rows.Count > 0)
                {
                    rows = bll.ExecNonQuery("WCS.UpdateTaskState", new DataParameter[] { new DataParameter("@State", 2), new DataParameter("@CarNo", carNo) });
                    WriteToService("CarPLC", carNo + "_CarReply", 49);
                    Logger.Info("小车：" + carNo + "收到入库请求,更新任务状态，影响行数" + rows);
                }
                else
                {
                    //判断有无正在执行的任务，如有不做处理,如没有则产生一笔空盘回库任务
                    filter = string.Format("WCS_Task.State='3' and WCS_TASK.TaskType='11' and WCS_TASK.CarNo='{0}'", carNo);
                    parameter = new DataParameter[] { new DataParameter("{0}", filter) };
                    DataTable dting = bll.FillDataTable("WCS.SelectTask", parameter);

                    if (dting.Rows.Count > 0)
                    {
                        WriteToService("CarPLC", carNo + "_CarReply", 50);
                        return;
                    }
                    //有无盘点回库任务
                    filter = string.Format("WCS_Task.State='4' and WCS_TASK.TaskType='14' and WCS_TASK.CarNo='{0}'", carNo);
                    parameter = new DataParameter[] { new DataParameter("{0}", filter) };
                    dting = bll.FillDataTable("WCS.SelectTask", parameter);
                    if (dting.Rows.Count > 0)
                    {
                        //盘点任务改变状态为5，堆垛机调度回库
                        string TaskNo = dting.Rows[0]["TaskNo"].ToString();
                        bll.ExecNonQuery("WCS.UpdateTaskStateByTaskNo", new DataParameter[] { new DataParameter("@State", 5), new DataParameter("@TaskNo", TaskNo) });

                        WriteToService("CarPLC", carNo + "_CarReply", 49);
                        return;
                    }

                    //查找锁定货位，且产品是托盘的货位,如有判断为实托盘出库后，托盘回库
                    string ProductCode = "00" + CraneNo;
                    parameter = new DataParameter[] 
                    { 
                        new DataParameter("{0}", string.Format("CMD_Cell.ProductCode='{0}' and CMD_Cell.IsActive='1' and CMD_Cell.IsLock='1' and CMD_Cell.ErrorFlag!='1'",ProductCode)),
                        new DataParameter("{1}", carNo)
                    };
                    dt = bll.FillDataTable("CMD.SelectCellByCar", parameter);
                    if (dt.Rows.Count > 0)
                    {
                        //产生一笔空托盘入库任务
                        //锁定货位
                        parameter = new DataParameter[] 
                        { 
                            new DataParameter("@CarNo", carNo),
                            new DataParameter("@CraneNo", CraneNo),
                            new DataParameter("@CellCode", dt.Rows[0]["CellCode"].ToString())
                        };
                        bll.ExecNonQueryTran("WCS.Sp_RequestPalletInTask", parameter);
                        WriteToService("CarPLC", carNo + "_CarReply", 49);
                        Logger.Info("小车：" + carNo + "接到入库请求,产生一笔托盘回库任务");
                    }
                }
                WriteToService("CarPLC", carNo + "_CarReply", 50);
            }
            catch (Exception e)
            {
                Logger.Error("Dispatching.Process.CarProcess：" + e.Message);
            }
        }
    }
}