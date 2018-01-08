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

public partial class WebUI_SysInfo_RoleManage_RoleManage : BasePage
{
    private string strTableView = "v_sys_GroupList";
    private string strPrimaryKey = "GroupID";
    //private string strOrderByFields = "GroupName ASC";
    private string strQueryFields = "GroupID,GroupName,Memo,State";
    bool PostBack = false;
    BLL.BLLBase bll = new BLL.BLLBase();

    #region 窗体加载
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (!IsPostBack)
            {
                InitSmartTree();

                ViewState["filter"] = "1=1";
                ViewState["CurrentPage"] = 1;
                DataTable dt = SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), 5, gvGroupList, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
                SetBindDataSub(dt);

            }



        }
        catch (Exception exp)
        {

        }
        lblCurrentPage.Visible = false;
        lblCurrentPageSub1.Visible = false;
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.UpdatePanel1.GetType(), "Resize", "resize();", true);
    }
    #endregion

    /// <summary>
    /// GridView行绑定事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvGroupList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowIndex == Convert.ToInt32(this.hdnRowIndex.Value))
            {
                e.Row.BackColor = ColorTranslator.FromHtml("#60c0ff");
            }
            e.Row.Attributes.Add("onclick", string.Format(" $('#hdnRowGroupName').val('{2}');$('#hdnRowValue').val('{1}');selectRow({0});", e.Row.RowIndex, e.Row.Cells[0].Text, e.Row.Cells[1].Text));
            e.Row.Attributes.Add("style", "cursor:pointer;");
        }

    }

    /// <summary>
    /// GridView行绑定事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvGroupListUser_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Visible = false;
        }
        else if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[0].Visible = false;
        }
    }



    #region 主档事件
    protected void btnFirst_Click(object sender, EventArgs e)
    {
        ViewState["CurrentPage"] = 1;
        this.hdnRowIndex.Value = "0";
        DataTable dt = SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), 5, gvGroupList, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
        lblCurrentPage.Visible = false;
        SetBindDataSub(dt);
    }


    protected void btnPre_Click(object sender, EventArgs e)
    {
        ViewState["CurrentPage"] = int.Parse(ViewState["CurrentPage"].ToString()) - 1;
        this.hdnRowIndex.Value = "0";
        DataTable dt = SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), 5, gvGroupList, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
        lblCurrentPage.Visible = false;
        SetBindDataSub(dt);
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        ViewState["CurrentPage"] = int.Parse(ViewState["CurrentPage"].ToString()) + 1;
        this.hdnRowIndex.Value = "0";
        DataTable dt = SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), 5, gvGroupList, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
        lblCurrentPage.Visible = false;
        SetBindDataSub(dt);
    }

    protected void btnLast_Click(object sender, EventArgs e)
    {
        ViewState["CurrentPage"] = 0;
        this.hdnRowIndex.Value = "0";
        DataTable dt = SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), 5, gvGroupList, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
        lblCurrentPage.Visible = false;
        SetBindDataSub(dt);
    }

    protected void btnToPage_Click(object sender, EventArgs e)
    {
        int PageIndex = 0;
        int.TryParse(this.txtPageNo.Text, out PageIndex);
        if (PageIndex == 0)
            PageIndex = 1;

        ViewState["CurrentPage"] = PageIndex;
        this.hdnRowIndex.Value = "0";
        DataTable dt = SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), 5, gvGroupList, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
        lblCurrentPage.Visible = false;
        SetBindDataSub(dt);
    }

    private void SetBindDataSub(DataTable dt)
    {
        string BillID = "";
        if (dt.Rows.Count > 0)
        {
            BillID = dt.Rows[0]["GroupID"].ToString();
            this.hdnRowValue.Value = dt.Rows[0]["GroupID"].ToString();
            hdnRowGroupName.Value = dt.Rows[0]["GroupName"].ToString();

        }
        BindDataSub(BillID);
    }
    public override void BindDataSub(string BillID)
    {
        //string script = string.Format("document.getElementById('iframeRoleSet').src='RoleSet.aspx?GroupID={0}&GroupName={1}' ;", this.hdnRowValue.Value, Server.UrlEncode(hdnRowGroupName.Value));
        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.UpdatePanel1.GetType(), "", script, true);
        string GroupName = hdnRowGroupName.Value;
        this.lbTitle.Text = "用户组 <font color='Gray'>" + GroupName + "</font> 权限设置";
        //InitSmartTree();
        //this.sTreeModule.ExpandDepth = 1;
        GroupOperationBind();
        if (GroupName == "admin")
        {
            this.lnkBtnSave.Enabled = false;
        }
        else
        {
            this.lnkBtnSave.Enabled = true;
        }



        BLL.BLLBase bll = new BLL.BLLBase();
        DataTable dtSub = bll.FillDataTable("Security.SelectGroupUser", new DataParameter[] { new DataParameter("@GroupID", BillID) });
        ViewState[FormID + "_S_gvGroupListUser"] = dtSub;
        this.gvGroupListUser.DataSource = dtSub;
        this.gvGroupListUser.DataBind();
        MovePage("S", this.gvGroupListUser, 0, btnFirstSub1, btnPreSub1, btnNextSub1, btnLastSub1, btnToPageSub1, lblCurrentPageSub1);
        lblCurrentPageSub1.Visible = false;

    }

    #endregion

    #region 子表绑定

    protected void btnFirstSub1_Click(object sender, EventArgs e)
    {
        MovePage("S", this.gvGroupListUser, 0, btnFirstSub1, btnPreSub1, btnNextSub1, btnLastSub1, btnToPageSub1, lblCurrentPageSub1);
        lblCurrentPageSub1.Visible = false;
    }

    protected void btnPreSub1_Click(object sender, EventArgs e)
    {
        MovePage("S", this.gvGroupListUser, this.gvGroupListUser.PageIndex - 1, btnFirstSub1, btnPreSub1, btnNextSub1, btnLastSub1, btnToPageSub1, lblCurrentPageSub1);
        lblCurrentPageSub1.Visible = false;
    }

    protected void btnNextSub1_Click(object sender, EventArgs e)
    {
        MovePage("S", this.gvGroupListUser, this.gvGroupListUser.PageIndex + 1, btnFirstSub1, btnPreSub1, btnNextSub1, btnLastSub1, btnToPageSub1, lblCurrentPageSub1);
        lblCurrentPageSub1.Visible = false;
    }

    protected void btnLastSub1_Click(object sender, EventArgs e)
    {
        MovePage("S", this.gvGroupListUser, this.gvGroupListUser.PageCount - 1, btnFirstSub1, btnPreSub1, btnNextSub1, btnLastSub1, btnToPageSub1, lblCurrentPageSub1);
        lblCurrentPageSub1.Visible = false;
    }

    protected void btnToPageSub1_Click(object sender, EventArgs e)
    {
        MovePage("S", this.gvGroupListUser, int.Parse(this.txtPageNoSub1.Text) - 1, btnFirstSub1, btnPreSub1, btnNextSub1, btnLastSub1, btnToPageSub1, lblCurrentPageSub1);
        lblCurrentPageSub1.Visible = false;
    }



    #endregion

    protected void btnReload_Click(object sender, EventArgs e)
    {
        BtnReloadSub(Convert.ToInt32(this.hdnRowIndex.Value), this.hdnRowValue.Value, this.gvGroupList);
    }


    protected void btnDeleteUser_Click(object sender, EventArgs e)
    {

        bll.ExecNonQuery("Security.UpdateUserGroup", new DataParameter[] { new DataParameter("@GroupID", 0), new DataParameter("{0}", ((Button)sender).CommandArgument) });


        BindDataSub(this.hdnRowValue.Value);
    }

    protected void btnAddUser_Click(object sender, EventArgs e)
    {
        BindDataSub(this.hdnRowValue.Value);
        int i = Convert.ToInt32(this.hdnRowIndex.Value);

        for (int j = 0; j < this.gvGroupList.Rows.Count; j++)
        {
            if (j % 2 == 0)
                this.gvGroupList.Rows[j].BackColor = ColorTranslator.FromHtml("#ffffff");
            else
                this.gvGroupList.Rows[j].BackColor = ColorTranslator.FromHtml("#E9F2FF");
            if (j == i)
                this.gvGroupList.Rows[j].BackColor = ColorTranslator.FromHtml("#60c0ff");
        }
    }


    protected void lnkBtnSave_Click(object sender, EventArgs e)
    {
        string GroupID = this.hdnRowValue.Value;




        bll.ExecNonQuery("Security.DeleteGroupOperation", new DataParameter[] { new DataParameter("@GroupID", GroupID), new DataParameter("@SystemName", "WMS") });

        foreach (TreeNode tnRoot in this.sTreeModule.Nodes)
        {
            foreach (TreeNode tnSub in tnRoot.ChildNodes)
            {
                foreach (TreeNode tnOp in tnSub.ChildNodes)
                {
                    if (tnOp.Checked)
                    {

                        string ModuleID = tnOp.Value;
                        bll.ExecNonQuery("Security.InsertGroupOperation", new DataParameter[] { new DataParameter("@GroupID", GroupID), new DataParameter("@ModuleID", ModuleID) });

                    }
                }
            }
        }

        JScript.Instance.ShowMessage(this.UpdatePanel1, "保存成功！");
        BindDataSub(this.hdnRowValue.Value);


    }

    public void InitSmartTree()
    {
        this.sTreeModule.Nodes.Clear();
        DataTable dtModules = bll.FillDataTable("Security.SelectSystemModules", new DataParameter[] { new DataParameter("@SystemName", "WMS") });
        DataTable dtSubModules = bll.FillDataTable("Security.SelectSystemSubModules", new DataParameter[] { new DataParameter("@SystemName", "WMS") });
        DataTable dtOperations = bll.FillDataTable("Security.SelectSystemOperations", new DataParameter[] { new DataParameter("@SystemName", "WMS") });
        foreach (DataRow dr in dtModules.Rows)
        {
            TreeNode tnRoot = new TreeNode(dr["MenuTitle"].ToString(), dr["ModuleCode"].ToString().Trim());
            tnRoot.SelectAction = TreeNodeSelectAction.Expand;
            tnRoot.ShowCheckBox = true;
            this.sTreeModule.Nodes.Add(tnRoot);
        }
        //为第一级菜单增加子级菜单


        if (dtModules.Rows.Count > 0)
        {
            foreach (DataRow drSub in dtSubModules.Rows)
            {
                for (int i = 0; i < sTreeModule.Nodes.Count; i++)
                {
                    if (sTreeModule.Nodes[i].Value == drSub["ModuleCode"].ToString().Trim())
                    {
                        TreeNode tnChild = new TreeNode(drSub["MenuTitle"].ToString(), drSub["SubModuleCode"].ToString().Trim());
                        tnChild.ShowCheckBox = true;
                        tnChild.SelectAction = TreeNodeSelectAction.Expand;
                        this.sTreeModule.Nodes[i].ChildNodes.Add(tnChild);
                        break;
                    }

                }
            }
        }

        foreach (DataRow drOp in dtOperations.Rows)
        {
            for (int i = 0; i < sTreeModule.Nodes.Count; i++)
            {
                for (int j = 0; j < sTreeModule.Nodes[i].ChildNodes.Count; j++)
                {
                    if (sTreeModule.Nodes[i].ChildNodes[j].Value == drOp["SubModuleCode"].ToString().Trim())
                    {
                        TreeNode tnOp = new TreeNode(drOp["OperatorDescription"].ToString(), drOp["ModuleID"].ToString());
                        tnOp.ShowCheckBox = true;
                        tnOp.SelectAction = TreeNodeSelectAction.None;
                        sTreeModule.Nodes[i].ChildNodes[j].ChildNodes.Add(tnOp);
                    }
                }
            }
        }

    }

    public void GroupOperationBind()
    {
        DataTable dtOP = bll.FillDataTable("Security.SelectGroupOperation", new DataParameter[] { new DataParameter("@GroupID", this.hdnRowValue.Value) });


        foreach (TreeNode tnRoot in this.sTreeModule.Nodes)
        {
            bool IsAllSelected = false;
            foreach (TreeNode tnSub in tnRoot.ChildNodes)
            {
                bool IsSubAllSelected = false;
                foreach (TreeNode tnOp in tnSub.ChildNodes)
                {
                    tnOp.Checked = false;
                    DataRow[] drs = dtOP.Select(string.Format("ModuleID={0}", tnOp.Value));
                    if (drs.Length > 0)
                    {
                        tnOp.Checked = true;
                    }
                    if (tnOp.Checked)
                    {
                        IsSubAllSelected = true;
                    }
                }
                if (IsSubAllSelected)
                {
                    tnSub.Checked = true;
                    IsAllSelected = true;
                }
                else
                {
                    tnSub.Checked = false;
                }
            }
            if (IsAllSelected)
            {
                tnRoot.Checked = true;
            }
            else
            {
                tnRoot.Checked = false;
            }
        }



    }

    protected void lnkBtnCollapse_Click(object sender, EventArgs e)
    {
        //this.tvModuleList.CollapseAll();
        this.sTreeModule.CollapseAll();
    }
    protected void lnkBtnExpand_Click(object sender, EventArgs e)
    {
        //this.tvModuleList.ExpandAll();
        this.sTreeModule.ExpandAll();
    }
 
}