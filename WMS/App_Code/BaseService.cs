using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Util;


/// <summary>
///BaseService 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
//若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。 
// [System.Web.Script.Services.ScriptService]
public class BaseService : System.Web.Services.WebService {

    public BaseService () {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }


    [WebMethod]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Xml)]
    public ReturnData strBaseData(string xmlpara)
    {
        ReturnData rr = new ReturnData();

        DataTable dt = Util.JsonHelper.Json2Dtb(xmlpara);


        string strWhere = Microsoft.JScript.GlobalObject.unescape(dt.Rows[0]["strWhere"].ToString());

        string strFieldName = Microsoft.JScript.GlobalObject.unescape(dt.Rows[0]["strFieldName"].ToString());
        string TableName = Microsoft.JScript.GlobalObject.unescape(dt.Rows[0]["TableName"].ToString()); ;
        if (strFieldName == "")
            strFieldName = "*";

       
        BLL.BLLBase bll = new BLL.BLLBase();
        dt = bll.FillDataTable("Security.SelectFieldValue", new DataParameter[] { new DataParameter("{0}", TableName), new DataParameter("{1}", strFieldName), new DataParameter("{2}", strWhere) });

        rr.data = Util.JsonHelper.Dtb2Json(dt);
        rr.type = "" + dt.GetType();

        return rr;



    }
    [WebMethod]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Xml)]
    public ReturnData autoCode(string xmlpara)
    {

        ReturnData rr = new ReturnData();
        try
        {
            BLL.BLLBase bll = new BLL.BLLBase();

            DataTable dt = Util.JsonHelper.Json2Dtb(xmlpara);
            string PreName = dt.Rows[0]["PreName"].ToString();
            string dtTime = dt.Rows[0]["dtTime"].ToString();
            string Filter = dt.Rows[0]["Filter"].ToString();

            string strCode = bll.GetAutoCode(PreName, DateTime.Parse(dtTime), Filter);
            rr.data = strCode;
            rr.type = "" + strCode.GetType();
        }
        catch (Exception ex)
        {

        }

        return rr;
    }

    [WebMethod]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Xml)]
    public ReturnData autoCodeByTableName(string xmlpara)
    {

        ReturnData rr = new ReturnData();
        try
        {



            BLL.BLLBase bll = new BLL.BLLBase();

            DataTable dt = Util.JsonHelper.Json2Dtb(xmlpara);
            string PreName = dt.Rows[0]["PreName"].ToString();
            string dtTime = dt.Rows[0]["dtTime"].ToString();
            string TableName = dt.Rows[0]["TableName"].ToString();
            string Filter = dt.Rows[0]["Filter"].ToString();

            string strCode = bll.GetAutoCodeByTableName(PreName, TableName, DateTime.Parse(dtTime), Filter);
            rr.data = strCode;
            rr.type = "" + strCode.GetType();
        }
        catch (Exception ex)
        {

        }

        return rr;
    }

    [WebMethod]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Xml)]
    public ReturnData GetCellInfo(string xmlpara)
    {

        ReturnData rr = new ReturnData();
        try
        {
            DataTable dt = Util.JsonHelper.Json2Dtb(xmlpara);
            string CellCode = dt.Rows[0]["CellCode"].ToString();

            BLL.BLLBase bll = new BLL.BLLBase();
            DataTable dtCell = bll.FillDataTable("CMD.SelectWareHouseCellInfoByCell", new DataParameter[] { new DataParameter("@CellCode", CellCode) });
            string str = Util.JsonHelper.Dtb2Json(dtCell);
            rr.data = str;
            rr.type = "" + dtCell.GetType();
        }
        catch (Exception ex)
        {

        }

        return rr;
    }

    [WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Xml)]
    public ReturnData doJsDAOMode(string xmlpara)
    {
        ReturnData rr = new ReturnData();
        try
        {
            DataTable dt = Util.JsonHelper.Json2Dtb(xmlpara);
            // Get the constructor and create an instance of MagicClass
            object[] arr = new object[] { };

            System.Reflection.Assembly aa = System.Reflection.Assembly.Load("BLL");
            Type magicType = aa.GetType("" + dt.Rows[0]["dll_className"]);


            object magicClassObject = Activator.CreateInstance(magicType);

            System.Reflection.MethodInfo magicMethod = magicType.GetMethod("" + dt.Rows[0]["dll_ModeName"]);

            System.Reflection.ParameterInfo[] parainfo = magicMethod.GetParameters();
            arr = new object[parainfo.Length];
            //參數
            for (int i = 0; i < parainfo.Length; i++)
            {
                if (parainfo[i].ParameterType == typeof(byte))
                {
                    arr[i] = byte.Parse("" + dt.Rows[0][parainfo[i].Name]);
                }
                else if (parainfo[i].ParameterType == typeof(bool))
                {
                    arr[i] = bool.Parse("" + dt.Rows[0][parainfo[i].Name]);
                }
                else if (parainfo[i].ParameterType == typeof(DateTime))
                {
                    arr[i] = DateTime.Parse("" + dt.Rows[0][parainfo[i].Name]);
                }
                else if (parainfo[i].ParameterType == typeof(int))
                {
                    arr[i] = int.Parse("" + dt.Rows[0][parainfo[i].Name]);
                }
                else if (parainfo[i].ParameterType == typeof(decimal))
                {
                    arr[i] = decimal.Parse("" + dt.Rows[0][parainfo[i].Name]);
                }
                else
                    arr[i] = "" + dt.Rows[0][parainfo[i].Name];

            }

            if (magicMethod.ReturnType == typeof(void))
            {
                magicMethod.Invoke(magicClassObject, arr); return null;
            }

            object magicValue = magicMethod.Invoke(magicClassObject, arr);
            if (magicValue.GetType() == typeof(DataTable))
            {
                dt = (DataTable)magicValue;
                rr.data = Util.JsonHelper.Dtb2Json(dt);
                rr.type = "" + dt.GetType();
            }
            else
            {
                rr.data = "" + magicValue;
                rr.type = "" + magicValue.GetType();
            }
            return rr;
        }
        catch (Exception ex)
        {
            rr.data = "doJsDAOMode ErrMsg:" + ex.Message;
            rr.type = "ErrMsg";
            return rr;
        }

    }
    
}

/// <summary>
/// 自定義 反回值
/// </summary>
public class ReturnData
{
    public string data = "";
    public string type = "";
}
