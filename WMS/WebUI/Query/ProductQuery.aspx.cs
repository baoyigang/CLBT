 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FastReport;
using FastReport.Data;
using FastReport.Utils;
using System.Data;
using System.IO;
using Util;


public partial class WebUI_Query_ProductQuery : BasePage
{
    private string strWhere;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            rptview.Visible = false;
            BindOther();
            writeJsvar("", "", "");

        }
        else
        {
            string hdnwh = HdnWH.Value;
            int W = int.Parse(hdnwh.Split('#')[0]);
            int H = int.Parse(hdnwh.Split('#')[1]);
            WebReport1.Width = W - 60;
            WebReport1.Height = H - 55;
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "", "BindEvent();", true);
        }
    }
    private void BindOther()
    {
       

    }
    protected void WebReport1_StartReport(object sender, EventArgs e)
    {
        if (!rptview.Visible) return;
        LoadRpt();
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        rptview.Visible = true;
        WebReport1.Refresh();
    }
    private void GetStrWhere()
    {
        strWhere = "1=1";
        
            if (this.txtSpec.Text.Trim().Length > 0)
                strWhere += string.Format(" and Spec like '%{0}%'", this.txtSpec.Text);

            if (this.txtBarCode.Text.Trim().Length > 0)
                strWhere += string.Format(" and Product.BarCode like '%{0}%'", this.txtBarCode.Text);
            if (this.txtAreaCode.Text.Trim().Length > 0)
            {
                strWhere += string.Format(" and AreaName like '%{0}%'", this.txtAreaCode.Text);
            }

    }
    private bool LoadRpt()
    {
        try
        {
            GetStrWhere();
            string frx = "ProductDetailQuery.frx";
            string Comds = "WMS.SelectProductDetailQuery";

            if (rpt2.Checked)
            {
                frx = "ProductTotalQuery.frx";
                Comds = "WMS.SelectProductTotalQuery";

            }
            WebReport1.Report = new Report();
            WebReport1.Report.Load(System.AppDomain.CurrentDomain.BaseDirectory + @"RptFiles\" + frx);

            BLL.BLLBase bll = new BLL.BLLBase();

            DataTable dt = bll.FillDataTable(Comds, new DataParameter[] { new DataParameter("{0}", strWhere) });



            if (dt.Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('您所选择的条件没有资料!');", true);
            }

            WebReport1.Report.RegisterData(dt, "ProductQuery");
        }
        catch (Exception ex)
        {
        }
        return true;
    }

}
 