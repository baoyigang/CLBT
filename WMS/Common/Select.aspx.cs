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
using System.Xml;
using System.Text;
using System.Data.SqlClient;
using Util;
using System.Collections.Generic; 
public partial class Common_Select : System.Web.UI.Page
{
    int pageIndex = 1;
    int pageSize = 10;
    int totalCount = 0;
    int pageCount = 0;
    string filter = "1=1";
    string TableName = "";
    string TableView = "";


    Hashtable htFields;
    string queryFields;
    string PrimaryKey = "";
    string orderBy = "";

    bool blnMultiSelect;
    Dictionary<int, string> d;
    BLL.BLLBase bll = new BLL.BLLBase();
    protected void Page_PreInit(object sender, EventArgs e)
    {

        if (Session["G_user"] == null)
        {
            Response.Write("<script language='javascript'>alert('对不起,操作时限已过,请重新登入！'); var parentwindow=window.dialogArguments;parentwindow.top.location = '../Login.aspx';window.close();</script>");
            return;
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            blnMultiSelect = Request.QueryString["Option"].ToString() == "1" ? true : false;
            TableName = Request.QueryString["TableName"];

            if (!IsPostBack)
            {


                if (Request.QueryString["Where"] != "" && Request.QueryString["Where"] != null)
                {
                    filter = Microsoft.JScript.GlobalObject.unescape(Request.QueryString["Where"]);
                    //filter = Request.QueryString["Where"].Replace('"', "'"[0]);
                }

                htFields = new Hashtable();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + string.Format("WebUI\\TableXML\\{0}.xml", TableName));
                StringBuilder fieldList = new StringBuilder();
                XmlNode nodeTable = xmlDoc.SelectSingleNode("TABLE");
                PrimaryKey = nodeTable.Attributes["PrimaryKey"].InnerText;
                orderBy = nodeTable.Attributes["OrderBy"].InnerText;
                if (nodeTable.Attributes["ViewName"] != null && nodeTable.Attributes["ViewName"].InnerText != "")
                {
                    ViewState["tableView"] = nodeTable.Attributes["ViewName"].InnerText;
                    TableView = nodeTable.Attributes["ViewName"].InnerText;
                }
                else
                {
                    ViewState["tableView"] = TableName;
                    TableView = TableName;
                }

                DataTable dtFieldQuery = new DataTable();
                DataColumn dc1 = new DataColumn("FieldDisplay", typeof(string));
                DataColumn dc2 = new DataColumn("FieldValue", typeof(string));
                DataColumn dc3 = new DataColumn("FieldIndex", typeof(int));
                dtFieldQuery.Columns.Add(dc1);
                dtFieldQuery.Columns.Add(dc2);
                dtFieldQuery.Columns.Add(dc3);

                foreach (XmlNode node in nodeTable.ChildNodes)
                {
                    fieldList.Append(node.ChildNodes[0].InnerText + ",");
                    htFields.Add(node.ChildNodes[0].InnerText, node.ChildNodes[1].InnerText);
                    if (node.Attributes["ShowInFieldQuery"].Value == "1")
                    {
                        DataRow dr = dtFieldQuery.NewRow();
                        dr.BeginEdit();
                        dr["FieldDisplay"] = node.ChildNodes[1].InnerText;
                        dr["FieldValue"] = node.ChildNodes[0].InnerText;
                        dr["FieldIndex"] = int.Parse(node.Attributes["ShowQueryIndex"].Value);
                        dr.EndEdit();
                        dtFieldQuery.Rows.Add(dr);
                    }
                }
                DataRow[] drs = dtFieldQuery.Select("", "FieldIndex");
                for (int i = 0; i < drs.Length; i++)
                {
                    this.ddlFieldName.Items.Add(new ListItem(drs[i]["FieldDisplay"].ToString(), drs[i]["FieldValue"].ToString()));
                }
                fieldList.Remove(fieldList.Length - 1, 1);
                queryFields = fieldList.ToString();

                BindGrid(1);   //pageIndex, pageSize, filter, orderBy, PrimaryKey, TableView, queryFields
                ViewState["htFields"] = htFields;
                ViewState["PageSize"] = pageSize;
                ViewState["filter"] = filter;
                ViewState["OrderBy"] = orderBy;
                ViewState["PrimaryKey"] = PrimaryKey;
                ViewState["queryFields"] = queryFields;
                ViewState["pageIndex"] = pageIndex;
            }
            else
            {
                htFields = (Hashtable)(ViewState["htFields"]);
                pageSize = Convert.ToInt32(ViewState["PageSize"]);
                filter = ViewState["filter"].ToString();
                orderBy = ViewState["OrderBy"].ToString();
                PrimaryKey = ViewState["PrimaryKey"].ToString();
                queryFields = ViewState["queryFields"].ToString();
                pageIndex = int.Parse(ViewState["pageIndex"].ToString());
                TableView = ViewState["tableView"].ToString();
            }

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.UpdatePanel1.GetType(), "Resize", "resize();", true);

        }
        catch (Exception exp)
        {
            JScript.Instance.ShowMessage(this, exp.Message);
        }
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {


        string strReturn = "";
        GridView1.HeaderStyle.Wrap = false;
        GridView1.RowStyle.Wrap = false;


        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (blnMultiSelect)//多選
                e.Row.Cells[1].Visible = false;

            else
                e.Row.Cells[0].Visible = false;


            d = new Dictionary<int, string>();
            for (int i = 2; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].Wrap = false;
                string fieldname = e.Row.Cells[i].Text.Trim();
                e.Row.Cells[i].Text = htFields[fieldname].ToString();
                if (!d.ContainsKey(i))
                    d.Add(i, fieldname);

            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //当鼠标停留时更改背景色

            e.Row.Attributes.Add("onmouseover", "HandleMouseEvent('over')");
            //当鼠标移开时还原背景色
            e.Row.Attributes.Add("onmouseout", "HandleMouseEvent('out')");
            if (blnMultiSelect)//多選
            {
                if ((HtmlInputCheckBox)e.Row.Cells[0].FindControl("chkSelect") != null)
                {
                    HtmlInputCheckBox oChk = (HtmlInputCheckBox)e.Row.Cells[2].FindControl("chkSelect");
                    string strChkName = oChk.ClientID;
                    strReturn = "";
                    for (int i = 2; i < e.Row.Cells.Count; i++)
                    {
                        string FiledName = "";
                        d.TryGetValue(i, out FiledName);
                        if (i == e.Row.Cells.Count - 1)
                            strReturn += "\"" + FiledName + "\": \"" + Microsoft.JScript.GlobalObject.escape(((DataRowView)e.Row.DataItem).Row.ItemArray[i - 2].ToString()) + "\"";
                        else
                            strReturn += "\"" + FiledName + "\": \"" + Microsoft.JScript.GlobalObject.escape(((DataRowView)e.Row.DataItem).Row.ItemArray[i - 2].ToString()) + "\",";
                    }
                    strReturn = "{" + strReturn + "}";

                    oChk.Attributes.Add("onclick", "AddValues('" + strChkName + "','" + strReturn + "');");
                    e.Row.Attributes.Add("ondblclick", "document.getElementById('" + strChkName + "').click();");
                    if (HdnSelectedValues.Value.IndexOf(strReturn) >= 0)
                        oChk.Checked = true;

                }
                e.Row.Cells[1].Visible = false;

                this.btnSelect.Enabled = true;

            }
            else//單選
            {
                if ((Button)e.Row.Cells[1].FindControl("btnSingle") != null)
                {
                    Button btn = (Button)e.Row.Cells[2].FindControl("btnSingle");

                    strReturn = "";

                    for (int i = 2; i < e.Row.Cells.Count; i++)
                    {
                        string FiledName = "";
                        d.TryGetValue(i, out FiledName);
                        if (i == e.Row.Cells.Count - 1)
                            strReturn += "\"" + d[i] + "\": \"" + Microsoft.JScript.GlobalObject.escape(((DataRowView)e.Row.DataItem).Row.ItemArray[i - 2].ToString()) + "\"";
                        else
                            strReturn += "\"" + d[i] + "\": \"" + Microsoft.JScript.GlobalObject.escape(((DataRowView)e.Row.DataItem).Row.ItemArray[i - 2].ToString()) + "\",";
                    }
                    strReturn = "{" + strReturn + "}";
                    btn.Attributes.Add("onclick", "SelectPage.HdnSelectedValues.value ='[" + strReturn + "]';window.parent.returnValue = document.getElementById('HdnSelectedValues').value;window.close();");
                    e.Row.Attributes.Add("ondblclick", "SelectPage.HdnSelectedValues.value ='[" + strReturn + "]';window.parent.returnValue = document.getElementById('HdnSelectedValues').value;window.close();");
                }

                e.Row.Cells[0].Visible = false;

                this.btnSelect.Enabled = false;

            }



        }
    }
    #region 翻页處理 +查询按钮+笔数切换
    protected void btnFirst_Click(object sender, System.EventArgs e)
    {
        SetBtnEnabled("F");
    }

    protected void btnLast_Click(object sender, System.EventArgs e)
    {
        SetBtnEnabled("L");
    }

    protected void btnPre_Click(object sender, System.EventArgs e)
    {
        SetBtnEnabled("P");
    }

    protected void btnNext_Click(object sender, System.EventArgs e)
    {
        SetBtnEnabled("N");
    }
    protected void btnToPage_Click(object sender, System.EventArgs e)
    {
        int PageIndex = 0;
        int.TryParse(this.txtPageNo.Text, out PageIndex);
        if (PageIndex == 0)
            PageIndex = 1;

        ViewState["pageIndex"] = PageIndex;
        SetBtnEnabled("");
    }

    private void SetBtnEnabled(string movePage)
    {
        switch (movePage)
        {
            case "F":
                ViewState["pageIndex"] = 1;
                break;
            case "P":
                ViewState["pageIndex"] = int.Parse(ViewState["pageIndex"].ToString()) - 1;
                break;
            case "N":
                ViewState["pageIndex"] = int.Parse(ViewState["pageIndex"].ToString()) + 1;
                break;
            case "L":
                ViewState["pageIndex"] = 0;
                break;
            default:
                if (ViewState["pageIndex"] == null)
                    ViewState["pageIndex"] = 1;
                break;
        }

        BindGrid(int.Parse(ViewState["pageIndex"].ToString()));

    }

    protected void btnSearch_Click(object sender, System.EventArgs e)
    {

        try
        {
            pageIndex = 1;
            //TableView = ViewState["tableView"].ToString();
            filter = string.Format(" {0} like '%{1}%' ", this.ddlFieldName.SelectedValue, this.txtSearch.Text.Trim());
            if (Request.QueryString["Where"] != "" && Request.QueryString["Where"] != null)
            {
                filter += " AND " + Microsoft.JScript.GlobalObject.unescape(Request.QueryString["Where"]);
            }

            ViewState["filter"] = filter;

            BindGrid(pageIndex);

        }
        catch (Exception exp)
        {
            JScript.Instance.ShowMessage(this.UpdatePanel1, exp.Message);
        }
    }
    //protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    ViewState["PageSize"] = this.ddlPageSize.SelectedValue;
    //    SetBtnEnabled("");
    //}
    /// <summary>
    /// 浏览界面每页笔数
    /// </summary>
    //private void BindPageSize()
    //{
    //    this.ddlPageSize.Items.Add(new ListItem("10", "10"));
    //    this.ddlPageSize.Items.Add(new ListItem("20", "20"));
    //    this.ddlPageSize.Items.Add(new ListItem("40", "40"));
    //    this.ddlPageSize.Items.Add(new ListItem("50", "50"));
    //}

    /// <summary>
    /// 綁定GirdView
    /// </summary>
    /// <param name="pageIndex"></param>
    private void BindGrid(int pageIndex)
    {
        totalCount = bll.GetRowCount(TableView, filter);
        pageCount = GetPageCount(totalCount, pageSize);
        if (pageIndex == 0 || pageIndex > pageCount)
            pageIndex = pageCount;

        DataTable dt = bll.GetDataPage(pageIndex, pageSize, filter, orderBy, PrimaryKey, TableView, queryFields);


        if (dt.Rows.Count == 0)
        {
            dt.Rows.Add(dt.NewRow());
            GridView1.DataSource = dt;
            GridView1.DataBind();
            int columnCount = GridView1.Rows[0].Cells.Count;
            GridView1.Rows[0].Cells.Clear();
            GridView1.Rows[0].Cells.Add(new TableCell());
            GridView1.Rows[0].Cells[0].ColumnSpan = columnCount;
            GridView1.Rows[0].Cells[0].Text = "没有符合以上条件的数据,请重新查询 ";
            GridView1.Rows[0].Visible = true;

            this.btnFirst.Enabled = false;
            this.btnPre.Enabled = false;
            this.btnNext.Enabled = false;
            this.btnLast.Enabled = false;
            this.btnToPage.Enabled = false;
            this.lblPage.Visible = false;

        }
        else
        {
            this.GridView1.DataSource = dt;
            this.GridView1.DataBind();

            this.btnLast.Enabled = true;
            this.btnFirst.Enabled = true;
            this.btnToPage.Enabled = true;

            if (pageIndex > 1)
                this.btnPre.Enabled = true;
            else
                this.btnPre.Enabled = false;

            if (pageIndex < pageCount)
                this.btnNext.Enabled = true;
            else
                this.btnNext.Enabled = false;

            lblPage.Visible = true;
            lblPage.Text = "共 [" + totalCount.ToString() + "] 笔记录  第 [" + pageIndex.ToString() + "] 页  共 [" + pageCount.ToString() + "] 页";

        }
    }

    #endregion

    protected int GetPageCount(int TotalCount, int PageSize)
    {
        int pageCount = 1;

        pageCount = TotalCount / PageSize;
        int p = TotalCount % PageSize;
        if (p > 0)
            pageCount += 1;

        return pageCount;
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        try
        {
            pageIndex = 1;
            //TableView = ViewState["tableView"].ToString();

            if (Request.QueryString["Where"] != "" && Request.QueryString["Where"] != null)
            {
                filter = Microsoft.JScript.GlobalObject.unescape(Request.QueryString["Where"]);
            }

            ViewState["filter"] = filter;

            BindGrid(pageIndex);

        }
        catch (Exception exp)
        {
            JScript.Instance.ShowMessage(this.UpdatePanel1, exp.Message);
        }
    }
}