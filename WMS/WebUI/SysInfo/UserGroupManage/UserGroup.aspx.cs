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
using Util;

public partial class WebUI_SysInfo_UserGroupManage_UserGroup : BasePage
{
    int pageIndex = 1;
    int pageSize = 15;
    int totalCount = 0;
    int pageCount = 0;
    string filter = "1=1";
    DataTable dtGroup;
    string PrimaryKey = "GroupID";
    string OrderByFields = "GroupName";
    string TableName = "v_sys_GroupList";
    private string strQueryFields = "GroupID,GroupName,Memo,State";

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
            dtGroup = SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, gvMain, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.UpdatePanel1.GetType(), "Resize", "resize();", true);
    }
    #endregion

    #region 数据源绑定
    void GridDataBind()
    {
        dtGroup = bll.GetDataPage("Security.SelectGroup", pageIndex, pageSize, out totalCount, out pageCount, new DataParameter[] { new DataParameter("{0}", filter) });
        if (dtGroup.Rows.Count == 0)
        {
            dtGroup.Rows.Add(dtGroup.NewRow());
            gvMain.DataSource = dtGroup;
            gvMain.DataBind();
            int columnCount = gvMain.Rows[0].Cells.Count;
            gvMain.Rows[0].Cells.Clear();
            gvMain.Rows[0].Cells.Add(new TableCell());
            gvMain.Rows[0].Cells[0].ColumnSpan = columnCount;
            gvMain.Rows[0].Cells[0].Text = "没有符合以上条件的数据,请重新查询 ";
            gvMain.Rows[0].Visible = true;

        }
        else
        {
            this.gvMain.DataSource = dtGroup;
            this.gvMain.DataBind();
        }

        ViewState["pageIndex"] = pageIndex;
        ViewState["totalCount"] = totalCount;
        ViewState["pageCount"] = pageCount;
        ViewState["filter"] = filter;
        ViewState["OrderByFields"] = OrderByFields;
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
            if (e.Row.Cells[1].Text.Trim() == "admin")
            {
                chk.Enabled = false;
                lkbtn.Enabled = false;
            }
        }
    }
    #endregion

    #region 数据编辑
    protected void gvMain_RowEditing(object sender, GridViewEditEventArgs e)
    {
        hdnOpFlag.Value = "1";
        ViewState["OpFlag"] = "1";
        SwitchView(1);
        this.txtGroupID.Text = dtGroup.Rows[e.NewEditIndex]["GroupID"].ToString();
        this.txtGroupName.Text = dtGroup.Rows[e.NewEditIndex]["GroupName"].ToString();
        this.txtMemo.Text = dtGroup.Rows[e.NewEditIndex]["Memo"].ToString();
    }
    #endregion

    #region 按字段查询
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        try
        {
            filter = string.Format("{0} like '%{1}%'", this.ddl_Field.SelectedValue, this.txtKeyWords.Text.Trim().Replace("'", ""));
            ViewState["filter"] = filter;


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

    #region 删除用户组
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            string strGroupID = "-1,";
            for (int i = 0; i < gvMain.Rows.Count; i++)
            {
                CheckBox chk = (CheckBox)gvMain.Rows[i].Cells[0].Controls[0];
                if (chk.Enabled && chk.Checked)
                {
                    int membercount = (int)bll.ExecScalar("Security.SelectGroupMemberCount", new DataParameter[] { new DataParameter("@GroupID", Convert.ToInt32(dtGroup.Rows[i]["GroupID"])) });
                    if (membercount > 0)
                    {
                        JScript.Instance.ShowMessage(this.UpdatePanel1, gvMain.Rows[i].Cells[1].Text + "用户组还有用户存在，请调整后再删除！");
                        return;
                    }

                    strGroupID += dtGroup.Rows[i]["GroupID"].ToString() + ",";

                }
            }
            strGroupID += "-1";


            bll.ExecNonQuery("Security.DeleteGroup", new DataParameter[] { new DataParameter("{0}", strGroupID) });
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
            if (ViewState["OpFlag"].ToString() == "0")//新增
            {
                int Count = bll.GetRowCount(TableName, string.Format("GroupName='{0}'", this.txtGroupName.Text.Trim()));
                if (Count > 0)
                {
                    JScript.Instance.ShowMessage(this.UpdatePanel1, "该用户组名称已经存在！");
                    return;
                }

                bll.ExecNonQuery("Security.InsertGroup", new DataParameter[] { new DataParameter("@GroupName", this.txtGroupName.Text.Trim()), new DataParameter("@Memo", this.txtMemo.Text.Trim()), new DataParameter("@State", 1) });



                SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, gvMain, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
                SwitchView(0);
                JScript.Instance.ShowMessage(this.UpdatePanel1, "数据添加成功！");
                AddOperateLog("用户管理", "添加用户信息");
            }
            else//修改
            {
                foreach (DataRow dr in dtGroup.Rows)
                {
                    if (dr["GroupID"].ToString() == this.txtGroupID.Text.Trim())
                    {
                        int Count = bll.GetRowCount(TableName, string.Format("GroupID<>{0} and GroupName='{1}'", this.txtGroupID.Text, this.txtGroupName.Text.Trim()));
                        if (Count > 0)
                        {
                            JScript.Instance.ShowMessage(this.UpdatePanel1, "该用户组名称已经存在！");
                            return;
                        }
                        bll.ExecNonQuery("Security.UpdateGroupInfo", new DataParameter[] { new DataParameter("@GroupName", this.txtGroupName.Text.Trim()), new DataParameter("@Memo", this.txtMemo.Text.Trim()), new DataParameter("@GroupID", this.txtGroupID.Text.Trim()) });

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
        GridDataBind();
    }

    protected void ClearData()
    {
        this.txtGroupID.Text = "";
        this.txtGroupName.Text = "";
        this.txtMemo.Text = "";
    }
    #endregion
}