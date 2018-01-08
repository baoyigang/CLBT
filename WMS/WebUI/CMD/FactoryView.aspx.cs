using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Util;

public partial class WebUI_CMD_FactoryView : BasePage   
{
    private string strID;
    private string TableName = "CMD_Factory";
    private string PrimaryKey = "FactoryID";
    BLL.BLLBase bll = new BLL.BLLBase();
    protected void Page_Load(object sender, EventArgs e)
    {
        strID = Request.QueryString["ID"] + "";
        if (!IsPostBack)
        {
            BindDropDownList();
            DataTable dt = bll.FillDataTable("Cmd.SelectFactory", new DataParameter[] { new DataParameter("{0}", string.Format("FactoryID='{0}'", strID)) });
            BindData(dt);
            writeJsvar(FormID, SqlCmd, strID);
        }
    }

    #region 绑定方法
    private void BindDropDownList()
    {

    }

    private void BindData(DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            this.txtID.Text = dt.Rows[0]["FactoryID"].ToString();
            this.txtFactoryName.Text = dt.Rows[0]["FactoryName"].ToString();
            
            this.txtLinkPerson.Text = dt.Rows[0]["LinkPerson"].ToString();
            this.txtLinkPhone.Text = dt.Rows[0]["LinkPhone"].ToString();
            this.txtFax.Text = dt.Rows[0]["Fax"].ToString();
            this.txtAddress.Text = dt.Rows[0]["Address"].ToString();
            this.txtMemo.Text = dt.Rows[0]["Memo"].ToString();
            this.txtCreator.Text = dt.Rows[0]["Creator"].ToString();
            this.txtCreatDate.Text = ToYMD(dt.Rows[0]["CreateDate"]);
            this.txtUpdater.Text = dt.Rows[0]["Updater"].ToString();
            this.txtUpdateDate.Text = ToYMD(dt.Rows[0]["UpdateDate"]);

        }

    }
    private void BindDataNull()
    {
        this.txtID.Text = "";
        this.txtFactoryName.Text = "";
      
        this.txtLinkPerson.Text = "";
        this.txtLinkPhone.Text = "";
        this.txtFax.Text = "";
        this.txtAddress.Text = "";
        this.txtMemo.Text = "";
        this.txtCreator.Text = "";
        this.txtCreatDate.Text = "";
        this.txtUpdater.Text = "";
        this.txtUpdateDate.Text = "";
    }
    #endregion

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string strID = this.txtID.Text;
        int Count = bll.GetRowCount("VUsed_CMD_Factory", string.Format("FactoryID='{0}'", this.txtID.Text.Trim()));
        if (Count > 0)
        {
             JScript.Instance.ShowMessage(this.updatePanel1, "该厂家编码已被其它单据使用，请调整后再删除！");
            return;
        }
        bll.ExecNonQuery("Cmd.DeleteFactory", new DataParameter[] { new DataParameter("{0}", "'" + strID + "'") });
        AddOperateLog("生产厂家", "删除单号：" + strID);

        btnNext_Click(sender, e);
        if (this.txtID.Text == strID)
        {
            btnPre_Click(sender, e);
            if (this.txtID.Text == strID)
            {
                BindDataNull();
            }
        }

    }

    #region 上下笔事件
    protected void btnFirst_Click(object sender, EventArgs e)
    {
        BindData(bll.GetRecord("F", TableName, "1=1", PrimaryKey, this.txtID.Text));
    }
    protected void btnPre_Click(object sender, EventArgs e)
    {
        BindData(bll.GetRecord("P", TableName, "1=1", PrimaryKey, this.txtID.Text));
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        BindData(bll.GetRecord("N", TableName, "1=1", PrimaryKey, this.txtID.Text));
    }
    protected void btnLast_Click(object sender, EventArgs e)
    {
        BindData(bll.GetRecord("L", TableName, "1=1", PrimaryKey, this.txtID.Text));
    }
    #endregion

}