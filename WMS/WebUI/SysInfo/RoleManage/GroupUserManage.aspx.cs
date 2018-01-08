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
using System.Drawing;

public partial class WebUI_SysInfo_RoleManage_GroupUserManage :BasePage
{
    string GroupID = "";
    string GroupName = "";

    protected void Page_Load(object sender, EventArgs e)
    {

        SqlCmd = "Security.SelectAllUser";
        pageSize = 13;
        if (Request.QueryString["GroupID"] != null)
        {

            GroupID = Request.QueryString["GroupID"].ToString();
            GroupName = Server.UrlDecode(Request.QueryString["GroupName"].ToString());
            if (!IsPostBack)
            {
                ViewState["hdnRowValue"] = "";
                ViewState["filter"] = "1=1";
                ViewState["CurrentPage"] = 1;

                try
                {
                    SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, this.gvGroupList, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
                }
                catch (Exception exp)
                {
                   JScript.Instance.ShowMessage(this.UpdatePanel1, exp.Message);
                }


                this.Label1.Text = GroupName + "  成员设置";

                this.Title = GroupName + "  成员设置";
            }
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.UpdatePanel1.GetType(), "Resize", "resize();", true);
    }
    protected void gvGroupList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = e.Row.DataItem as DataRowView;


            CheckBox btn = (CheckBox)e.Row.Cells[2].FindControl("cbSelect");
            btn.Text = "加入" + GroupName;

            if (drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("GroupName")].ToString() == GroupName.Trim())
            {
                btn.Checked = true;
                btn.Enabled = false;
            }

            if (ViewState["hdnRowValue"].ToString().Length > 0)
            {
                string[] Users = ViewState["hdnRowValue"].ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (((IList)Users).Contains("'" + e.Row.Cells[3].Text + "'"))
                {
                    btn.Checked = true;
                }
            }
            e.Row.Cells[3].Visible = false;

        }
        else if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[3].Visible = false;
        }
    }
    /// <summary>
    /// 保存当前组用户

    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            UpdateTempUser();
            string users = "-1,";

            if (ViewState["hdnRowValue"].ToString().Length > 0)
                users += ViewState["hdnRowValue"].ToString().Replace("'", "");



            users += "-1";

            BLL.BLLBase bll = new BLL.BLLBase();
            bll.ExecNonQuery("Security.UpdateUserGroup", new DataParameter[] { new DataParameter("@GroupID", GroupID), new DataParameter("{0}", users) });

            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "Resize", "Close('1');", true);

        }
        catch (Exception ex)
        {
            System.Diagnostics.StackFrame frame = new System.Diagnostics.StackFrame(0);
            Session["ModuleName"] = this.Page.Title;
            Session["FunctionName"] = frame.GetMethod().Name;
            Session["ExceptionalType"] = ex.GetType().FullName;
            Session["ExceptionalDescription"] = ex.Message;
            Response.Redirect("../../../Common/MistakesPage.aspx");

        }


    }


    private void UpdateTempUser()
    {
        string strID = "";
        for (int i = 0; i < this.gvGroupList.Rows.Count; i++)
        {
            CheckBox cb = (CheckBox)(this.gvGroupList.Rows[i].FindControl("cbSelect"));
            if (cb != null && cb.Checked)
            {
                strID += "'" + this.gvGroupList.Rows[i].Cells[3].Text + "',";
            }
            else
            {
                ViewState["hdnRowValue"] = ViewState["hdnRowValue"].ToString().Replace("'" + this.gvGroupList.Rows[i].Cells[3].Text + "',", "");
            }
        }
        ViewState["hdnRowValue"] = ViewState["hdnRowValue"].ToString() + strID;

    }

    #region  换页
    protected void btnFirst_Click(object sender, EventArgs e)
    {
        UpdateTempUser();
        ViewState["CurrentPage"] = 1;
        SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, gvGroupList, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);

    }

    protected void btnPre_Click(object sender, EventArgs e)
    {
        UpdateTempUser();
        ViewState["CurrentPage"] = int.Parse(ViewState["CurrentPage"].ToString()) - 1;
        SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, gvGroupList, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        UpdateTempUser();
        ViewState["CurrentPage"] = int.Parse(ViewState["CurrentPage"].ToString()) + 1;
        SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, gvGroupList, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
    }

    protected void btnLast_Click(object sender, EventArgs e)
    {
        UpdateTempUser();
        ViewState["CurrentPage"] = 0;
        SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, gvGroupList, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
    }

    protected void btnToPage_Click(object sender, EventArgs e)
    {
        UpdateTempUser();
        int PageIndex = 0;
        int.TryParse(this.txtPageNo.Text, out PageIndex);
        if (PageIndex == 0)
            PageIndex = 1;

        ViewState["CurrentPage"] = PageIndex;
        SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, gvGroupList, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
    }

    protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    #endregion

}