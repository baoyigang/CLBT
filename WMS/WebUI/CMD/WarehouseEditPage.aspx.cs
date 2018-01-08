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

public partial class WebUI_CMD_WarehouseEditPage : BasePage
{
    BLL.BLLBase bll = new BLL.BLLBase();
    DataTable dtHouse;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["WAREHOUSE_CODE"] != null)
            {

                dtHouse = bll.FillDataTable("Cmd.SelectWarehouse", new DataParameter[] { new DataParameter("{0}", "WarehouseCode='" + Request.QueryString["WAREHOUSE_CODE"] + "'") });

                this.txtWHID.Text = dtHouse.Rows[0]["WarehouseCode"].ToString();
                this.txtWhCode.Text = dtHouse.Rows[0]["WarehouseCode"].ToString();
                this.txtWhName.Text = dtHouse.Rows[0]["WarehouseName"].ToString();
                this.txtMemo.Text = dtHouse.Rows[0]["MEMO"].ToString();

                SetTextReadOnly(this.txtWhCode);

            }
            else
            {
                this.txtWHID.Text = "";
                this.txtWhCode.Text = bll.GetNewID("CMD_WAREHOUSE", "WarehouseCode", "1=1");
            }
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (this.txtWHID.Text.Trim().Length == 0)//新增
        {
            int count = bll.GetRowCount("CMD_WAREHOUSE", string.Format("WarehouseCode='{0}'", this.txtWhCode.Text));
            if (count > 0)
            {
                JScript.Instance.ShowMessage(this, "此仓库编码已存在，不能新增！");
                return;
            }
            //新增
            bll.ExecNonQuery("Cmd.InsertWarehouse", new DataParameter[] { new DataParameter("@WarehouseCode", this.txtWhCode.Text), new DataParameter("@WarehouseName", this.txtWhName.Text.Trim().Replace("\'", "\''")), new DataParameter("@MEMO", this.txtMemo.Text.Trim()) });
            JScript.Instance.RegisterScript(this, "ReloadParent();");
            AddOperateLog("仓库管理", "添加仓库信息");
        }
        else
        {
            //更新
            bll.ExecNonQuery("Cmd.UpdateWarehouse", new DataParameter[] { new DataParameter("@WarehouseCode", this.txtWhCode.Text), new DataParameter("@WarehouseName", this.txtWhName.Text.Trim().Replace("\'", "\''")), 
                                        new DataParameter("@MEMO", this.txtMemo.Text.Trim()),new DataParameter("{0}",this.txtWhCode.Text) });

            JScript.Instance.RegisterScript(this, "UpdateParent();");
            AddOperateLog("仓库管理", "修改仓库信息");
        }

        JScript.Instance.RegisterScript(this, "window.close();");
    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        JScript.Instance.RegisterScript(this, "window.close();");
    }


    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string whcode = this.txtWhCode.Text;
        DataTable dtTemp = bll.FillDataTable("Cmd.SelectArea", new DataParameter[] { new DataParameter("{0}", "WarehouseCode='" + whcode + "'") });
        int count = dtTemp.Rows.Count;


        if (count > 0)
        {
            JScript.Instance.ShowMessage(this, whcode + "还有下属库区，不能删除！");
            return;
        }
        else
        {
            bll.ExecNonQuery("Cmd.DeleteWarehouse", new DataParameter[] { new DataParameter("{0}", whcode) });

            this.txtWHID.Text = "";
            this.txtWhName.Text = "";
            this.txtMemo.Text = "";
            this.txtWhCode.ReadOnly = false;

            string path = whcode;
            JScript.Instance.RegisterScript(this, "RefreshParent('" + path + "');");
        }
        AddOperateLog("仓库管理", "删除仓库信息");
    }
}