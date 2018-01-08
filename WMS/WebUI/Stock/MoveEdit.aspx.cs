 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Util;


public partial class WebUI_Stock_MoveEdit : BasePage
{
    protected string strID;
    BLL.BLLBase bll = new BLL.BLLBase();
    protected void Page_Load(object sender, EventArgs e)
    {
        strID = Request.QueryString["ID"] + "";
        this.dgViewSub1.PageSize = pageSubSize;
        if (!IsPostBack)
        {
            BindDropDownList();
            if (strID != "")
            {
                DataTable dt = bll.FillDataTable("WMS.SelectBillMaster", new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}'", strID)) });
                BindData(dt);

                SetTextReadOnly(this.txtID);
            }
            else
            {
                BindDataSub();

                txtBillDate.changed = "$('#txtID').val(autoCodeByTableName('MS', '1=1','WMS_BillMaster', 'txtBillDate'));";
                this.txtBillDate.DateValue = DateTime.Now;
                this.txtID.Text = bll.GetAutoCodeByTableName("MS", "WMS_BillMaster", DateTime.Now, "1=1");

                this.txtCreator.Text = Session["EmployeeCode"].ToString();
                this.txtUpdater.Text = Session["EmployeeCode"].ToString();
                this.txtCreatDate.Text = ToYMD(DateTime.Now);
                this.txtUpdateDate.Text = ToYMD(DateTime.Now);
            }
        }

        ScriptManager.RegisterStartupScript(this.updatePanel1, this.updatePanel1.GetType(), "Resize", "resize();BindEvent();", true);
        writeJsvar(FormID, SqlCmd, strID);
        SetTextReadOnly(this.txtCreator, this.txtCreatDate, this.txtUpdater, this.txtUpdateDate);


    }

    private void BindDropDownList()
    {
        DataTable dtArea = bll.FillDataTable("Security.SelectUserAreaByWhere", new DataParameter[] { new DataParameter("{0}", string.Format("UserName='{0}'", Session["G_user"].ToString())) });
        this.ddlAreaCode.DataValueField = "AreaCode";
        this.ddlAreaCode.DataTextField = "AreaName";
        this.ddlAreaCode.DataSource = dtArea;
        this.ddlAreaCode.DataBind();



    }


    private void BindData(DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            this.txtID.Text = dt.Rows[0]["BillID"].ToString();
            this.txtBillDate.DateValue = dt.Rows[0]["BillDate"];
            this.ddlAreaCode.SelectedValue = dt.Rows[0]["AreaCode"].ToString();

            this.txtMemo.Text = dt.Rows[0]["Memo"].ToString();
            this.txtCreator.Text = dt.Rows[0]["Creator"].ToString();
            this.txtCreatDate.Text = ToYMD(dt.Rows[0]["CreateDate"]);
            this.txtUpdater.Text = dt.Rows[0]["Updater"].ToString();
            this.txtUpdateDate.Text = ToYMD(dt.Rows[0]["UpdateDate"]);
        }
        BindDataSub();
    }

    private void BindDataSub()
    {
        DataTable dt = bll.FillDataTable("WMS.SelectBillDetail", new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}'", this.txtID.Text)) });
        ViewState[FormID + "_Edit_dgViewSub1"] = dt;
        this.dgViewSub1.DataSource = dt;
        this.dgViewSub1.DataBind();
        MovePage("Edit", this.dgViewSub1, 0, btnFirstSub1, btnPreSub1, btnNextSub1, btnLastSub1, btnToPageSub1, lblCurrentPageSub1);

    }

    protected void dgViewSub1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = e.Row.DataItem as DataRowView;
            SetTextReadOnly((TextBox)e.Row.FindControl("NewCellCode"));
            ((Label)e.Row.FindControl("RowID")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("RowID")].ToString();
            ((Label)e.Row.FindControl("OldCellCode")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("CellCode")].ToString();
            ((TextBox)e.Row.FindControl("NewCellCode")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("NewCellCode")].ToString();
            ((TextBox)e.Row.FindControl("SubMemo")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("Memo")].ToString();
        }
    }

    protected void btnAddDetail_Click(object sender, EventArgs e)
    {
        UpdateTempSub(this.dgViewSub1);
        DataTable dt = (DataTable)ViewState[FormID + "_Edit_dgViewSub1"];
        DataTable dt1 = Util.JsonHelper.Json2Dtb(hdnMulSelect.Value);
        int RowIndex = dt.Rows.Count;
        for (int i = 0; i < dt1.Rows.Count; i++)
        {
            DataRow dr = dt.NewRow();
            dr["RowID"] = RowIndex + i + 1;

            dr["BillID"] = this.txtID.Text.Trim();
            dr["ProductCode"] = dt1.Rows[i]["ProductCode"];
            dr["ProductName"] = dt1.Rows[i]["ProductName"];
            dr["CellCode"] = dt1.Rows[i]["CellCode"];
            dr["Quantity"] = dt1.Rows[i]["Quantity"];
            dr["Spec"] = dt1.Rows[i]["Spec"];
            dr["Propertity"] = dt1.Rows[i]["Propertity"];
            dr["ModelNo"] = dt1.Rows[i]["ModelNo"];
            dr["StandardNo"] = dt1.Rows[i]["StandardNo"];
            dr["PartNo"] = dt1.Rows[i]["PartNo"];
            dr["BarCode"] = dt1.Rows[i]["BarCode"];
            dt.Rows.Add(dr);

        }

        this.dgViewSub1.DataSource = dt;
        this.dgViewSub1.DataBind();
        ViewState[FormID + "_Edit_dgViewSub1"] = dt;

        MovePage("Edit", this.dgViewSub1, this.dgViewSub1.PageCount, btnFirstSub1, btnPreSub1, btnNextSub1, btnLastSub1, btnToPageSub1, lblCurrentPageSub1);
    }
    protected void btnDelDetail_Click(object sender, EventArgs e)
    {
        UpdateTempSub(this.dgViewSub1);
        DataTable dt = (DataTable)ViewState[FormID + "_Edit_" + dgViewSub1.ID];
        int RowID = 0;
        for (int i = 0; i < this.dgViewSub1.Rows.Count; i++)
        {
            CheckBox cb = (CheckBox)(this.dgViewSub1.Rows[i].FindControl("cbSelect"));
            if (cb != null && cb.Checked && cb.Enabled)
            {
                Label hk = (Label)(this.dgViewSub1.Rows[i].Cells[1].FindControl("RowID"));
                RowID = int.Parse(hk.Text);
                DataRow[] drs = dt.Select(string.Format("RowID ={0}", hk.Text));
                for (int j = 0; j < drs.Length; j++)
                    dt.Rows.Remove(drs[j]);

            }
        }
        DataRow[] drExists = dt.Select("", "RowID");
        for (int i = 0; i < drExists.Length; i++)
        {
            drExists[i]["RowID"] = i + 1;
        }
        this.dgViewSub1.DataSource = dt;
        this.dgViewSub1.DataBind();
        ViewState[FormID + "_Edit_" + dgViewSub1.ID] = dt;
        MovePage("Edit", this.dgViewSub1, this.dgViewSub1.PageIndex, btnFirstSub1, btnPreSub1, btnNextSub1, btnLastSub1, btnToPageSub1, lblCurrentPageSub1);

    }









    public override void UpdateTempSub(GridView dgv)
    {
        DataTable dt1 = (DataTable)ViewState[FormID + "_Edit_" + dgv.ID];
        if (dt1.Rows.Count == 0)
        {
            hdnOldCellCode.Value = "";
            hdnNewCellCode.Value = "";
            this.ddlAreaCode.Enabled = true;
            return;
        }
        DataRow dr;
        string strOldCellCode = "";
        string strNewCellCode = "";
        for (int i = 0; i < dgv.Rows.Count; i++)
        {
            dr = dt1.Rows[i + dgv.PageSize * dgv.PageIndex];
            dr.BeginEdit();

            dr["BillID"] = this.txtID.Text.Trim();
            dr["RowID"] = ((Label)dgv.Rows[i].FindControl("RowID")).Text;
            dr["NewCellCode"] = ((TextBox)dgv.Rows[i].FindControl("NewCellCode")).Text;
            dr["CellCode"] = ((Label)dgv.Rows[i].FindControl("OldCellCode")).Text;
            dr["Memo"] = ((TextBox)dgv.Rows[i].FindControl("SubMemo")).Text;
            dr.EndEdit();
            strOldCellCode += "'" + dr["CellCode"].ToString() + "',";
            strNewCellCode += "'" + dr["NewCellCode"].ToString() + "',";
        }
        hdnOldCellCode.Value += strOldCellCode;
        hdnNewCellCode.Value += strNewCellCode;
        dt1.AcceptChanges();
        if (dt1.Rows.Count > 0)
            this.ddlAreaCode.Enabled = false;


        object o = dt1.Compute("SUM(Quantity)", "");
        this.txtTotalQty.Text = o.ToString();
        ViewState[FormID + "_Edit_" + dgv.ID] = dt1;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        UpdateTempSub(this.dgViewSub1);
        string[] Commands = new string[3];
        DataParameter[] para;
        if (strID == "") //新增
        {
            int Count = bll.GetRowCount("WMS_BillMaster", string.Format("BillID='{0}'", this.txtID.Text.Trim()));
            if (Count > 0)
            {
                JScript.Instance.ShowMessage(this.updatePanel1, "该移库库单已经存在！");
                return;
            }
            para = new DataParameter[] { 
                                             new DataParameter("@BillID", this.txtID.Text.Trim()),
                                             new DataParameter("@BillDate", this.txtBillDate.DateValue),
                                             new DataParameter("@BillTypeCode","030"),
                                             new DataParameter("@AreaCode",this.ddlAreaCode.SelectedValue),
                                             new DataParameter("@Memo", this.txtMemo.Text.Trim()),
                                             new DataParameter("@Creator", Session["EmployeeCode"].ToString()),
                                             new DataParameter("@Updater", Session["EmployeeCode"].ToString())
                                             
                                              };
            Commands[0] = "WMS.InsertMoveStockBill";

        }
        else //修改
        {
            para = new DataParameter[] { 
                                             new DataParameter("@BillDate", this.txtBillDate.DateValue),
                                             new DataParameter("@AreaCode",this.ddlAreaCode.SelectedValue),
                                             new DataParameter("@Memo", this.txtMemo.Text.Trim()),
                                             new DataParameter("@Updater", Session["EmployeeCode"].ToString()),
                                             new DataParameter("{0}",string.Format("BillID='{0}'", this.txtID.Text.Trim())) };
            Commands[0] = "WMS.UpdateMoveStock";
        }
        DataTable dt = (DataTable)ViewState[FormID + "_Edit_dgViewSub1"];
        //判断货位是否被其他单据锁定
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            int count = 0;
            count = bll.GetRowCount("Cmd_Cell", string.Format("CellCode='{0}' and IsLock=1", dt.Rows[i]["CellCode"]));
            if (count > 0)
            {
                JScript.Instance.ShowMessage(this.updatePanel1, "货位 " + dt.Rows[i]["CellCode"].ToString() + "已经被其它单据锁定，不能移库！");
                return;
            }
            count = bll.GetRowCount("Cmd_Cell", string.Format("CellCode='{0}' and IsLock=1", dt.Rows[i]["NewCellCode"]));
            if (count > 0)
            {
                JScript.Instance.ShowMessage(this.updatePanel1, "货位 " + dt.Rows[i]["NewCellCode"].ToString() + "已经被其它单据锁定，不能移库！");
                return;
            }
        }
        try
        {

            Commands[1] = "WMS.DeleteBillDetail";
            Commands[2] = "WMS.InsertMoveStockDetail";
            bll.ExecTran(Commands, para, "BillID", new DataTable[] { dt });

        }
        catch (Exception ex)
        {
            JScript.Instance.ShowMessage(this.updatePanel1, ex.Message);
            return;
        }

        Response.Redirect(FormID + "View.aspx?SubModuleCode=" + SubModuleCode + "&FormID=" + Server.UrlEncode(FormID) + "&SqlCmd=" + SqlCmd + "&ID=" + Server.UrlEncode(this.txtID.Text));
    }

    #region 子表绑定

    protected void btnFirstSub1_Click(object sender, EventArgs e)
    {
        MovePage("Edit", this.dgViewSub1, 0, btnFirstSub1, btnPreSub1, btnNextSub1, btnLastSub1, btnToPageSub1, lblCurrentPageSub1);
    }

    protected void btnPreSub1_Click(object sender, EventArgs e)
    {
        MovePage("Edit", this.dgViewSub1, this.dgViewSub1.PageIndex - 1, btnFirstSub1, btnPreSub1, btnNextSub1, btnLastSub1, btnToPageSub1, lblCurrentPageSub1);
    }

    protected void btnNextSub1_Click(object sender, EventArgs e)
    {
        MovePage("Edit", this.dgViewSub1, this.dgViewSub1.PageIndex + 1, btnFirstSub1, btnPreSub1, btnNextSub1, btnLastSub1, btnToPageSub1, lblCurrentPageSub1);
    }

    protected void btnLastSub1_Click(object sender, EventArgs e)
    {
        MovePage("Edit", this.dgViewSub1, this.dgViewSub1.PageCount - 1, btnFirstSub1, btnPreSub1, btnNextSub1, btnLastSub1, btnToPageSub1, lblCurrentPageSub1);
    }

    protected void btnToPageSub1_Click(object sender, EventArgs e)
    {
        MovePage("Edit", this.dgViewSub1, int.Parse(this.txtPageNoSub1.Text) - 1, btnFirstSub1, btnPreSub1, btnNextSub1, btnLastSub1, btnToPageSub1, lblCurrentPageSub1);
    }



    #endregion



}
 