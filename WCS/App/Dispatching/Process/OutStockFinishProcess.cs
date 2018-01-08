using System;
using System.Collections.Generic;
using System.Text;
using MCP;
using System.Data;
using Util;

namespace App.Dispatching.Process
{
    public class OutStockFinishProcess : AbstractProcess
    {
        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            object obj = ObjectUtil.GetObject(stateItem.State);
            if (obj == null)
                return;

            BLL.BLLBase bll = new BLL.BLLBase();
            string Request = obj.ToString();
            if (Request.Equals("True") || Request.Equals("0"))
            {
                string AreaCode=BLL.Server.GetAreaCode();
                string AisleNo = "";
                try
                {
                    string taskNo = "";
                    switch (stateItem.ItemName)
                    {
                        case "OutFinish1":
                            taskNo = ObjectUtil.GetObject(WriteToService(stateItem.Name, "ReadTaskNo1")).ToString();
                            break;
                        case "OutFinish2":
                            taskNo = ObjectUtil.GetObject(WriteToService(stateItem.Name, "ReadTaskNo2")).ToString();
                            break;

                    }

                    //DataTable dtT = bll.FillDataTable("WCS.SelectTask", new DataParameter("{0}", string.Format("WCS_Task.AreaCode='{0}' and S1.AisleNo='{1}' and WCS_TASK.State=10  and (WCS_TASK.TaskType='12' or WCS_TASK.taskType='14')", AreaCode, AisleNo)));
                    //if (dtT.Rows.Count==0)
                    //{
                    //    return;
                    //}
                    //string taskNo = dtT.Rows[0]["taskNo"].ToString();
                    
                    if (taskNo.Trim().Length > 0)
                    {
                        

                        DataTable dt = bll.FillDataTable("WCS.SelectReadTaskByTaskNo", new DataParameter[] { new DataParameter("@TaskNo", taskNo) });
                        if (dt.Rows.Count > 0)
                        {
                            string TaskType = dt.Rows[0]["TaskType"].ToString();
                            string strState = dt.Rows[0]["State"].ToString();
                            string StationNo = dt.Rows[0]["StationNo"].ToString();
                            if (TaskType=="11")
                            {
                                DataParameter[] param;
                                param = new DataParameter[] { new DataParameter("@StationNo", StationNo), new DataParameter("@TaskNo", taskNo) };
                                bll.ExecNonQuery("WCS.UpdateTaskInStockRequest", param);
                                Logger.Info("任务号:" + taskNo + " 托盘:" + " 开始入库,到达入库口:" + StationNo);
                            }

                            if (TaskType == "12" || TaskType == "15" || TaskType == "14") //出库,托盘出库,盘点
                            {
                                DataParameter[] param = new DataParameter[] { new DataParameter("@TaskNo", taskNo) };
                                DataTable dtXml = bll.FillDataTable("WCS.Sp_TaskProcess", param);
                                Logger.Info("出库任务完成,任务号:" + taskNo );

                  
                                string Flag = "";

                                if (TaskType == "12")
                                        Flag = "BatchOutStock";
                         
                                if (dtXml.Rows.Count > 0)
                                {
                                    string BillNo = dtXml.Rows[0][0].ToString();
                                    if (BillNo.Trim().Length > 0)
                                    {

                                        string xml = Util.ConvertObj.ConvertDataTableToXmlOperation(dtXml, Flag);
                                        WriteToService("ERP", "ACK", xml);
                                        Logger.Info("单号" + dtXml.Rows[0][0].ToString() + "已完成，开始上报ERP系统");
                                    }
                                }
                                    //盘点入库
                                    if (TaskType=="14")
                                    {
                                        int SlideNum = 1;
                                        string CellCode = dt.Rows[0]["CellCode"].ToString();
                                        if (CellCode.Length > 0)
                                        {
                                            if (CellCode.Substring(3, 1) == "1" || CellCode.Substring(3, 1) == "2")
                                            {
                                                StationNo = "01";
                                                SlideNum = 1;
                                            }
                                            else if (CellCode.Substring(3, 1) == "3" || CellCode.Substring(3, 1) == "4")
                                            {
                                                StationNo = "02";
                                                SlideNum = 2;
                                            }
                                            else if (CellCode.Substring(3, 1) == "5" || CellCode.Substring(3, 1) == "6")
                                            {
                                                StationNo = "03";
                                                SlideNum = 3;
                                            }
                                            else
                                            {
                                                StationNo = "04";
                                                SlideNum = 4;
                                            }
                                        }
                                        else
                                        {
                                            Logger.Error("盘点任务货位丢失，请核对");
                                            return;
                                        }
                                        int staskNo=int.Parse(taskNo);

                                        if (StationNo=="01")
                                        {
                                            Context.ProcessDispatcher.WriteToService("TranLine", "TaskNo", staskNo);
                                            Context.ProcessDispatcher.WriteToService("TranLine", "SlideNum", SlideNum);
                                            Context.ProcessDispatcher.WriteToService("TranLine", "NewTask", 1);
                                            Context.ProcessDispatcher.WriteToService("TranLine", "TaskType", 1);
                                        }
                                        else
                                        {
                                            Context.ProcessDispatcher.WriteToService("TranLine", "TaskNo1", staskNo);
                                            Context.ProcessDispatcher.WriteToService("TranLine", "SlideNum1", SlideNum);
                                            Context.ProcessDispatcher.WriteToService("TranLine", "NewTask1", 1);
                                            Context.ProcessDispatcher.WriteToService("TranLine", "TaskType1", 1);
                                        }

                                        bll.ExecNonQuery("WCS.UpdateTaskStateByTaskNo", new DataParameter[] { new DataParameter("@State", 2), new DataParameter("@TaskNo", taskNo) });
                                    }
                                
                                //string strValue = "";
                                //string[] str = new string[3];
                                //if (TaskType == "12" || TaskType == "14")//显示拣货信息.
                                //{
                                //    str[0] = "1";
                                //    if (TaskType == "14")
                                //        str[0] = "2";

                                //    while ((strValue = FormDialog.ShowDialog(str, dt)) != "")
                                //    {
                                //        break;
                                //    }
                                //}
                            }
                        }                        
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("到达出库口,错误讯息:" + ex.Message);
                }
            }
        }
    }
}
