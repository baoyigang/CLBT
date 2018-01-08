using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Util;


public partial class WebUI_CMD_CraneEdit :BasePage
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
                DataTable dt = bll.FillDataTable("Cmd.SelectCrane", new DataParameter[] { new DataParameter("{0}", string.Format("CraneNo='{0}'", strID)) });
                BindData(dt);

                SetTextReadOnly(this.txtID);
            }
            else
            {
                this.txtID.Text = bll.GetNewID("CMD_Crane", "CraneNo", "1=1");


            }

            writeJsvar(FormID, SqlCmd, strID);

        }
    }

    private void BindDropDownList()
    {

    }


    private void BindData(DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            this.txtID.Text = dt.Rows[0]["CraneNo"].ToString();
            this.txtTypeName.Text = dt.Rows[0]["CraneName"].ToString();
            this.ddlActive.SelectedValue = dt.Rows[0]["State"].ToString();
            this.txtMemo.Text = dt.Rows[0]["Memo"].ToString();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {


        if (strID == "") //新增
        {
            int Count = bll.GetRowCount("CMD_Crane", string.Format("CraneNo='{0}'", this.txtID.Text));
            if (Count > 0)
            {
                JScript.Instance.ShowMessage(this.Page, "该堆垛机编码已经存在！");
                return;
            }

            bll.ExecNonQuery("Cmd.InsertCrane", new DataParameter[] { 
                                                                                new DataParameter("@CraneNo", this.txtID.Text.Trim()),
                                                                                new DataParameter("@CraneName", this.txtTypeName.Text.Trim()),
                                                                                new DataParameter("@State", this.ddlActive.SelectedValue),
                                                                                new DataParameter("@Memo", this.txtMemo.Text.Trim()) 
                                                                                 
                                                                              });
        }
        else //修改
        {
            bll.ExecNonQuery("Cmd.UpdateCrane", new DataParameter[] {  new DataParameter("@CraneName", this.txtTypeName.Text.Trim()),
                                                                                 new DataParameter("@Memo", this.txtMemo.Text.Trim()) ,
                                                                                 new DataParameter("@State",this.ddlActive.SelectedValue),
                                                                                 new DataParameter("@CraneNo", this.txtID.Text.Trim())
                                                                               });
        }

        Response.Redirect(FormID + "View.aspx?SubModuleCode=" + SubModuleCode + "&FormID=" + Server.UrlEncode(FormID) + "&SqlCmd=" + SqlCmd + "&ID=" + Server.UrlEncode(this.txtID.Text));
    }
}