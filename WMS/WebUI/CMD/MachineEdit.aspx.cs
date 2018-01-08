using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Util;

public partial class WebUI_CMD_MachineEdit :BasePage
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

        DataTable ProductType = bll.FillDataTable("Cmd.SelectProductType", new DataParameter[] { new DataParameter("{0}", "cmd.AreaCode not in ('001') and ProductTypeCode<>'0001'") });
        this.ddlProductTypeCode.DataValueField = "ProductTypeCode";
        this.ddlProductTypeCode.DataTextField = "ProductTypeName";
        this.ddlProductTypeCode.DataSource = ProductType;
        this.ddlProductTypeCode.DataBind();

        //ddlTrainTypeCode
        //DataTable TrainType = bll.FillDataTable("Cmd.SelectTrainType");
        //this.ddlTrainTypeCode.DataValueField = "TypeCode";
        //this.ddlTrainTypeCode.DataTextField = "TypeName";
        //this.ddlTrainTypeCode.DataSource = TrainType;
        //this.ddlTrainTypeCode.DataBind();

        //ddlCCLX_Factory
        //DataTable CCLX_Factory = bll.FillDataTable("Cmd.SelectFactory", new DataParameter[] { new DataParameter("{0}", "Flag=1") });
        //this.ddlCCLX_Factory.DataValueField = "FactoryID";
        //this.ddlCCLX_Factory.DataTextField = "FactoryName";
        //this.ddlCCLX_Factory.DataSource = CCLX_Factory;
        //this.ddlCCLX_Factory.DataBind();

        ////ddlFCCLX_Factory

        //DataTable FCCLX_Factory = bll.FillDataTable("Cmd.SelectFactory", new DataParameter[] { new DataParameter("{0}", "Flag=2") });
        //this.ddlFCCLX_Factory.DataValueField = "FactoryID";
        //this.ddlFCCLX_Factory.DataTextField = "FactoryName";
        //this.ddlFCCLX_Factory.DataSource = FCCLX_Factory;
        //this.ddlFCCLX_Factory.DataBind();

        ///ddlCX_Factory
        DataTable CX_Factory = bll.FillDataTable("Cmd.SelectFactory", new DataParameter[] { new DataParameter("{0}", "Flag=4") });
        this.ddlCX_Factory.DataValueField = "FactoryID";
        this.ddlCX_Factory.DataTextField = "FactoryName";
        this.ddlCX_Factory.DataSource = CX_Factory;
        this.ddlCX_Factory.DataBind();

    }


    private void BindData(DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            this.txtID.Text = dt.Rows[0]["ProductCode"].ToString();
            this.txtProductName.Text = dt.Rows[0]["ProductName"].ToString();
            this.ddlProductTypeCode.SelectedValue = dt.Rows[0]["ProductTypeCode"].ToString();
            //this.ddlTrainTypeCode.SelectedValue = dt.Rows[0]["TrainTypeCode"].ToString();
            this.txtAxieNo.Text = dt.Rows[0]["AxieNo"].ToString();
            this.txtWheelDiameter.Text = dt.Rows[0]["WheelDiameter"].ToString();

            this.txtCCZ_Diameter.Text = dt.Rows[0]["CCZ_Diameter"].ToString();
            this.txtFCCZ_Diameter.Text = dt.Rows[0]["FCCZ_Diameter"].ToString();
            this.txtCCD_Diameter.Text = dt.Rows[0]["CCD_Diameter"].ToString();
            this.txtFCCD_Diameter.Text = dt.Rows[0]["FCCD_Diameter"].ToString();
            this.txtCCXPBZW_Size.Text = dt.Rows[0]["CCXPBZW_Size"].ToString();
            this.txtFCCXPBZW_Size.Text = dt.Rows[0]["FCCXPBZW_Size"].ToString();
            this.txtGearNo.Text = dt.Rows[0]["GearNo"].ToString();
            this.txtCCLX_Flag.Text = dt.Rows[0]["CCLX_Flag"].ToString();
            this.txtFCCLX_Flag.Text = dt.Rows[0]["FCCLX_Flag"].ToString();
            this.txtCCLX_Year.Text = dt.Rows[0]["CCLX_Year"].ToString();
            this.txtFCCLX_Year.Text = dt.Rows[0]["FCCLX_Year"].ToString();
            //this.ddlCCLX_Factory.SelectedValue = dt.Rows[0]["CCLX_FactoryID"].ToString();
            //this.ddlFCCLX_Factory.SelectedValue = dt.Rows[0]["FCCLX_FactoryID"].ToString();
            this.ddlCX_Factory.SelectedValue = dt.Rows[0]["CX_FactoryID"].ToString();
            this.txtCCLG_Flag.Text = dt.Rows[0]["CCLG_Flag"].ToString();
            //this.txtFCCLG_Flag.Text = dt.Rows[0]["FCCLG_Flag"].ToString();
            this.chkTemp.Checked = dt.Rows[0]["IsTemp"].ToString() == "1" ? true : false;

            this.txtLDXC.Text = dt.Rows[0]["LDXC"].ToString();
            this.txtCX_DateTime.DateValue = dt.Rows[0]["CX_DateTime"];
            this.txtInstockDate.Text = ToYMD(dt.Rows[0]["InstockDate"]);

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
                            new DataParameter("@ProductTypeCode", this.ddlProductTypeCode.SelectedValue),
                            new DataParameter("@TrainTypeCode",""),
                            new DataParameter("@AxieNo", this.txtAxieNo.Text.Trim()),
                            new DataParameter("@WheelDiameter", this.txtWheelDiameter.Text.Trim()),
                            new DataParameter("@CCZ_Diameter", this.txtCCZ_Diameter.Text.Trim()),
                            new DataParameter("@FCCZ_Diameter", this.txtFCCZ_Diameter.Text.Trim()),
                            new DataParameter("@CCD_Diameter", this.txtCCD_Diameter.Text.Trim()),
                            new DataParameter("@FCCD_Diameter", this.txtFCCD_Diameter.Text.Trim()),
                            new DataParameter("@CCXPBZW_Size", this.txtCCXPBZW_Size.Text.Trim()),
                            new DataParameter("@FCCXPBZW_Size", this.txtFCCXPBZW_Size.Text.Trim()),
                            new DataParameter("@GearNo", this.txtGearNo.Text.Trim()),
                            new DataParameter("@CCLX_Flag", this.txtCCLX_Flag.Text.Trim()),
                            new DataParameter("@FCCLX_Flag", this.txtFCCLX_Flag.Text.Trim()),
                            new DataParameter("@CCLX_Year", this.txtCCLX_Year.Text.Trim()),
                            new DataParameter("@FCCLX_Year", this.txtFCCLX_Year.Text.Trim()),
                            new DataParameter("@CCLX_Factory", ""),
                            new DataParameter("@FCCLX_Factory", ""),
                            new DataParameter("@CX_Factory", this.ddlCX_Factory.SelectedValue),
                            new DataParameter("@CCLG_Flag", this.txtCCLG_Flag.Text.Trim()),
                            new DataParameter("@FCCLG_Flag", ""),
                            new DataParameter("@WarehouseCode", ""),
                            new DataParameter("@AreaCode", bll.GetFieldValue("CMD_ProductType","AreaCode","ProductTypeCode='"+this.ddlProductTypeCode.SelectedValue+"'")),
                            new DataParameter("@LDXC", this.txtLDXC.Text.Trim()),
                            new DataParameter("@CX_DateTime", this.txtCX_DateTime.DateValue),
                            new DataParameter("@Memo", this.txtMemo.Text.Trim()),
                            new DataParameter("@Creator", Session["EmployeeCode"].ToString()),
                            new DataParameter("@Updater", Session["EmployeeCode"].ToString()),
                            new DataParameter("@IsTemp",chkTemp.Checked ? "1" :"0")
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
                            new DataParameter("@ProductTypeCode", this.ddlProductTypeCode.SelectedValue),
                            new DataParameter("@TrainTypeCode", ""),
                            new DataParameter("@AxieNo", this.txtAxieNo.Text.Trim()),
                            new DataParameter("@WheelDiameter", this.txtWheelDiameter.Text.Trim()),
                            new DataParameter("@CCZ_Diameter", this.txtCCZ_Diameter.Text.Trim()),
                            new DataParameter("@FCCZ_Diameter", this.txtFCCZ_Diameter.Text.Trim()),
                            new DataParameter("@CCD_Diameter", this.txtCCD_Diameter.Text.Trim()),
                            new DataParameter("@FCCD_Diameter", this.txtFCCD_Diameter.Text.Trim()),
                            new DataParameter("@CCXPBZW_Size", this.txtCCXPBZW_Size.Text.Trim()),
                            new DataParameter("@FCCXPBZW_Size", this.txtFCCXPBZW_Size.Text.Trim()),
                            new DataParameter("@GearNo", this.txtGearNo.Text.Trim()),
                            new DataParameter("@CCLX_Flag", this.txtCCLX_Flag.Text.Trim()),
                            new DataParameter("@FCCLX_Flag", this.txtFCCLX_Flag.Text.Trim()),
                            new DataParameter("@CCLX_Year", this.txtCCLX_Year.Text.Trim()),
                            new DataParameter("@FCCLX_Year", this.txtFCCLX_Year.Text.Trim()),
                            new DataParameter("@CCLX_Factory", ""),
                            new DataParameter("@FCCLX_Factory", ""),
                            new DataParameter("@CX_Factory", this.ddlCX_Factory.SelectedValue),
                            new DataParameter("@CCLG_Flag", this.txtCCLG_Flag.Text.Trim()),
                            new DataParameter("@FCCLG_Flag", ""),
                            new DataParameter("@WarehouseCode", ""),
                             new DataParameter("@AreaCode", bll.GetFieldValue("CMD_ProductType","AreaCode","ProductTypeCode='"+this.ddlProductTypeCode.SelectedValue+"'")),
                            new DataParameter("@IsTemp",chkTemp.Checked ? "1" :"0"),
                            new DataParameter("@LDXC", this.txtLDXC.Text.Trim()),
                            new DataParameter("@CX_DateTime", this.txtCX_DateTime.DateValue),
                            new DataParameter("@Memo", this.txtMemo.Text.Trim()),
                            new DataParameter("@Creator", Session["EmployeeCode"].ToString()),
                            new DataParameter("@Updater", Session["EmployeeCode"].ToString()),
                            new DataParameter("@ProductCode", this.txtID.Text.Trim())
                                                                               });
        }

        Response.Redirect(FormID + "View.aspx?SubModuleCode=" + SubModuleCode + "&FormID=" + Server.UrlEncode(FormID) + "&SqlCmd=" + SqlCmd + "&ID=" + Server.UrlEncode(this.txtID.Text));
    }
}
