using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Util;

public partial class WebUI_CMD_ProductEdit : BasePage
{
    protected string strID;
    BLL.BLLBase bll = new BLL.BLLBase();
    protected void Page_Load(object sender, EventArgs e)
    {
        strID = Request.QueryString["ID"] + "";

        if (!IsPostBack)
        {
            BindDropDownList();
            if (strID != "")
            {
                DataTable dt = bll.FillDataTable("Cmd.SelectProduct", new DataParameter[] { new DataParameter("{0}", string.Format("ProductCode='{0}'", strID)) });
                BindData(dt);

                SetTextReadOnly(this.txtID);
            }
            else
            {
                this.txtID.Text = bll.GetNewID("CMD_Product", "ProductCode", "1=1");
                this.txtCreator.Text = Session["EmployeeCode"].ToString();
                this.txtUpdater.Text = Session["EmployeeCode"].ToString();
                this.txtCreatDate.Text = ToYMD(DateTime.Now);
                this.txtUpdateDate.Text = ToYMD(DateTime.Now);

            }

            writeJsvar(FormID, SqlCmd, strID);
            SetTextReadOnly(this.txtCreator, this.txtCreatDate, this.txtUpdater, this.txtUpdateDate);

        }
    }

    private void BindDropDownList()
    {

        DataTable ProductType = bll.FillDataTable("Cmd.SelectProductCategory");
        this.ddlCategoryCode.DataValueField = "CategoryCode";
        this.ddlCategoryCode.DataTextField = "CategoryName";
        this.ddlCategoryCode.DataSource = ProductType;
        this.ddlCategoryCode.DataBind();

        DataTable dtFactory = bll.FillDataTable("Cmd.SelectFactory", new DataParameter[] { new DataParameter("{0}", "Flag=3") });
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
    protected void btnSave_Click(object sender, EventArgs e)
    {


        if (strID == "") //新增
        {
            int Count = bll.GetRowCount("CMD_Product", string.Format("ProductCode='{0}'", this.txtID.Text));
            if (Count > 0)
            {
                 JScript.Instance.ShowMessage(this.Page, "该产品编号已经存在！");
                return;
            }

            //判断车型，轮对轴号 是否重复

            //Count=bll.GetRowCount("CMD_Product", string.Format("ProductCode='{0}'", this.txtID.Text));
            //if (Count > 0)
            //{
            //    WMS.App_Code.JScript.Instance.ShowMessage(this.Page, "该产品编号已经存在！");
            //    return;
            //}
            //, , , , , , , , , , 
            //      , , , , , , , , , , , 
            //      , , , , Creator, CreateDate, Updater, UpdateDate, , 



            bll.ExecNonQuery("Cmd.InsertProduct", new DataParameter[] { 
                            new DataParameter("@ProductCode", this.txtID.Text.Trim()),
                            new DataParameter("@ProductName", this.txtProductName.Text.Trim()),
                            new DataParameter("@CategoryCode", this.ddlCategoryCode.SelectedValue),
                            new DataParameter("@ProductEName", this.txtProductEName.Text),
                            new DataParameter("@FactoryID", this.ddlFactory.SelectedValue),
                            new DataParameter("@ModelNo", this.txtModelNo.Text.Trim()),
                            new DataParameter("@Spec", this.txtSpec.Text.Trim()),
                            new DataParameter("@Barcode", this.txtBarcode.Text.Trim()),
                            new DataParameter("@Propertity", this.txtPropertity.Text.Trim()),
                            new DataParameter("@Unit", this.txtUnit.Text.Trim()),
                            new DataParameter("@Length", this.txtLength.Text.Trim()),
                            new DataParameter("@Width", this.txtWidth.Text.Trim()),
                            new DataParameter("@Height", this.txtHeight.Text.Trim()),
                            new DataParameter("@Material", this.txtMaterial.Text.Trim()),
                            new DataParameter("@Weight", this.txtWeight.Text.Trim()),
                            new DataParameter("@Color", this.txtColor.Text.Trim()),
                            new DataParameter("@StandardNo",this.txtStandardNo.Text.Trim()),
                            new DataParameter("@PartNo",this.txtPartNo.Text.Trim()),
                            new DataParameter("@ValidPeriod", this.txtValidPeriod.Text.Trim()),
                            new DataParameter("@Description", this.txtDescription.Text.Trim()),
                            new DataParameter("@AreaCode", bll.GetFieldValue("CMD_ProductCategory","AreaCode","CategoryCode='"+this.ddlCategoryCode.SelectedValue+"'")),
                            new DataParameter("@Memo", this.txtMemo.Text.Trim()),
                            new DataParameter("@Creator", Session["EmployeeCode"].ToString()),
                            new DataParameter("@Updater", Session["EmployeeCode"].ToString()),
                          
                });
        }
        else //修改
        {
            //判断车型，轮对轴号 是否重复
            //int Count = bll.GetRowCount("CMD_Product", string.Format("ProductCode='{0}'", this.txtID.Text));
            //if (Count > 0)
            //{
            //    WMS.App_Code.JScript.Instance.ShowMessage(this.Page, "该产品编号已经存在！");
            //    return;
            //}


            bll.ExecNonQuery("Cmd.UpdateProduct", new DataParameter[] {  
                            new DataParameter("@ProductName", this.txtProductName.Text.Trim()),
                            new DataParameter("@CategoryCode", this.ddlCategoryCode.SelectedValue),
                            new DataParameter("@ProductEName", this.txtProductEName.Text),
                            new DataParameter("@FactoryID", this.ddlFactory.SelectedValue),
                            new DataParameter("@ModelNo", this.txtModelNo.Text.Trim()),
                            new DataParameter("@Spec", this.txtSpec.Text.Trim()),
                            new DataParameter("@Barcode", this.txtBarcode.Text.Trim()),
                            new DataParameter("@Propertity", this.txtPropertity.Text.Trim()),
                            new DataParameter("@Unit", this.txtUnit.Text.Trim()),
                            new DataParameter("@Length", this.txtLength.Text.Trim()),
                            new DataParameter("@Width", this.txtWidth.Text.Trim()),
                            new DataParameter("@Height", this.txtHeight.Text.Trim()),
                            new DataParameter("@Material", this.txtMaterial.Text.Trim()),
                            new DataParameter("@Weight", this.txtWeight.Text.Trim()),
                            new DataParameter("@Color", this.txtColor.Text.Trim()),
                            new DataParameter("@StandardNo",this.txtStandardNo.Text.Trim()),
                            new DataParameter("@PartNo",this.txtPartNo.Text.Trim()),
                            new DataParameter("@ValidPeriod", this.txtValidPeriod.Text.Trim()),
                            new DataParameter("@Description", this.txtDescription.Text.Trim()),
                            new DataParameter("@AreaCode", bll.GetFieldValue("CMD_ProductCategory","AreaCode","CategoryCode='"+this.ddlCategoryCode.SelectedValue+"'")),
                            new DataParameter("@Memo", this.txtMemo.Text.Trim()),
                            new DataParameter("@Updater", Session["EmployeeCode"].ToString()),
                            new DataParameter("@ProductCode", this.txtID.Text.Trim())
                                                                               });
        }

        Response.Redirect(FormID + "View.aspx?SubModuleCode=" + SubModuleCode + "&FormID=" + Server.UrlEncode(FormID) + "&SqlCmd=" + SqlCmd + "&ID=" + Server.UrlEncode(this.txtID.Text));
    }
}
