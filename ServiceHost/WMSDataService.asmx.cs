using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Xml;
using System.IO;
using Util;
using IServices;
using System.Text;

namespace ServiceHost
{
    /// <summary>
    /// WMSDataService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    //[System.Web.Script.Services.ScriptService]
    public class WMSDataService : System.Web.Services.WebService
    {
        /// <summary>
        /// WMS系统提供接收ERP系统下发物料信息的接口
        /// </summary>
        /// <param name="wmsProductObject"></param>
        /// <returns></returns>
        [WebMethod]
        public string transWmsProduct(string wmsProductObject)
        {
            WriteToLog("transWmsProduct", wmsProductObject);

            DataSet xmlDS = Util.ConvertObj.XmlStringToDataSet(wmsProductObject);

            DataTable dt = xmlDS.Tables[0];
            DataTable dtNew = dt.DefaultView.ToTable(true, "ProductCode", "Size");

            string Msg = "成功";
            string bln = "Y";

            string result = bln + "," + Msg;
            string strXML1 = "<?xml version=\"1.0\" encoding=\"GB2312\" standalone=\"yes\"?>" + Environment.NewLine + "<DATASETS>" + Environment.NewLine + "<DATASET>" + Environment.NewLine;

            string strXML2 = Environment.NewLine + "</DATASET>" + Environment.NewLine + "</DATASETS>" + Environment.NewLine;

            string strResult = "<RESULT>" + result + "</RESULT>";

        
            if (dt.Rows.Count == 0)
            {
                bln = "N";
                Msg = "内容不能为空！";
                result = bln + "," + Msg;
                strResult = "<RESULT>" + result + "</RESULT>";
                return strXML1 + strResult + strXML2;
            }



            string[] Comds = new string[dt.Rows.Count];
            List<DataParameter[]> paras = new List<DataParameter[]>();
            BLL.BLLBase bll = new BLL.BLLBase();
            string strProductCode = bll.GetNewID("CMD_Product", "ProductCode", "1=1");


            for (int i = 0; i < dt.Rows.Count; i++)
            { 
                DataRow dr = dt.Rows[i];

                int HasCount = bll.GetRowCount("CMD_Product", string.Format("ProductNo='{0}' and Spec='{1}'", dr["ProductOldCode"].ToString().Replace("'", "''"), dr["OldSize"].ToString().Replace("'", "''")));
               
                DataParameter[] para;
                if (HasCount > 0)
                {
                    Comds[i] = "WMSServices.UpdateProduct";

                    para = new DataParameter[] {    new DataParameter("@ProductCode",dr["ProductCode"].ToString()), 
                                                    new DataParameter("@ProductName",dr["ProductName"].ToString()),
                                                    new DataParameter("@ProductEName",dr["ProductEName"].ToString()),
                                                    new DataParameter("@Size",dr["Size"].ToString()),
                                                    new DataParameter("@AlloyTemper",dr["AlloyTemper"].ToString()),
                                                    new DataParameter("@Weight",dr["Weight"].ToString()),
                                                    new DataParameter("@ProductType",dr["ProductType"].ToString()),
                                                    new DataParameter("@StandardNo",dr["StandardNo"].ToString()),
                                                    new DataParameter("@PartNo",dr["PartNo"].ToString()),
                                                    new DataParameter("@Memo",dr["Memo"].ToString()),
                                                    new DataParameter("@ProductOldCode",dr["ProductOldCode"].ToString()),
                                                    new DataParameter("@OldSize",dr["OldSize"].ToString())
                                                };

                }
                else
                {
                    Comds[i] = "WMSServices.InsertProduct";
                    string strWmsProductCode = strProductCode;
                    strProductCode = Util.Utility.NewID(strWmsProductCode);

                    para = new DataParameter[] {    new DataParameter("@ProductWMSCode",strWmsProductCode),
                                                    new DataParameter("@ProductCode",dr["ProductCode"].ToString()), 
                                                    new DataParameter("@ProductName",dr["ProductName"].ToString()),
                                                    new DataParameter("@ProductEName",dr["ProductEName"].ToString()),
                                                    new DataParameter("@Size",dr["Size"].ToString()),
                                                    new DataParameter("@AlloyTemper",dr["AlloyTemper"].ToString()),
                                                    new DataParameter("@Weight",dr["Weight"].ToString()),
                                                    new DataParameter("@ProductType",dr["ProductType"].ToString()),
                                                    new DataParameter("@StandardNo",dr["StandardNo"].ToString()),
                                                    new DataParameter("@PartNo",dr["PartNo"].ToString()),
                                                    new DataParameter("@Memo",dr["Memo"].ToString())
                                               };

                }
                paras.Add(para);
            }
         
          
            try
            {
                bll.ExecTran(Comds, paras);
            }
            catch (Exception ex)
            {
                Msg = ex.Message;
                bln = "N";
            }
            result = bln + "," + Msg;
            strResult = "<RESULT>" + result + "</RESULT>";
            return strXML1 + strResult + strXML2;
        }
        /// <summary>
        /// ERP系统为WMS提供批次入库信息
        /// </summary>
        /// <param name="wmsProductObject"></param>
        /// <returns></returns>
        [WebMethod]
        public string transInStock(string wmsInStockObject)
        {
            WriteToLog("transInStock", wmsInStockObject);
            DataSet xmlDS = Util.ConvertObj.XmlStringToDataSet(wmsInStockObject);
            DataTable dt = xmlDS.Tables[0];

            string Msg = "成功";
            string bln = "Y";

            string result = bln + "," + Msg;
            string strXML1 = "<?xml version=\"1.0\" encoding=\"GB2312\" standalone=\"yes\"?>" + Environment.NewLine + "<DATASETS>" + Environment.NewLine + "<DATASET>" + Environment.NewLine;

            string strXML2 = Environment.NewLine + "</DATASET>" + Environment.NewLine + "</DATASETS>" + Environment.NewLine;

            string strResult = "<RESULT>" + result + "</RESULT>";
            BLL.BLLBase bll = new BLL.BLLBase();


            if (dt.Rows.Count == 0)
            {
                bln = "N";
                Msg = "内容不能为空！";
                result = bln + "," + Msg;
                strResult = "<RESULT>" + result + "</RESULT>";
                return strXML1 + strResult + strXML2;
            }
            else
            {

                DataTable dtCode = dt.DefaultView.ToTable(true, "BillNo");

                List<string> list = new List<string>();
                List<DataParameter[]> paras = new List<DataParameter[]>();
                DataParameter[] para;
                string strWhere = "'-1'";
                string strBillID = bll.GetAutoCodeByTableName("IS", "WMS_BillMaster", DateTime.Now, "1=1");
                string BillID = strBillID;
                for (int k = 0; k < dtCode.Rows.Count; k++)
                {

                    if (k != 0)
                    {
                        BillID = Util.Utility.NewID(strBillID);
                        strBillID = BillID;
                    }

                    string BillNo=dtCode.Rows[k]["BillNo"].ToString().Replace("'", "''");
                    int HasCount = bll.GetRowCount("WMS_BillMaster", string.Format("SourceBillNo='{0}' and BillID like 'IS%' and SourceBillNo!=''", BillNo));
                    if (HasCount > 0)
                    {
                        bln = "N";
                        Msg = "单号" + BillNo + "已经传入WMS，不能再次传递！";
                        result = bln + "," + Msg;
                        strResult = "<RESULT>" + result + "</RESULT>";
                        return strXML1 + strResult + strXML2;
                    }
                    DataRow[] drs = dt.Select(string.Format("BillNo='{0}'", BillNo));

                    for (int i = 0; i < drs.Length; i++)
                    {
                        //判断该熔次卷号是否在库存中
                        DataRow dr = drs[i];
                        HasCount = bll.GetRowCount("CMD_CELL", string.Format("Barcode like '%{0}%'", dr["BatchNo"].ToString()));
                        if (HasCount > 0)
                        {
                            bln = "N";
                            Msg = "熔次卷号" + dr["BatchNo"].ToString() + "已经传入WMS，不能再次传递！";
                            result = bln + "," + Msg;
                            strResult = "<RESULT>" + result + "</RESULT>";
                            return strXML1 + strResult + strXML2;
                        }
                        //判断是否有在未执行的入库任务中

                        HasCount = bll.GetRowCount("WCS_Task", string.Format("TaskType='11' and Barcode like '%{0}%' and state<7", dr["BatchNo"].ToString()));
                        if (HasCount > 0)
                        {
                            bln = "N";
                            Msg = "熔次卷号" + dr["BatchNo"].ToString() + "已经传入WMS，不能再次传递！";
                            result = bln + "," + Msg;
                            strResult = "<RESULT>" + result + "</RESULT>";
                            return strXML1 + strResult + strXML2;
                        }


                       
                        list.Add("WMSServices.InsertBillTemp");
                        para = new DataParameter[] { new DataParameter("@BillType","IS"), 
                                                     new DataParameter("@BillNo",dr["BillNo"]),
                                                     new DataParameter("@BillDate",dr["BillDate"]),
                                                     new DataParameter("@BatchNo",dr["BatchNo"]),
                                                     new DataParameter("@ProductCode",dr["ProductCode"]),
                                                     new DataParameter("@Size",dr["Size"]),
                                                     new DataParameter("@Weight",dr["Weight"]),
                                                     new DataParameter("@Quantity",1),
                                                     new DataParameter("@Memo",dr["Memo"]) 
                                                             
                                         };

                        paras.Add(para);
                    }
                    list.Add("WMSServices.InsertInStock");//插入主表
                    para = new DataParameter[] { new DataParameter("@BillID", BillID), new DataParameter("@BillNo", BillNo) };
                    paras.Add(para);


                    list.Add("WMSServices.InsertInStockDetail"); //从表
                    para = new DataParameter[] { new DataParameter("@BillID", BillID), new DataParameter("@BillNo", BillNo) };
                    paras.Add(para);

                    strWhere += ",'" + BillID + "'";

                    list.Add("WMSServices.DeleteBillTemp");
                    para = new DataParameter[] { new DataParameter("@BillType","IS"),
                                                 new DataParameter("@BillNo",BillNo)
                                                };
                    paras.Add(para);

                }
                list.Add("WMSServices.SpInstockTask");//入库作业
                para = new DataParameter[] { new DataParameter("@strWhere", strWhere), new DataParameter("@UserName", "WebServices") };
                paras.Add(para);

               


               

                try
                {
                    bll.ExecTran(list.ToArray(), paras);
                }
                catch (Exception ex)
                {
                    Msg = ex.Message;
                    bln = "N";
                }
            }

            result = bln + "," + Msg;
            strResult = "<RESULT>" + result + "</RESULT>";
            return strXML1 + strResult + strXML2;

        }

        /// <summary>
        /// ERP系统为WMS提供批次下架信息
        /// </summary>
        /// <param name="wmsProductObject"></param>
        /// <returns></returns>
        [WebMethod]
        public string transOutStock(string wmsOutStockObject)
        {
            WriteToLog("transOutStock", wmsOutStockObject);
            DataSet xmlDS = Util.ConvertObj.XmlStringToDataSet(wmsOutStockObject);
            DataTable dt = xmlDS.Tables[0];

            string Msg = "成功";
            string bln = "Y";

            string result = bln + "," + Msg;
            string strXML1 = "<?xml version=\"1.0\" encoding=\"GB2312\" standalone=\"yes\"?>" + Environment.NewLine + "<DATASETS>" + Environment.NewLine + "<DATASET>" + Environment.NewLine;

            string strXML2 = Environment.NewLine + "</DATASET>" + Environment.NewLine + "</DATASETS>" + Environment.NewLine;

            string strResult = "<RESULT>" + result + "</RESULT>";
            BLL.BLLBase bll = new BLL.BLLBase();


           
            if (dt.Rows.Count == 0)
            {
                bln = "N";
                Msg = "内容不能为空！";
                result = bln + "," + Msg;
                strResult = "<RESULT>" + result + "</RESULT>";
                return strXML1 + strResult + strXML2;
            }
            else
            {
                DataTable dtCode = dt.DefaultView.ToTable(true, "BillNo");
                List<string> list = new List<string>();
                List<DataParameter[]> paras = new List<DataParameter[]>();
                DataParameter[] para;

                string strBillID = bll.GetAutoCodeByTableName("OS", "WMS_BillMaster", DateTime.Now, "1=1");
                string BillID = strBillID;
                for (int k = 0; k < dtCode.Rows.Count; k++)
                {

                    if (k != 0)
                    {
                        BillID = Util.Utility.NewID(strBillID);
                        strBillID = BillID;
                    }
                    string BillNo = dtCode.Rows[k]["BillNo"].ToString().Replace("'", "''");
                    int HasCount = bll.GetRowCount("WMS_BillMaster", string.Format("SourceBillNo='{0}' and BillID like 'OS%'", BillNo));
                    if (HasCount > 0)
                    {
                        bln = "N";
                        Msg = "单号" + dtCode.Rows[0]["BillNo"].ToString() + "已经传入WMS，不能再次传递！";
                        result = bln + "," + Msg;
                        strResult = "<RESULT>" + result + "</RESULT>";
                        return strXML1 + strResult + strXML2;
                    }


                   

                    
                    DataRow[] drs = dt.Select(string.Format("BillNo='{0}'", BillNo));

                    for (int i = 0; i < drs.Length; i++)
                    {
                        DataRow dr = drs[i];

                        HasCount = bll.GetRowCount("CMD_CELL", string.Format("Barcode like '%{0}%' and IsLock=0", dr["BatchNo"].ToString()));
                        if (HasCount == 0)
                        {
                            bln = "N";
                            Msg = "单号：" + dtCode.Rows[0]["BillNo"].ToString() + " 熔次卷号：" + dr["BatchNo"] + " 库存数量为零无法出库！";
                            result = bln + "," + Msg;
                            strResult = "<RESULT>" + result + "</RESULT>";
                            return strXML1 + strResult + strXML2;
                        }


                       
                       list.Add("WMSServices.InsertBillTemp");
                        para = new DataParameter[] { new DataParameter("@BillType","OS"), 
                                            new DataParameter("@BillNo",dr["BillNo"]),
                                            new DataParameter("@BillDate",dr["BillDate"]),
                                            new DataParameter("@BatchNo",dr["BatchNo"]),
                                            new DataParameter("@ProductCode",dr["ProductCode"]),
                                            new DataParameter("@Size",dr["Size"]),
                                            new DataParameter("@Weight",dr["Weight"]),
                                            new DataParameter("@Quantity",1),
                                            new DataParameter("@Memo",dr["Memo"]) 
                                                             
                                         };

                        paras.Add(para);
                    }
                   

                   list.Add("WMSServices.InsertOutStock");//插入主表
                    para = new DataParameter[] { new DataParameter("@BillID", BillID), new DataParameter("@BillNo", BillNo) };
                    paras.Add(para);


                   list.Add("WMSServices.InsertOutStockDetail"); //从表
                    para = new DataParameter[] { new DataParameter("@BillID", BillID), new DataParameter("@BillNo", BillNo) };
                    paras.Add(para);


                    list.Add("WMSServices.DeleteBillTemp");
                    para = new DataParameter[] { new DataParameter("@BillType","OS"),
                                         new DataParameter("@BillNo",BillNo)
                                        };

                    paras.Add(para);
                }
                try
                {
                    bll.ExecTran(list.ToArray(), paras);
                }
                catch (Exception ex)
                {
                    Msg = ex.Message;
                    bln = "N";
                }
            }

            result = bln + "," + Msg;
            strResult = "<RESULT>" + result + "</RESULT>";
            return strXML1 + strResult + strXML2;
        }
 
        private void WriteToLog(string WmsType, string Msg)
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory + @"\PDATcp";

            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);
            path = path + @"\" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            System.IO.File.AppendAllText(path, string.Format("{0} , {1} :  {2}", DateTime.Now, WmsType, Msg + "\r\n"));
        }


        private  string GetUnicodeString(string sValue)
        {

            Encoding def = Encoding.Default;

            Encoding unicode = Encoding.UTF8;



            // Check whether default encoding is same as "UTF-8" encoding

            if (def == unicode) return sValue;



            // Check parameter

            if (sValue == null || sValue.Length == 0) return sValue;



            // Convert the string into a byte[].

            byte[] defBytes = def.GetBytes(sValue);



            // Perform the conversion from one encoding to the other.

            byte[] unicodeBytes = Encoding.Convert(def, unicode, defBytes);

            char[] uniChars = new char[unicodeBytes.Length];

            for (int i = 0; i < unicodeBytes.Length; i++)

                uniChars[i] = (char)(unicodeBytes[i]);
            return new string(uniChars);

        }


    }
}
