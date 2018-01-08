using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using Util;
using System.Configuration;

public partial class Index_Main : System.Web.UI.Page
{
    #region 变量

    DataTable dtDestopItem;

    Table tb;
    Panel pl;
    Hashtable GlobalMenuTitle;//菜单标题
    Hashtable GlobalMenuLink;//菜单链接地址
    Hashtable GlobalMenuParent;//菜单父标题

    Hashtable GlobalModuleID;//主键

    int iTableCount;
    int iCount = 0;
    int iCountColor = 0;
    #endregion
    #region 页面加载事件
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["IsFirstLogin"] != null)
        //{
        //    if (Session["IsFirstLogin"].ToString() == "1")
        //    {
        //    }
        //    else
        //    {
        if (!IsPostBack)
        {
            GC.Collect();
            Response.ExpiresAbsolute = DateTime.Now;
            try
            {
                //string str = "<script> " +
                //             "try " +
                //             "{ " +
                //             " var nav=window.parent.frames.Navigation.document.getElementById('labNavigation');" +
                //             " nav.innerText='快速通道';" +
                //             "} " +
                //             "catch(e) " +
                //             "{ " +
                //             "} " +
                //             "</script>";
                //this.ClientScript.RegisterClientScriptBlock(this.GetType(), DateTime.Now.ToLongTimeString(), str);

                if (Session["UserID"] != null)
                {
                    GetDestopItemByUserID(Session["UserID"].ToString());
                }
                else
                {
                    Response.Write("<script language=javascript>parent.parent.parent.location.href='../../WebUI/Start/SessionTimeOut.aspx';</script>");
                    Response.End();
                    return;

                    //Response.Redirect("../../WebUI/Start/SessionTimeOut.aspx");
                }
                CreatePage();


                #region 提醒

                #endregion
            }
            catch (Exception ex)
            {
                //Session["ModuleName"] = "MainPage.aspx";
                //Session["FunctionName"] = "Page_Load事件";
                //Session["ExceptionalType"] = ex.GetType().FullName;
                //Session["ExceptionalDescription"] = ex.Message;
                //Response.Redirect("Common/MistakesPage.aspx");
            }

        }
    }
    #endregion
    #region 创建页面
    private void CreatePage()
    {
        ////////
        GlobalMenuTitle = new Hashtable();
        GlobalMenuLink = new Hashtable();
        GlobalMenuParent = new Hashtable();
        GlobalModuleID = new Hashtable();
        tb = new Table();
        //Session["IsFirstLogin"] = "2";
        tb = CreateTable(iTableCount);
        tb.Attributes.Add("Width", "100%");
        tb.Attributes.Add("border", "0");
        tb.Attributes.Add("bordercolor", "#ffffff");
        tb.Attributes.Add("frame", "void");
        tb.Attributes.Add("cellpadding", "0");
        tb.Attributes.Add("cellspacing", "0");
        tb.Attributes.Add("align", "center");
        // tb.Attributes.Add("style", "margin-right:10px;");
        pl = new Panel();
        pl.Attributes.Add("Width", "100%");

        int i = 0, j = 1;
        if (dtDestopItem.Rows.Count > 0)
        {
            foreach (DataRow dr in dtDestopItem.Rows)
            {
                ImageButton im = new ImageButton();
                im.ID = "im" + i.ToString() + i.ToString() + j.ToString();
                im.ImageUrl = "../images/" + dr["DestopImage"].ToString();
                //im.Click += new ImageClickEventHandler(im_Click);
                GlobalMenuTitle.Add(im.ID, dr["MenuTitle"].ToString());
                GlobalMenuLink.Add(im.ID, dr["MenuUrl"].ToString() + "?FormID=" + dr["FormID"].ToString() + "&SubModuleCode=" + dr["MenuCode"].ToString() + "&SqlCmd=" + dr["SqlCmdFlag"].ToString());
                GlobalMenuParent.Add(im.ID, dr["MenuParent"].ToString());
                GlobalModuleID.Add(im.ID, dr["ModuleID"].ToString());
                tb.Rows[i].Cells[j].Attributes.Add("onClick", "window.parent.addTab(\"" + GlobalMenuLink["im" + i.ToString() + i.ToString() + j.ToString()].ToString() + "\",\"" + GlobalMenuLink["im" + i.ToString() + i.ToString() + j.ToString()].ToString() + "\",\"" + GlobalMenuTitle["im" + i.ToString() + i.ToString() + j.ToString()].ToString() + "\");return false;");
                tb.Rows[i].Cells[j].Controls.Add(im);
                tb.Rows[i].Cells[j].Controls.Add(new LiteralControl("<br/><font style=\"font-size: 13px;font-family:微软雅黑;\">" + dr["MenuTitle"].ToString() + "</font>"));
                j = j + 2;
                if (j > 11)
                {
                    j = 1; i++;
                }
            }
        }
        pl.Controls.Add(tb);
        form1.Controls.Add(pl);
    }
    #endregion
    #region 链接事件
    protected void im_Click(object sender, ImageClickEventArgs e)
    {
        //id, link, name
        ImageButton ImageButtonID = (ImageButton)sender;
        string strScript = "<script> ";
        //strScript += "var nav=window.parent.document.getElementById(\'labNavigation\');";
        //strScript += " nav.innerText=\'" + GlobalMenuParent[ImageButtonID.ID].ToString() + ">>" + GlobalMenuTitle[ImageButtonID.ID].ToString() + "\';";
        strScript += string.Format("window.parent.addTab(\"{0}\",\"{1}\",\"{2}\");", GlobalModuleID[ImageButtonID.ID].ToString(), GlobalMenuLink[ImageButtonID.ID].ToString(), GlobalMenuTitle[ImageButtonID.ID].ToString());
        //strScript += "window.open(\'" + GlobalMenuLink[ImageButtonID.ID].ToString() + "\',\'_self\')";
        strScript += "</script>";
        this.ClientScript.RegisterClientScriptBlock(this.GetType(), DateTime.Now.ToLongTimeString(), strScript);
    }
    private void GetDestopItemByUserID(string UserID)
    {
        BLL.BLLBase bll = new BLL.BLLBase();
        dtDestopItem = bll.FillDataTable("Security.SelectUserQuickDesktop", new DataParameter[] { new DataParameter("@UserID", UserID) });
        iTableCount = dtDestopItem.Rows.Count;
    }
    #endregion
    #region 创建表格
    /// <summary>
    /// 创建表格
    /// </summary>
    /// <param name="iRowCount"></param>
    /// <returns></returns>
    private Table CreateTable(int iTableCount)
    {
        for (int i = 0; i < iTableCount / 6 + 1; i++)
        {
            TableRow row = CreateTableRow();
            row.Attributes.Add("align", "center");
            for (int j = 0; j < 6; j++)
            {

                row.Cells.Add(new TableCell());
                row.Cells.Add(CreateTableCell(i, j));
                if (j == 5)
                {
                    row.Cells.Add(new TableCell());
                }
            }
            tb.Rows.Add(row);
        }
        return tb;
    }
    #endregion 创建表格
    #region 创建Table的Row
    /// <summary>
    /// 创建Table的Row
    /// </summary>
    /// <returns></returns>
    private TableRow CreateTableRow()
    {
        TableRow CreateTableRow = new TableRow();
        //CreateTableRow.Attributes.Add("style","height:200px;width:200px");
        return CreateTableRow;
    }
    #endregion 创建Table的Row
    #region 创建Table的cell
    /// <summary>
    /// 创建Table的Cell
    /// </summary>
    /// <returns></returns>
    private TableCell CreateTableCell(int i, int j)
    {
        TableCell CreateTableCell = new TableCell();
        CreateTableCell.Attributes.Add("style", "height:100px;width:100px;border:1px solid #EAF2F4");
        CreateTableCell.ID = i.ToString() + j.ToString();
        int iID = Convert.ToInt32(CreateTableCell.ID);

        if (iCount < iTableCount)
        {
            CreateTableCell.Attributes.Add("onMouseOver", "SetNewColor(this);");
            CreateTableCell.Attributes.Add("onMouseOut", "SetOldColor(this);");
            iCountColor++;
        }
        iCount++;
        return CreateTableCell;
    }
    #endregion
}