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
using System.Data.SqlClient;
using System.Drawing;
using System.Collections.Generic;
using Util;


public partial class WebUI_SysInfo_UserManage_UserList :BasePage
{
    int pageIndex = 1;
    int pageSize = 15;
    int totalCount = 0;
    int pageCount = 0;
    string filter = "1=1";

    string PrimaryKey = "UserID";
    string OrderByFields = "UserName";
    string TableView = "sys_UserList";
    DataTable dtUser;
    BLL.BLLBase bll = new BLL.BLLBase();
    #region 窗体加载
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            ViewState["filter"] = "1=1";
            ViewState["CurrentPage"] = 1;

            try
            {
                SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, gvMain, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
            }
            catch (Exception exp)
            {
                JScript.Instance.ShowMessage(this.UpdatePanel1, exp.Message);
            }

            writeJsvar(FormID, SqlCmd, "");
        }
        else
        {
            dtUser = SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, gvMain, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.UpdatePanel1.GetType(), "Resize", "resize();", true);
    }
    #endregion



    #region 显示切换
    private void SwitchView(int index)
    {
        if (index == 0)
        {
            this.dvList.Visible = true;
            this.dvEdit.Visible = false;
        }
        else
        {
            this.dvList.Visible = false;
            this.dvEdit.Visible = true;
        }
    }
    #endregion

    #region GridView绑定
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox chk = new CheckBox();

            chk.ID = "checkAll";
            chk.Attributes.Add("onclick", "checkboxChange(this,'gvMain',0);");
            chk.Text = "操作";
            e.Row.Cells[0].Controls.Add(chk);
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {


            CheckBox chk = new CheckBox();
            //chk.Attributes.Add("onclick","");
            LinkButton lkbtn = new LinkButton();
            lkbtn.CommandName = "Edit";
            lkbtn.ID = e.Row.ID;
            lkbtn.Text = " 编辑 ";
            if (this.hdnXGQX.Value == "0")
            {
                lkbtn.Enabled = false;
            }
            e.Row.Cells[0].Controls.Add(chk);
            e.Row.Cells[0].Controls.Add(lkbtn);

            Button btn = new Button();
            btn.CommandName = "EditUser";
            DataRowView drv = e.Row.DataItem as DataRowView;

            btn.OnClientClick = "return ShowUserArea('" + drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("UserID")].ToString() + "');";
            btn.CssClass = "ButtonModify";
            btn.Text = "仓库管理";
            e.Row.Cells[4].Controls.Add(btn);
            if (e.Row.Cells[1].Text.Trim() == "admin")
            {
                chk.Enabled = false;
                lkbtn.Enabled = false;
                btn.Enabled = false;
            }
        }
    }
    #endregion

    protected void btnEditUser_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.UpdatePanel1.GetType(), "Edit", "ShowUserArea('" + ((Button)sender).CommandArgument + "');", true);
    }
    #region 数据编辑
    protected void gvMain_RowEditing(object sender, GridViewEditEventArgs e)
    {
        hdnOpFlag.Value = "1";
        ViewState["OpFlag"] = "1";
        SwitchView(1);
        this.txtUserID.Text = dtUser.Rows[e.NewEditIndex]["UserID"].ToString();
        this.txtUserName.Text = dtUser.Rows[e.NewEditIndex]["UserName"].ToString();
        this.txtEmployeeCode.Text = dtUser.Rows[e.NewEditIndex]["EmployeeCode"].ToString();
        //this.txtEmployeeName.Text = dsUser.Tables[0].Rows[e.NewEditIndex]["EmployeeName"].ToString(); 
        this.txtMemo.Text = dtUser.Rows[e.NewEditIndex]["Memo"].ToString();
    }
    #endregion

    #region 按字段查询
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        try
        {
            filter = string.Format("{0} like '%{1}%'", this.ddl_Field.SelectedValue, this.txtKeyWords.Text.Trim().Replace("'", ""));
            ViewState["filter"] = filter;

            SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, gvMain, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);

        }
        catch (Exception exp)
        {
           JScript.Instance.ShowMessage(this.UpdatePanel1, exp.Message);
        }
    }
    #endregion

    #region 新增用户
    protected void btnCreate_Click(object sender, EventArgs e)
    {
        ClearData();
        this.hdnOpFlag.Value = "0";
        ViewState["OpFlag"] = "0";
        SwitchView(1);
    }
    #endregion

    #region 删除用户
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {

            string strUserID = "-1,";
            for (int i = 0; i < gvMain.Rows.Count; i++)
            {
                CheckBox chk = (CheckBox)gvMain.Rows[i].Cells[0].Controls[0];
                if (gvMain.Rows[i].Cells[1].Text == "admin" && chk.Checked)
                {
                   JScript.Instance.ShowMessage(this.UpdatePanel1, "管理员帐号不能删除！");
                    continue;
                }
                if (chk.Enabled && chk.Checked)
                {
                    strUserID += dtUser.Rows[i]["UserID"].ToString() + ",";
                }
            }
            strUserID += "-1";


            bll.ExecNonQuery("Security.DeleteUser", new DataParameter[] { new DataParameter("{0}", strUserID) });
            AddOperateLog("用户管理", "删除用户信息");
            SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, gvMain, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);


        }
        catch (Exception exp)
        {
           JScript.Instance.ShowMessage(this.UpdatePanel1, exp.Message);
        }
    }
    #endregion

    # region 分页控件 页码changing事件
    protected void btnFirst_Click(object sender, EventArgs e)
    {
        ViewState["CurrentPage"] = 1;
        SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, gvMain, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);

    }

    protected void btnPre_Click(object sender, EventArgs e)
    {
        ViewState["CurrentPage"] = int.Parse(ViewState["CurrentPage"].ToString()) - 1;
        SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, gvMain, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        ViewState["CurrentPage"] = int.Parse(ViewState["CurrentPage"].ToString()) + 1;
        SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, gvMain, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
    }

    protected void btnLast_Click(object sender, EventArgs e)
    {
        ViewState["CurrentPage"] = 0;
        SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, gvMain, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
    }

    protected void btnToPage_Click(object sender, EventArgs e)
    {
        int PageIndex = 0;
        int.TryParse(this.txtPageNo.Text, out PageIndex);
        if (PageIndex == 0)
            PageIndex = 1;

        ViewState["CurrentPage"] = PageIndex;
        SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, gvMain, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
    }
    #endregion

    #region 数据保存
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            BLL.Security.UserBll ubll = new BLL.Security.UserBll();
            if (ViewState["OpFlag"].ToString() == "0")//新增
            {
                DataTable dtTemp = ubll.GetUserInfo(this.txtUserName.Text.Trim());
                if (dtTemp.Rows.Count > 0)
                {
                    JScript.Instance.ShowMessage(this.UpdatePanel1, "该用户名已经存在！");
                    return;
                }
                if (this.txtEmployeeCode.Text.Trim() == "")
                {
                    txtEmployeeCode.Text = txtUserName.Text.Trim();
                }
                ubll.InsertUser(this.txtUserName.Text.Trim(), this.txtEmployeeCode.Text, this.txtMemo.Text);


                SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, gvMain, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
                SwitchView(0);
               JScript.Instance.ShowMessage(this.UpdatePanel1, "数据添加成功！");
                AddOperateLog("用户管理", "添加用户信息");
            }
            else//修改
            {
                foreach (DataRow dr in dtUser.Rows)
                {
                    if (dr["UserID"].ToString() == this.txtUserID.Text.Trim())
                    {
                        DataTable dtTemp = ubll.GetUserList(1, 10, string.Format("UserID<>{0} and UserName='{1}'", this.txtUserID.Text, this.txtUserName.Text.Trim()), OrderByFields);
                        if (dtTemp.Rows.Count > 0)
                        {
                            JScript.Instance.ShowMessage(this.UpdatePanel1, "该用户名已经存在！");
                            return;
                        }

                        ubll.UpdateUser(this.txtUserName.Text.Trim(), this.txtEmployeeCode.Text.Trim(), this.txtMemo.Text.Trim(), int.Parse(this.txtUserID.Text));
                        break;
                    }
                }
                this.gvMain.EditIndex = -1;

                SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, gvMain, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
                JScript.Instance.ShowMessage(this.UpdatePanel1, "数据修改成功！");
                SwitchView(0);
                AddOperateLog("用户管理", "修改用户信息");
            }


        }
        catch (Exception exp)
        {
           JScript.Instance.ShowMessage(this.UpdatePanel1, exp.Message);
        }
    }
    #endregion

    #region 取消
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.gvMain.EditIndex = -1;
        ClearData();
        SwitchView(0);
        SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, gvMain, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
    }

    protected void ClearData()
    {
        this.txtUserID.Text = "";
        this.txtUserName.Text = "";
        this.txtEmployeeCode.Text = "";
        //this.txtEmployeeName.Text = "";
        this.txtMemo.Text = "";
    }
    #endregion
}