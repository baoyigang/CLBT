using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebUI_SysInfo_UserManage_Temp : System.Web.UI.Page
{
    protected string UserID;
    protected void Page_PreInit(object sender, EventArgs e)
    {

        if (Session["G_user"] == null)
        {
            Response.Write("<script language='javascript'>alert('对不起,操作时限已过,请重新登入！'); var parentwindow=window.dialogArguments;parentwindow.top.location = '../Login.aspx';window.close();</script>");
            return;
        }

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        UserID = Request.Params["UserID"] + "";
    }
}