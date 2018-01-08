 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Util;



public partial class WebUI_CMD_ProductView : BasePage
{
    private string strID;
    private string TableName = "VCMD_Product";
    private string PrimaryKey = "ProductCode";
    BLL.BLLBase bll = new BLL.BLLBase();
    private string filter = "IsFixed='0'";
    protected void Page_Load(object sender, EventArgs e)
    {
        strID = Request.QueryString["ID"] + "";
        if (!IsPostBack)
        {
            BindDropDownList();
            DataTable dt = bll.FillDataTable("Cmd.SelectProduct", new DataParameter[] { new DataParameter("{0}", string.Format("ProductCode='{0}'", strID)) });
            BindData(dt);
            writeJsvar(FormID, SqlCmd, strID);
        }
    }

    #region 绑定方法
    private void BindDropDownList()
    {



        DataTable ProductType = bll.FillDataTable("Cmd.SelectProductCategory");
        this.ddlCategoryCode.DataValueField = "CategoryCode";
        this.ddlCategoryCode.DataTextField = "CategoryName";
        this.ddlCategoryCode.DataSource = ProductType;
        this.ddlCategoryCode.DataBind();

        //ddlTrainTypeCode
        //DataTable TrainType = bll.FillDataTable("Cmd.SelectTrainType");
        //this.ddlTrainTypeCode.DataValueField = "TypeCode";
        //this.ddlTrainTypeCode.DataTextField = "TypeName";
        //this.ddlTrainTypeCode.DataSource = TrainType;
        //this.ddlTrainTypeCode.DataBind();

       

        ///ddlCX_Factory
        DataTable dtFactory = bll.FillDataTable("Cmd.SelectFactory");
        this.ddlFactory.DataValueField = "FactoryID";
        this.ddlFactory.DataTextField = "FactoryName";
        this.ddlFactory.DataSource = dtFactory;
        this.ddlFactory.DataBind();

    }

    private void BindData(DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {

            this.txtID.Text = dt.Rows[0]["ProductCode"].ToString();
            this.txtProductName.Text = dt.Rows[0]["ProductName"].ToString();
            this.ddlCategoryCode.SelectedValue = dt.Rows[0]["CategoryCode"].ToString();
            //this.ddlTrainTypeCode.SelectedValue = dt.Rows[0]["TrainTypeCode"].ToString();
            this.txtProductEName.Text = dt.Rows[0]["ProductEName"].ToString();
            this.txtModelNo.Text = dt.Rows[0]["ModelNo"].ToString();

            this.txtUnit.Text = dt.Rows[0]["Unit"].ToString();
            this.txtLength.Text = dt.Rows[0]["Length"].ToString();
            this.txtWidth.Text = dt.Rows[0]["Width"].ToString();
            this.txtHeight.Text = dt.Rows[0]["Height"].ToString();
            this.txtWeight.Text = dt.Rows[0]["Weight"].ToString();
            this.txtMaterial.Text = dt.Rows[0]["Material"].ToString();
            this.txtColor.Text = dt.Rows[0]["Color"].ToString();
            this.txtValidPeriod.Text = dt.Rows[0]["ValidPeriod"].ToString();

            this.txtPartNo.Text = dt.Rows[0]["PartNo"].ToString();
            this.txtStandardNo.Text = dt.Rows[0]["StandardNo"].ToString();
             
            this.ddlFactory.SelectedValue = dt.Rows[0]["FactoryID"].ToString();


            this.txtBarcode.Text = dt.Rows[0]["Barcode"].ToString();
            this.txtPropertity.Text = dt.Rows[0]["Propertity"].ToString();
            this.txtDescription.Text = dt.Rows[0]["Description"].ToString();

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
        this.txtProductName.Text = "";

        //this.ddlTrainTypeCode.SelectedValue = dt.Rows[0]["TrainTypeCode"].ToString();
        this.txtProductEName.Text = "";
        this.txtModelNo.Text = "";

        this.txtUnit.Text = "";
        this.txtLength.Text = "";
        this.txtWidth.Text = "";
        this.txtHeight.Text = "";
        this.txtWeight.Text = "";
        this.txtMaterial.Text = "";
        this.txtColor.Text = "";
        this.txtValidPeriod.Text = "";
        this.txtStandardNo.Text = "";
        this.txtPartNo.Text = "";

        this.ddlFactory.SelectedValue = "";


        this.txtBarcode.Text = "";
        this.txtPropertity.Text = "";
        this.txtDescription.Text = "";

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
        int Count = bll.GetRowCount("VUsed_CMD_Product", string.Format("ProductCode='{0}'", this.txtID.Text.Trim()));
        if (Count > 0)
        {
            JScript.Instance.ShowMessage(this.updatePanel, "该产品编号已被其它单据使用，请调整后再删除！");
            return;
        }
        bll.ExecNonQuery("Cmd.DeleteProduct", new DataParameter[] { new DataParameter("{0}", "'" + strID + "'") });

        AddOperateLog("产品信息", "删除单号：" + strID);
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
        BindData(bll.GetRecord("F", TableName, filter, PrimaryKey, this.txtID.Text));
    }
    protected void btnPre_Click(object sender, EventArgs e)
    {
        BindData(bll.GetRecord("P", TableName, filter, PrimaryKey, this.txtID.Text));
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        BindData(bll.GetRecord("N", TableName, filter, PrimaryKey, this.txtID.Text));
    }
    protected void btnLast_Click(object sender, EventArgs e)
    {
        BindData(bll.GetRecord("L", TableName, filter, PrimaryKey, this.txtID.Text));
    }
    #endregion

}
