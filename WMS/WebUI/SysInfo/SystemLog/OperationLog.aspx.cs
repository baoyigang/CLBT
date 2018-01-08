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
public partial class WebUI_SysInfo_SystemLog_OperationLog : BasePage
{
    int pageIndex = 1;
    int pageSize = 15;
    int totalCount = 0;
    int pageCount = 0;
    string filter = "1=1";
    DataTable dtLog;
    string PrimaryKey = "OperatorLogID";
    string OrderByFields = "LoginTime desc ";
    string TableName = "sys_OperatorLog";
    string strQueryFields = "OperatorLogID,LoginUser,LoginTime,LoginModule,ExecuteOperator";
    BLL.BLLBase bll = new BLL.BLLBase();

    #region 窗体加裁
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["filter"] = "1=1";
            ViewState["CurrentPage"] = 1;
            this.btnDeleteAll.Enabled = this.btnDelete.Enabled;
            try
            {
                dtLog = SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, gvMain, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
            }
            catch (Exception exp)
            {
                JScript.Instance.ShowMessage(this.UpdatePanel1, exp.Message);
            }

            writeJsvar(FormID, SqlCmd, "");
        }
        else
        {
            dtLog = SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, gvMain, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.UpdatePanel1.GetType(), "Resize", "resize();", true);
    }
    #endregion

    #region 数据源绑定
    void GridDataBind()
    {
        dtLog = bll.GetDataPage("Security.SeleteOperatorLog", pageIndex, pageSize, out totalCount, out pageCount, new DataParameter[] { new DataParameter("{0}", filter), new DataParameter("{1}", OrderByFields) });
        if (dtLog.Rows.Count == 0)
        {
            dtLog.Rows.Add(dtLog.NewRow());
            gvMain.DataSource = dtLog;
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
            this.gvMain.DataSource = dtLog;
            this.gvMain.DataBind();
        }

        ViewState["pageIndex"] = pageIndex;
        ViewState["totalCount"] = totalCount;
        ViewState["pageCount"] = pageCount;
        ViewState["filter"] = filter;
        ViewState["OrderByFields"] = OrderByFields;
    }
    #endregion

    #region GridView绑定
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    #endregion

    #region 删除日志
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            string strGroupID = "-1,";
            for (int i = 0; i < gvMain.Rows.Count; i++)
            {
                CheckBox chk = (CheckBox)(this.gvMain.Rows[i].FindControl("cbSelect"));
                if (chk.Enabled && chk.Checked)
                {
                    strGroupID += dtLog.Rows[i]["OperatorLogID"].ToString() + ",";
                }
            }
            strGroupID += "-1";
            bll.ExecNonQuery("Security.DeleteOperatorLog", new DataParameter[] { new DataParameter("{0}", strGroupID) });

            AddOperateLog("操作日志管理", "删除操作日志");
            dtLog = SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, gvMain, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
        }
        catch (Exception exp)
        {
            JScript.Instance.ShowMessage(this.UpdatePanel1, exp.Message);
        }
    }
    #endregion


    #region 清空日志
    protected void btnDeleteAll_Click(object sender, EventArgs e)
    {
        try
        {
            bll.ExecNonQuery("Security.DeleteAllOperatorLog", null);

            AddOperateLog("操作日志管理", "清空操作日志");
            dtLog = SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, gvMain, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
        }
        catch (Exception exp)
        {
            JScript.Instance.ShowMessage(this.UpdatePanel1, exp.Message);
        }
    }
    #endregion

    #region 查询
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string start = "1900-01-01";
        string end = System.DateTime.Now.AddDays(1).ToString();
        try
        {
            if (this.txtStartDate.tDate.Text.Trim().Length > 0)
            {
                start = Convert.ToDateTime(this.txtStartDate.tDate.Text.Trim()).ToString();
            }
            if (this.txtEndDate.tDate.Text.Trim().Length > 0)
            {
                end = Convert.ToDateTime(this.txtEndDate.tDate.Text.Trim()).AddDays(1).ToString();
            }


            filter = string.Format("{0} like '{1}%' and (LoginTime>='{2}' and LoginTime<'{3}')", this.ddl_Field.SelectedValue, this.txtKeyWords.Text.Trim().Replace("'", ""), start, end);
            ViewState["filter"] = filter;
            dtLog = SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, gvMain, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
        }
        catch (Exception ex)
        {
            JScript.Instance.ShowMessage(this.UpdatePanel1, ex.Message);
            return;
        }
    }
    #endregion

    # region 分页控件 页码changing事件
    protected void btnFirst_Click(object sender, EventArgs e)
    {
        ViewState["CurrentPage"] = 1;
        dtLog = SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, gvMain, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);

    }

    protected void btnPre_Click(object sender, EventArgs e)
    {
        ViewState["CurrentPage"] = int.Parse(ViewState["CurrentPage"].ToString()) - 1;
        dtLog = SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, gvMain, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        ViewState["CurrentPage"] = int.Parse(ViewState["CurrentPage"].ToString()) + 1;
        dtLog = SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, gvMain, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
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
        dtLog = SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, gvMain, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
    }
    #endregion
}