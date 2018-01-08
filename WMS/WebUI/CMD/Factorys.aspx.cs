using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Util;
 


public partial class WebUI_CMD_Factorys : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["filter"] = "1=1";
            ViewState["CurrentPage"] = 1;

            try
            {
                SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, GridView1, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
            }
            catch (Exception exp)
            {
                JScript.Instance.ShowMessage(this.UpdatePanel1, exp.Message);
            }

            writeJsvar(FormID, SqlCmd, "");
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.UpdatePanel1.GetType(), "Resize", "resize();", true);


    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {

        try
        {
            ViewState["filter"] = string.Format("{0} like '%{1}%'", this.ddlField.SelectedValue, this.txtSearch.Text.Trim().Replace("'", ""));
            ViewState["CurrentPage"] = 1;
            SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, GridView1, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);

        }
        catch (Exception exp)
        {
             JScript.Instance.ShowMessage(this.UpdatePanel1, exp.Message);
        }
    }

    protected void btnDeletet_Click(object sender, EventArgs e)
    {
        string strColorCode = "'-1',";
        BLL.BLLBase bll = new BLL.BLLBase();
        for (int i = 0; i < this.GridView1.Rows.Count; i++)
        {
            CheckBox cb = (CheckBox)(this.GridView1.Rows[i].FindControl("cbSelect"));
            if (cb != null && cb.Checked)
            {
                HyperLink hk = (HyperLink)(this.GridView1.Rows[i].FindControl("HyperLink1"));
                //判断能否删除
                int Count = bll.GetRowCount("VUsed_CMD_Factory", string.Format("FactoryID='{0}'", hk.Text));
                if (Count > 0)
                {
                     JScript.Instance.ShowMessage(this.UpdatePanel1, GridView1.Rows[i].Cells[3].Text + " 厂家被其它单据使用，请调整后再删除！");
                    return;
                }

                strColorCode += "'" + hk.Text + "',";
            }
        }
        strColorCode += "'-1'";


        bll.ExecNonQuery("Cmd.DeleteFactory", new DataParameter[] { new DataParameter("{0}", strColorCode) });
        AddOperateLog("生产厂家", "删除单号：" + strColorCode.Replace("'-1',", "").Replace(",'-1'", ""));
        SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, GridView1, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);

    }

    protected void btnFirst_Click(object sender, EventArgs e)
    {
        ViewState["CurrentPage"] = 1;
        SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, GridView1, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);

    }

    protected void btnPre_Click(object sender, EventArgs e)
    {
        ViewState["CurrentPage"] = int.Parse(ViewState["CurrentPage"].ToString()) - 1;
        SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, GridView1, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        ViewState["CurrentPage"] = int.Parse(ViewState["CurrentPage"].ToString()) + 1;
        SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, GridView1, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
    }

    protected void btnLast_Click(object sender, EventArgs e)
    {
        ViewState["CurrentPage"] = 0;
        SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, GridView1, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
    }

    protected void btnToPage_Click(object sender, EventArgs e)
    {
        int PageIndex = 0;
        int.TryParse(this.txtPageNo.Text, out PageIndex);
        if (PageIndex == 0)
            PageIndex = 1;

        ViewState["CurrentPage"] = PageIndex;
        SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, GridView1, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
    }

    protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {

    }


}