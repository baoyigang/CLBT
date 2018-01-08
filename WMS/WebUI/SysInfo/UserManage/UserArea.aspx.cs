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
using Util;
using System.Collections.Generic;

public partial class WebUI_SysInfo_UserManage_UserArea : BasePage
{
    BLL.BLLBase bll = new BLL.BLLBase();
    protected void Page_Load(object sender, EventArgs e)
    {

        string UserID = Request.QueryString["UserID"].ToString(); ;
        SetTextReadOnly(this.txtUserName);

        if (!Page.IsPostBack)
        {
            DataTable dtuser = bll.FillDataTable("Security.SelectUser", new DataParameter[] { new DataParameter("{0}", string.Format("UserID={0}", UserID)) });
            if (dtuser.Rows.Count > 0)
            {
                this.txtUserID.Value = UserID;
                this.txtUserName.Text = dtuser.Rows[0]["UserName"].ToString();
            }
            BindWarehouse();



            DataTable dtUserArea = bll.FillDataTable("Security.SelectUserArea", new DataParameter[] { new DataParameter("@UserID", UserID) });

            foreach (ListItem cbox in chkArea.Items)
            {

                DataRow[] drs = dtUserArea.Select("AreaCode=" + cbox.Value);
                if (drs.Length > 0)
                    cbox.Selected = true;

            }




        }


    }
    private void BindWarehouse()
    {

        DataTable dtWare = bll.FillDataTable("CMD.SelectArea");
        this.chkArea.DataValueField = "AreaCode";
        this.chkArea.DataTextField = "AreaName";
        this.chkArea.DataSource = dtWare;
        this.chkArea.DataBind();

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        List<string> comds = new List<string>();
        List<DataParameter[]> paras = new List<DataParameter[]>();

        comds.Add("Security.DeleteUserArea");
        paras.Add(new DataParameter[] { new DataParameter("@UserID", this.txtUserID.Value) });




        foreach (ListItem cbox in chkArea.Items)
        {
            if (cbox.Selected)
            {
                comds.Add("Security.InsertUserArea");
                paras.Add(new DataParameter[] { new DataParameter("@UserID", this.txtUserID.Value), new DataParameter("@AreaCode", cbox.Value) });
            }

        }


        bll.ExecTran(comds.ToArray(), paras);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Resize", "alert('数据保存成功！'); Close();", true);
    }
}