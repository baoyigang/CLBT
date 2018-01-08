using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Util;


public partial class WebUI_CMD_CraneView : BasePage
{
    private string strID;
    private string TableName = "CMD_Crane";
    private string PrimaryKey = "CraneNo";
    BLL.BLLBase bll = new BLL.BLLBase();
    protected void Page_Load(object sender, EventArgs e)
    {
        strID = Request.QueryString["ID"] + "";
        if (!IsPostBack)
        {
            BindDropDownList();
            DataTable dt = bll.FillDataTable("Cmd.SelectCrane", new DataParameter[] { new DataParameter("{0}", string.Format("CraneNo='{0}'", strID)) });
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
            this.txtID.Text = dt.Rows[0]["CraneNo"].ToString();
            this.txtTypeName.Text = dt.Rows[0]["CraneName"].ToString();
            this.ddlActive.SelectedValue = dt.Rows[0]["State"].ToString();
            this.txtMemo.Text = dt.Rows[0]["Memo"].ToString();


        }

    }
    private void BindDataNull()
    {
        this.txtID.Text = "";
        this.txtTypeName.Text = "";
        this.txtMemo.Text = "";

    }
    #endregion

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string strID = this.txtID.Text;
        int Count = bll.GetRowCount("VUsed_CMD_TrainType", string.Format("TypeCode='{0}'", this.txtID.Text.Trim()));
        if (Count > 0)
        {
           JScript.Instance.ShowMessage(this.updatePanel, "该车型编码已被其它单据使用，请调整后再删除！");
            return;
        }
        bll.ExecNonQuery("Cmd.DeleteTrainType", new DataParameter[] { new DataParameter("{0}", "'" + strID + "'") });
        AddOperateLog("车型资料", "删除单号：" + strID);

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