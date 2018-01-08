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
 

public partial class WebUI_CMD_WarehouseAreaEditPage :BasePage
{

    DataTable dtArea;

    private string TableName = "CMD_WH_AREA";

    BLL.BLLBase bll = new BLL.BLLBase();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["CMD_WH_AREA_ID"] != null)
            {
                dtArea = bll.FillDataTable("Cmd.SelectArea", new DataParameter[] { new DataParameter("{0}", "AreaCode='" + Request.QueryString["CMD_WH_AREA_ID"] + "'") });
                if (this.dtArea.Rows.Count > 0)
                {
                    this.txtWHID.Text = dtArea.Rows[0]["WarehouseCode"].ToString();
                    this.txtAreaID.Text = dtArea.Rows[0]["AreaCode"].ToString();
                    this.txtWhName.Text = bll.GetFieldValue("CMD_Warehouse", "WarehouseName", "WarehouseCode='" + this.txtWHID.Text + "'");
                    this.txtAreaCode.Text = dtArea.Rows[0]["AreaCode"].ToString();
                    this.txtAreaName.Text = dtArea.Rows[0]["AreaName"].ToString();
                    this.txtMemo.Text = dtArea.Rows[0]["MEMO"].ToString();

                }
            }
            else if (Request.QueryString["WHCODE"] != null)
            {
                this.txtWHID.Text = Request.QueryString["WHCODE"];

                this.txtWhName.Text = bll.GetFieldValue("CMD_Warehouse", "WarehouseName", "WarehouseCode='" + this.txtWHID.Text + "'");
                this.txtAreaID.Text = "";
                this.txtAreaCode.Text = bll.GetNewID("CMD_Area", "AreaCode", "1=1");

            }
            SetTextReadOnly(this.txtAreaCode, this.txtWhName, this.txtWHID);
        }
        else
        {

        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (this.txtAreaID.Text.Trim().Length == 0)//新增
        {
            int count = bll.GetRowCount(TableName, string.Format("AreaCode='{0}'", this.txtAreaCode.Text));
            if (count > 0)
            {
                JScript.Instance.ShowMessage(this, "此库区编码已存在，不能新增！");
                return;
            }

            bll.ExecNonQuery("Cmd.InsertArea", new DataParameter[] {
                                                                        new DataParameter("@AreaCode", this.txtAreaCode.Text),
                                                                        new DataParameter("@AreaName", this.txtAreaName.Text.Trim().Replace("\'", "\''")),
                                                                        new DataParameter("@WarehouseCode",  this.txtWHID.Text),
                                                                        new DataParameter("@Memo", this.txtMemo.Text.Trim())
                                                                         });

            JScript.Instance.RegisterScript(this, "ReloadParent();");
            AddOperateLog("库区管理", "添加库区信息");
        }
        else
        {
            bll.ExecNonQuery("Cmd.UpdateArea", new DataParameter[] {
                                                                        new DataParameter("@AreaName", this.txtAreaName.Text.Trim().Replace("\'", "\''")),
                                                                        new DataParameter("@MEMO", this.txtMemo.Text.Trim()),
                                                                        new DataParameter("{0}",this.txtAreaID.Text) });


           JScript.Instance.RegisterScript(this, "UpdateParent();");
            AddOperateLog("库区管理", "修改库区信息");
        }

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        JScript.Instance.RegisterScript(this, "window.close();");
    }


    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string whcode = this.txtWHID.Text;
        string areaCode = this.txtAreaCode.Text;
        string areaid = this.txtAreaID.Text;
        DataTable dtTemp = bll.FillDataTable("Cmd.SelectShelf", new DataParameter[] { new DataParameter("{0}", "AreaCode='" + areaid + "'") });
        int count = dtTemp.Rows.Count;
        if (count > 0)
        {
            JScript.Instance.ShowMessage(this, areaCode + "库区还有下属货架，不能删除！");
            return;
        }
        else
        {
            bll.ExecNonQuery("Cmd.DeleteArea", new DataParameter[] { new DataParameter("{0}", areaid) });
            this.txtAreaID.Text = "";
            this.txtAreaCode.Text = "";
            this.txtAreaName.Text = "";
            this.txtMemo.Text = "";
            this.txtAreaCode.ReadOnly = false;

            string path = whcode + "/" + areaCode;
            JScript.Instance.RegisterScript(this, "RefreshParent('" + path + "');");
            AddOperateLog("库区管理", "删除库区信息");
        }
    }
}