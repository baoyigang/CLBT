using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;

public partial class WebUI_SysInfo_PwdModify_PwdModify :BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            txtUserName.Text = Session["G_user"].ToString();
        }
        catch (Exception ex)
        {
            txtUserName.Text = "";
        }
    }
    protected void lbtnSave_Click(object sender, EventArgs e)
    {
        //DatabaseOperater dbOperator = new DatabaseOperater();

        //dbOperator.GetModelName = "sys_UserListlogin";
        //dbOperator.GetStrProcParaValue = txtUserName.Text + "," + encrypObject.EncryptMD5(); ;

        BLL.Security.UserBll userBll = new BLL.Security.UserBll();

        DataTable dtUserList = userBll.GetUserInfo(txtUserName.Text.Trim());

        if (dtUserList.Rows[0]["UserPassword"].ToString().Trim() == this.txtOldPwd.Text.Trim())
        {

            userBll.UpdateUserPWD(this.txtUserName.Text.Trim(), this.txtAckPwd.Text.Trim());
            //WMS.App_Code.JScript.Instance.ShowMessage(this, "密码修改成功!");
            ClientScript.RegisterClientScriptBlock(ClientScript.GetType(), "myscript", "<script type='text/javascript'>alert('密码修改成功!'); Exit();</script>");
        }
        else
        {
            JScript.Instance.ShowMessage(this, "原密码错误!");
        }


    }
    public string getColorValue(string s)
    {
        if (Session["IsUseGlobalParameter"].ToString() == "1")
            return s;
        else return "";
    }
}