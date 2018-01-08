using System;
using System.Collections.Generic;
using System.Text;
using MCP;
using System.Data;
using Util;


namespace App.Dispatching.Process
{
    public class ErpProcess : AbstractProcess
    {
        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            try
            {
                string cmd = stateItem.ItemName;
                Dictionary<string, string> obj = (Dictionary<string, string>)stateItem.State;
                string IsUpErp = "1";
                string ErpMsg = obj["MSG"];
                if (obj["Result"].ToUpper() == "N")
                    IsUpErp = "0";
                string strTaskType = "";
                string Where = string.Format("WCS_Task.Barcode like '%{0}%'", obj["BillNo"]);

                if (cmd.Trim() == "")
                {
                    Logger.Error("Erp回传内容为空！");
                    return;

                }
                switch (cmd)
                {
                    case "InStock":
                        strTaskType = "入库";
                        Where = string.Format(" billid in (  select billid from wcs_task where taskid in (select taskid  from WCS_TASK where barcode like '%{0}%' and tasktype='11')) ", obj["BillNo"]);
                        break;
                    case "OutStock":
                        strTaskType = "出库";
                        Where = string.Format("  billid in ( select BillID  from WCS_TASK where barcode like '%{0}%' and tasktype='12') ", obj["BillNo"]);
                        break;
                    case "CheckStock":
                        Where = string.Format("BillID='{0}'", obj["BillNo"]);
                        strTaskType = "盘点";
                        Where += " and TaskType=14 ";
                        break;
                }

                BLL.BLLBase bll = new BLL.BLLBase();


                bll.ExecNonQuery("WCS.UpdateBillUpErp", new DataParameter[] { new DataParameter("@IsUpERP", IsUpErp), new DataParameter("@ErpMSG", ErpMsg), new DataParameter("{0}", Where) });
                string InfoMesg = strTaskType + "熔次卷号：" + obj["BillNo"] + "返回值为：" + obj["Result"] + "," + obj["MSG"];
                if (IsUpErp == "0")
                {
                    Logger.Error(InfoMesg);
                }
                else
                {
                    Logger.Info(InfoMesg);
                }


            }
            catch (Exception ex)
            {
                Logger.Error("ErpProcess出错:" + ex.Message);
            }
        }
    }
}