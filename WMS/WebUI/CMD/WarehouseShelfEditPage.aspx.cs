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

public partial class WebUI_CMD_WarehouseShelfEditPage : BasePage
{
    DataTable dtShelf;
    BLL.BLLBase bll = new BLL.BLLBase();
    private string TableName;
    private string ColumnName;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["CMD_WH_SHELF_ID"] != null)
            {
                dtShelf = bll.FillDataTable("Cmd.SelectCellShelf", new DataParameter[] { new DataParameter("{0}", " ShelfCode='" + Request.QueryString["CMD_WH_SHELF_ID"] + "'") });

                this.txtShelfID.Text = dtShelf.Rows[0]["ShelfCode"].ToString();

                this.txtWHID.Text = dtShelf.Rows[0]["WarehouseCode"].ToString();
                this.txtWhName.Text = bll.GetFieldValue("CMD_Warehouse", "WarehouseName", "WarehouseCode='" + this.txtWHID.Text + "'");
                this.txtAreaID.Text = dtShelf.Rows[0]["AreaCode"].ToString();

                this.txtShelfCode.Text = dtShelf.Rows[0]["ShelfCode"].ToString();
                this.txtShelfName.Text = dtShelf.Rows[0]["ShelfName"].ToString();
                this.txtCellRows.Text = dtShelf.Rows[0]["Rows"].ToString();
                this.txtCellCols.Text = dtShelf.Rows[0]["Columns"].ToString();

                this.txtMemo.Text = dtShelf.Rows[0]["MEMO"].ToString();
                this.ddlActive.SelectedValue = dtShelf.Rows[0]["IsActive"].ToString();


            }
            else if (Request.QueryString["AREACODE"] != null)
            {
                this.txtAreaID.Text = Request.QueryString["AREACODE"];
                this.txtWHID.Text = bll.GetFieldValue("CMD_Area", "WarehouseCode", "AreaCode='" + this.txtAreaID.Text + "'");
                this.txtWhName.Text = bll.GetFieldValue("CMD_Warehouse", "WarehouseName", "WarehouseCode='" + this.txtWHID.Text + "'");

                this.txtShelfID.Text = "";

                int RowCount = bll.GetRowCount("CMD_WH_SHELF", string.Format("AREA_CODE='{0}'", Request.QueryString["AREACODE"])) + 1;
                this.txtShelfCode.Text = Request.QueryString["AREACODE"] + RowCount.ToString().PadLeft(3, '0');


            }
            SetTextReadOnly(txtShelfCode, this.txtWhName, this.txtCellCols, this.txtCellRows);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (this.txtShelfID.Text.Trim().Length == 0)
            {
                int count = bll.GetRowCount("CMD_WH_SHELF", string.Format("ShelfCode='{0}'", this.txtShelfCode.Text));
                if (count > 0)
                {
                    JScript.Instance.ShowMessage(this, "此货架编码已存在，不能新增！");
                    return;
                }

                bll.ExecNonQuery("Cmd.InsertShelf", new DataParameter[] { 
                                                                                new DataParameter("@WarehouseCode", this.txtWHID.Text),
                                                                                new DataParameter("@AreaCode", this.txtAreaID.Text.Trim()),
                                                                                new DataParameter("@ShelfCode",  this.txtShelfCode.Text),
                                                                                new DataParameter("@ShelfName", this.txtShelfName.Text.Trim()),
                                                                                new DataParameter("@Rows",  this.txtCellRows.Text),
                                                                                new DataParameter("@Columns", this.txtCellCols.Text.Trim()),
                                                                                new DataParameter("@IsActive", this.ddlActive.SelectedValue),
                                                                                new DataParameter("@Memo", this.txtMemo.Text.Trim())
                                                                                });
                //this.btnContinue.Enabled = true;
                //this.btnSave.Enabled = false;
                JScript.Instance.RegisterScript(this, "ReloadParent();");
                AddOperateLog("货贺管理", "添加货贺信息");
            }
            else
            {
                bll.ExecNonQuery("Cmd.UpdateShelf", new DataParameter[] { new DataParameter("@WarehouseCode", this.txtWHID.Text),
                                                                                new DataParameter("@AreaCode", this.txtAreaID.Text.Trim()),
                                                                                new DataParameter("@ShelfName", this.txtShelfName.Text.Trim()),
                                                                                new DataParameter("@Rows",  this.txtCellRows.Text),
                                                                                new DataParameter("@Columns", this.txtCellCols.Text.Trim()),
                                                                                new DataParameter("@IsActive", this.ddlActive.SelectedValue),
                                                                                new DataParameter("@Memo", this.txtMemo.Text.Trim()),
                                                                              new DataParameter("{0}",  this.txtShelfID.Text)});


                JScript.Instance.RegisterScript(this, "UpdateParent();");
                AddOperateLog("货贺管理", "修改货贺信息");
            }
        }
        catch (Exception exp)
        {
            JScript.Instance.ShowMessage(this, exp.Message);
        }
        //JScript.Instance.RegisterScript(this, "window.close();");
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        JScript.Instance.RegisterScript(this, "window.close();");
    }

    protected void btnContinue_Click(object sender, EventArgs e)
    {
        this.txtShelfID.Text = "";

        this.txtShelfName.Text = "";
        this.txtMemo.Text = "";
        this.btnSave.Enabled = true;
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string whcode = this.txtWHID.Text;
        string areacode = this.txtAreaID.Text;
        string shelfCode = this.txtShelfCode.Text;
        string shelfid = this.txtShelfID.Text;


        DataTable dtTemp = bll.FillDataTable("Cmd.SelectCell", new DataParameter[] { new DataParameter("{0}", "SHELF_CODE='" + shelfCode + "'") });
        int count = dtTemp.Rows.Count;
        if (count > 0)
        {
            JScript.Instance.ShowMessage(this, shelfCode + "货架还有下属货位，不能删除！");
            return;
        }
        else
        {
            bll.ExecNonQuery("Cmd.DeleteShelf", new DataParameter[] { new DataParameter("{0}", shelfid) });
            this.txtShelfID.Text = "";
            this.txtShelfCode.Text = "";
            this.txtShelfName.Text = "";
            this.txtMemo.Text = "";
            this.btnSave.Enabled = true;

            string path = whcode + "/" + areacode + "/" + shelfCode;
            JScript.Instance.RegisterScript(this, "RefreshParent('" + path + "');");
        }
        AddOperateLog("货贺管理", "删除货贺信息");
    }
}