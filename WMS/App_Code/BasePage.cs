using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using Util;
/// <summary>
///BasePage 的摘要说明
/// </summary>
public class BasePage : System.Web.UI.Page
{
    protected string SubModuleCode;
    protected string SubModuleTitle;
    protected string FormID;
    protected string SqlCmd;
    protected int pageSize = 20;
    protected int pageSubSize = 10;
    protected void Page_PreInit(object sender, EventArgs e)
    {
        try
        {
            if (Session["G_user"] == null)
            {

                string strPath = "../../";
                if (Page.Request.Url.AbsolutePath.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries).Length == 3)
                    strPath = "../";
                else if (Page.Request.Url.AbsolutePath.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries).Length == 5)
                    strPath = "../../../";

                Response.Redirect(ResolveUrl("~/SessionTimeOut.aspx"));
                return;
                // Response.Redirect(strPath + "SessionTimeOut.aspx");

                //Response.Write("<script language='javascript'>alert('对不起,操作时限已过,请重新登入！');window.top.location.href = " + strPath + "/Login.aspx';</script>");
                //Response.End();


                Response.Write("<script language='javascript'>alert(parent.location.href);parent.location.href=" + strPath + "SessionTimeOut.aspx';</script>");
                Response.End();
                return;

            }

            FormID = Request.QueryString["FormID"];
            SqlCmd = Request.QueryString["SqlCmd"];
            SubModuleCode = Request.QueryString["SubModuleCode"];

            //if (!IsPostBack)
            //    InitLoading();
            this.Error += new System.EventHandler(BasePage_Error);

            Session["IsUseGlobalParameter"] = "1";
            if (Request.QueryString["SubModuleCode"] != null)
            {
                Session["SubModuleCode"] = Request.QueryString["SubModuleCode"];

            }
            //if (Session["G_user"] != null)
            //{
            //    //入库单分配权限控制【一次只允许一个用户进行分配】

            //    if (Session["SubModuleCode"] != null && Session["SubModuleCode"].ToString() != "MNU_M00B_00D" && Application["MNU_M00B_00D"] != null && Application["MNU_M00B_00D"].ToString() == Session["G_user"].ToString())
            //    {
            //        Application["MNU_M00B_00D"] = null;
            //    }
            //    //出库单分配权限控制【一次只允许一个用户进行分配】

            //    if (Session["SubModuleCode"] != null && Session["SubModuleCode"].ToString() != "MNU_M00E_00D" && Application["MNU_M00E_00D"] != null && Application["MNU_M00E_00D"].ToString() == Session["G_user"].ToString())
            //    {
            //        Application["MNU_M00E_00D"] = null;
            //    }
            //    //移位单生成权限控制【一次只允许一个用户进行生成移位单】

            //    if (Session["SubModuleCode"] != null && Session["SubModuleCode"].ToString() != "MNU_M00D_00G" && Application["MNU_M00D_00G"] != null && Application["MNU_M00D_00G"].ToString() == Session["G_user"].ToString())
            //    {
            //        Application["MNU_M00D_00G"] = null;
            //    }

            //}
            //else
            //{

            //    //Response.Redirect("~/SessionTimeOut.aspx", false);
            //    Response.Redirect("../../WebUI/Start/SessionTimeOut.aspx", false);
            //}
        }
        catch (Exception ex)
        {
        }
    }

    //错误处理
    protected void BasePage_Error(object sender, System.EventArgs e)
    {
        string errMsg;
        Exception currentError = Server.GetLastError();

        errMsg = "<link rel=\"stylesheet\" href=\"/TheMovie/Styles/TheMovie.CSS\">";
        errMsg += "<h1>Page Error</h1><hr/>An unexpected error has occurred on this page. The system " +
            "administrators have been notified. Please feel free to contact us with the information " +
            "surrounding this error.<br/>" +
            "The error occurred in: " + Request.Url.ToString() + "<br/>" +
            "Error Message: <font class=\"ErrorMessage\">" + currentError.Message.ToString() + "</font><hr/>" +
            "<b>Stack Trace:</b><br/>" +
            currentError.ToString();

        Response.Write(errMsg);
        //Response.Redirect("../../Common/MistakesPage.aspx");
        Server.ClearError();
    }
    protected void Page_PreLoad(object sender, EventArgs e)
    {

        #region 权限控制
        try
        {
            if (Session["SubModuleCode"] != null)
            {
                string[] path = this.Page.Request.Path.Split('/');
                if (path.Length > 0)
                {
                    if (path[path.Length - 1].IndexOf(FormID + "s", 0) >= 0) //s
                    {
                        if ((Button)Page.FindControl("btnAdd") != null)
                            ((Button)Page.FindControl("btnAdd")).Enabled = false;

                        if ((Button)Page.FindControl("btnDelete") != null)
                            ((Button)Page.FindControl("btnDelete")).Enabled = false;

                        if ((Button)Page.FindControl("btnPrint") != null)
                            ((Button)Page.FindControl("btnPrint")).Enabled = false;

                    }
                    if (path[path.Length - 1].IndexOf(FormID + "View", 0) >= 0)
                    {
                        if ((Button)Page.FindControl("btnAdd") != null)
                            ((Button)Page.FindControl("btnAdd")).Enabled = false;

                        if ((Button)Page.FindControl("btnDelete") != null)
                            ((Button)Page.FindControl("btnDelete")).Enabled = false;
                        if ((Button)Page.FindControl("btnEdit") != null)
                            ((Button)Page.FindControl("btnEdit")).Enabled = false;

                        if ((Button)Page.FindControl("btnPrint") != null)
                            ((Button)Page.FindControl("btnPrint")).Enabled = false;


                    }
                }

                if (Session["DT_UserOperation"] == null)
                {
                    BLL.BLLBase bll = new BLL.BLLBase();
                    int iGroupID = int.Parse(Session["GroupID"].ToString());
                    DataTable dt = bll.FillDataTable("Security.SelectGroupRole", new DataParameter[] { new DataParameter("@GroupID", iGroupID), new DataParameter("@SystemName", "WMS") });
                    Session["DT_UserOperation"] = dt;
                }

                DataTable dtOP = (DataTable)(Session["DT_UserOperation"]);
                DataRow[] drs = dtOP.Select(string.Format("SubModuleCode='{0}'", Session["SubModuleCode"].ToString()));


                foreach (DataRow dr in drs)
                {
                    Session["SubModuleTitle"] = drs[0]["MenuTitle"].ToString();
                    SubModuleTitle = Session["SubModuleTitle"].ToString();
                    int op = int.Parse(dr["OperatorCode"].ToString());
                    switch (op)
                    {
                        case 0:
                            if ((Button)Page.FindControl("btnAdd") != null)
                            {
                                ((Button)Page.FindControl("btnAdd")).Enabled = true;
                            }
                            break;
                        case 1:
                            if ((Button)Page.FindControl("btnDelete") != null)
                            {
                                ((Button)Page.FindControl("btnDelete")).Enabled = true;
                            }
                            break;
                        case 2: //修改
                            if ((HiddenField)Page.FindControl("hdnXGQX") != null)
                            {
                                ((HiddenField)Page.FindControl("hdnXGQX")).Value = "1";
                            }
                            if ((Button)Page.FindControl("btnEdit") != null)
                            {
                                ((Button)Page.FindControl("btnEdit")).Enabled = true;
                            }
                            break;


                        case 3: //打印
                            if ((Button)Page.FindControl("btnPrint") != null)
                            {
                                ((Button)Page.FindControl("btnPrint")).Enabled = true;
                            } break;
                        case 4: //抛出
                            if ((Button)Page.FindControl("btnExport") != null)
                            {
                                ((Button)Page.FindControl("btnExport")).Enabled = true;
                            } break;
                        case 5: //审核
                            if ((Button)Page.FindControl("btnCheck") != null)
                            {
                                ((Button)Page.FindControl("btnCheck")).Enabled = true;
                            }
                            break;
                        case 6: //二审
                            if ((Button)Page.FindControl("btnReCheck") != null)
                            {
                                ((Button)Page.FindControl("btnReCheck")).Enabled = true;
                            } break;
                        case 7: //关闭
                            if ((Button)Page.FindControl("btnClose") != null)
                            {
                                ((Button)Page.FindControl("btnClose")).Enabled = true;
                            } break;
                        case 10:
                            if ((Button)Page.FindControl("btnUpdateClearing") != null)
                            {
                                ((Button)Page.FindControl("btnUpdateClearing")).Enabled = true;
                            } break;
                        default: break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            JScript.Instance.ShowMessage(Page, ex.Message);
        }

        #endregion
    }

    /// <summary>
    /// 添加操作日志
    /// </summary>
    /// <param name="moduleName">操作模块</param>
    /// <param name="executeOperation">操作内容</param>
    protected void AddOperateLog(string moduleName, string executeOperation)
    {

        BLL.BLLBase bll = new BLL.BLLBase();
        bll.ExecNonQuery("Security.InsertOperatorLog", new DataParameter[]{new DataParameter("@LoginUser",Session["G_user"].ToString()),new DataParameter("@LoginTime",DateTime.Now),
                                                         new DataParameter("@LoginModule",moduleName),new DataParameter("@ExecuteOperator",executeOperation)});
    }

    /// <summary>
    /// 设置cookie
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    protected void SaveCookie30Day(string key, string value)
    {
        HttpCookie cookie = new HttpCookie(key, value);    //实例化HttpCookie类并添加值
        cookie.Expires = DateTime.Now.AddHours(12);                             //设置保存时间
        Response.Cookies.Add(cookie);
    }

    /// <summary>
    /// 明细无资料显示列名
    /// </summary>
    /// <param name="moduleName">操作模块</param>
    /// <param name="executeOperation">操作内容</param>
    protected void BindDataSubNotDetail(GridView dgv, DataTable dt)
    {
        DataTable dtNew = dt.Clone();
        dtNew.Rows.Add(dtNew.NewRow());
        dgv.DataSource = dtNew;
        dgv.DataBind();
        int columnCount = dgv.Rows[0].Cells.Count;
        dgv.Rows[0].Cells.Clear();
        dgv.Rows[0].Cells.Add(new TableCell());
        dgv.Rows[0].Cells[0].ColumnSpan = columnCount;
        dgv.Rows[0].Cells[0].Text = "没有明细资料";
        dgv.Rows[0].Visible = true;
    }
    /// <summary>
    /// 添加错误日志
    /// </summary>
    /// <param name="moduleName">操作模块</param>
    /// <param name="executeOperation">操作内容</param>
    protected void AddExceptionLog(string errorMessage)
    {
        //ExceptionLog bll = new ExceptionLog();
        //bll.Insert();
    }
    public void initJavascript()
    {
        //ScriptManager.RegisterStartupScript(
        HttpContext.Current.Response.Write(" <script language=JavaScript type=text/javascript>");
        HttpContext.Current.Response.Write("var t_id = setInterval(animate,20);");
        HttpContext.Current.Response.Write("var pos=0;var dir=2;var len=0;");
        HttpContext.Current.Response.Write("function animate(){");
        HttpContext.Current.Response.Write("var elem = document.getElementById('progress');");
        HttpContext.Current.Response.Write("if(elem != null) {");
        HttpContext.Current.Response.Write("if (pos==0) len += dir;");
        HttpContext.Current.Response.Write("if (len>32 || pos>79) pos += dir;");
        HttpContext.Current.Response.Write("if (pos>79) len -= dir;");
        HttpContext.Current.Response.Write(" if (pos>79 && len==0) pos=0;");
        HttpContext.Current.Response.Write("elem.style.left = pos;");
        HttpContext.Current.Response.Write("elem.style.width = len;");
        HttpContext.Current.Response.Write("}}");
        HttpContext.Current.Response.Write("function remove_loading() {");
        HttpContext.Current.Response.Write(" this.clearInterval(t_id);");
        HttpContext.Current.Response.Write("var targelem = document.getElementById('loader_container');");
        HttpContext.Current.Response.Write("targelem.style.display='none';");
        HttpContext.Current.Response.Write("targelem.style.visibility='hidden';");
        HttpContext.Current.Response.Write("}");
        HttpContext.Current.Response.Write("</script>");
        HttpContext.Current.Response.Write("<style>");
        HttpContext.Current.Response.Write("#loader_container {text-align:center; position:absolute; top:40%; width:100%; left: 0;}");
        HttpContext.Current.Response.Write("#loader {font-family:Tahoma, Helvetica, sans; font-size:11.5px; color:#000000; background-color:#FFFFFF; padding:10px 0 16px 0; margin:0 auto; display:block; width:130px; border:1px solid #5a667b; text-align:left; z-index:2;}");
        HttpContext.Current.Response.Write("#progress {height:5px; font-size:1px; width:1px; position:relative; top:1px; left:0px; background-color:#8894a8;}");
        HttpContext.Current.Response.Write("#loader_bg {background-color:#e4e7eb; position:relative; top:8px; left:8px; height:7px; width:113px; font-size:1px;}");
        HttpContext.Current.Response.Write("</style>");
        HttpContext.Current.Response.Write("<div id=loader_container>");
        HttpContext.Current.Response.Write("<div id=loader>");
        HttpContext.Current.Response.Write("<div align=center>页面正在加载中 ...</div>");
        HttpContext.Current.Response.Write("<div id=loader_bg><div id=progress> </div></div>");
        HttpContext.Current.Response.Write("</div></div>");
        HttpContext.Current.Response.Flush();
    }
    public void Hindinit()
    {
        HttpContext.Current.Response.Write(" <script language=JavaScript type=text/javascript>");
        HttpContext.Current.Response.Write(" if( document.getElementById('loader_container')) document.getElementById('loader_container').style.display='none';");
        HttpContext.Current.Response.Write("</script>");
        HttpContext.Current.Response.Flush();
    }
    public void initAjaxJavascript(UpdatePanel up)
    {

        StringBuilder sb = new StringBuilder();
        sb.Append("<script language=JavaScript type=text/javascript>");
        sb.Append("var t_id = setInterval(animate,20);");
        sb.Append("var pos=0;var dir=2;var len=0;");
        sb.Append("function animate(){");
        sb.Append("var elem = document.getElementById('progress');");
        sb.Append("if(elem != null) {");
        sb.Append("if (pos==0) len += dir;");
        sb.Append("if (len>32 || pos>79) pos += dir;");
        sb.Append("if (pos>79) len -= dir;");
        sb.Append(" if (pos>79 && len==0) pos=0;");
        sb.Append("elem.style.left = pos;");
        sb.Append("elem.style.width = len;");
        sb.Append("}}");
        sb.Append("function remove_loading() {");
        sb.Append(" this.clearInterval(t_id);");
        sb.Append("var targelem = document.getElementById('loader_container');");
        sb.Append("targelem.style.display='none';");
        sb.Append("targelem.style.visibility='hidden';");
        sb.Append("}");
        sb.Append("</script>");
        sb.Append("<style>");
        sb.Append("#loader_container {text-align:center; position:absolute; top:40%; width:100%; left: 0;}");
        sb.Append("#loader {font-family:Tahoma, Helvetica, sans; font-size:11.5px; color:#000000; background-color:#FFFFFF; padding:10px 0 16px 0; margin:0 auto; display:block; width:130px; border:1px solid #5a667b; text-align:left; z-index:2;}");
        sb.Append("#progress {height:5px; font-size:1px; width:1px; position:relative; top:1px; left:0px; background-color:#8894a8;}");
        sb.Append("#loader_bg {background-color:#e4e7eb; position:relative; top:8px; left:8px; height:7px; width:113px; font-size:1px;}");
        sb.Append("</style>");
        sb.Append("<div id=loader_container>");
        sb.Append("<div id=loader>");
        sb.Append("<div align=center>页面正在加载中 ...</div>");
        sb.Append("<div id=loader_bg><div id=progress> </div></div>");
        sb.Append("</div></div>");
        ScriptManager.RegisterClientScriptBlock(up, typeof(UpdatePanel), "Progress", sb.ToString(), false);

        //HttpContext.Current.Response.Flush();
    }
    public void HideAjaxinit(UpdatePanel up)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(" <script language=JavaScript type=text/javascript>");
        sb.Append(" if( document.getElementById('loader_container')) document.getElementById('loader_container').style.display='none';");
        sb.Append("</script>");
        ScriptManager.RegisterClientScriptBlock(up, typeof(UpdatePanel), "Progress", sb.ToString(), false);
        //HttpContext.Current.Response.Flush();
    }
    public void Loading()
    {
        HttpContext hc = HttpContext.Current;
        //创建一个页面居中的div
        hc.Response.Write("<div id='loading'style='position: absolute; height: 100px; text-align: center;z-index: 9999; left: 50%; top: 50%; margin-top: -50px; margin-left: -175px;'> ");
        hc.Response.Write("<br />页面正在加载中，请稍候......<br /><br /> ");
        hc.Response.Write("<table border='0' cellpadding='0' cellspacing='0' style='background-image: url(images/Progress/plan-bg.gif);text-align: center; width: 300px;'> ");
        hc.Response.Write("<tr><td style='height: 20px; text-align: center'><marquee direction='right' scrollamount='30' width='290px'> <img height='10' src='images/Progress/plan-wait.gif' width='270' />");
        hc.Response.Write("</marquee></td></tr></table></div>");
        //hc.Response.Write("<script>mydiv.innerText = '';</script>");
        hc.Response.Write("<script type=text/javascript>");
        //最重要是这句了,重写文档的onreadystatechange事件,判断文档是否加载完毕
        hc.Response.Write("function document.onreadystatechange()");
        hc.Response.Write(@"{ try  
                                   {
                                    if (document.readyState == 'complete') 
                                    {
                                         delNode('loading');
                                        
                                    }
                                   }
                                 catch(e)
                                    {
                                        alert('页面加载失败');
                                    }
                                                        } 

                            function delNode(nodeId)
                            {   
                                try
                                {   
                                      var div =document.getElementById(nodeId); 
                                      if(div !==null)
                                      {
                                          div.parentNode.removeChild(div);   
                                          div=null;    
                                          CollectGarbage(); 
                                      } 
                                }
                                catch(e)
                                {   
                                   alert('删除ID为'+nodeId+'的节点出现异常');
                                }   
                            }

                            ");

        hc.Response.Write("</script>");
        hc.Response.Flush();
    }
    public void InitLoading()
    {
        HttpContext hc = HttpContext.Current;
        //创建一个页面居中的div
        hc.Response.Write("<style>");
        hc.Response.Write("#loader_container {text-align:center; position:absolute; top:40%; width:100%; left: 0;}");
        hc.Response.Write("#loader {font-family:Tahoma, Helvetica, sans; font-size:11.5px; color:#000000; background-color:#FFFFFF; padding:10px 0 16px 0; margin:0 auto; display:block; width:320px; border:1px solid #5a667b; text-align:left; z-index:2;}");
        hc.Response.Write("#progress {height:8px; font-size:1px; width:34px; position:relative; top:1px; left:0px; background-color:#8894a8;}");
        hc.Response.Write("#loader_bg {background-color:#e4e7eb; position:relative; top:8px; left:22px; height:10px; width:270px; font-size:1px;}");
        hc.Response.Write("</style>");
        hc.Response.Write("<div id=loader_container>");
        hc.Response.Write("<div id=loader>");
        hc.Response.Write("<div align=center style='font-size:20px'>页面正在加载中 ...</div>");
        hc.Response.Write("<div id=loader_bg><marquee direction='right' scrollamount='10'><div id=progress> </div></marquee></div>");
        hc.Response.Write("</div></div>");
        //hc.Response.Response.Write("<script>mydiv.innerText = '';</script>");
        hc.Response.Write("<script type=text/javascript>");
        //最重要是这句了,重写文档的onreadystatechange事件,判断文档是否加载完毕
        hc.Response.Write("function document.onreadystatechange()");
        hc.Response.Write(@"{ try  
                                   {
                                    if (document.readyState == 'complete') 
                                    {
                                         delNode('loader_container');
                                        
                                    }
                                   }
                                 catch(e)
                                    {
                                        alert('页面加载失败');
                                    }
                                                        } 

                            function delNode(nodeId)
                            {   
                                try
                                {   
                                      var div =document.getElementById(nodeId); 
                                      if(div !==null)
                                      {
                                          div.parentNode.removeChild(div);   
                                          div=null;    
                                          CollectGarbage(); 
                                      } 
                                }
                                catch(e)
                                {   
                                   alert('删除ID为'+nodeId+'的节点出现异常');
                                }   
                            }

                            ");

        hc.Response.Write("</script>");
        hc.Response.Flush();
    }

    protected int GetPageCount(int TotalCount, int PageSize)
    {
        int pageCount = 1;

        pageCount = TotalCount / PageSize;
        int p = TotalCount % PageSize;
        if (p > 0)
            pageCount += 1;

        return pageCount;
    }

    /// <summary>
    /// 往前台 寫入變量\下拉控件js結構\提示信息
    /// </summary>
    ///  <param name="strddlcols">要寫入所有下拉控件名稱 "ddl1,ddl2" </param>
    public void writeJsvar(string FormID, string SqlCmd, string id)
    {
        String cstext = "var FormID = '" + FormID + "';" +
                        "var id = '" + id.Trim() + "';" +
                        "var SqlCmd = '" + SqlCmd.Trim() + "';" +
                        "var SubModuleCode='" + SubModuleCode + "';" +
                        "var HostPath='" + ResolveUrl("~/") + "';" +
                        "var SubModuleTitle='" + SubModuleTitle + "';";

        this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "Jsvar", cstext.ToString(), true);
    }

    /// <summary>
    /// 將日期格式yyyy/MM/dd
    /// </summary>
    public string ToYMD(object dateobj)
    {
        return DateFormat(dateobj, "yyyy/MM/dd");
    }
    /// <summary>
    /// 將日期格式yyyy/MM/dd HH:mm
    /// </summary>
    public string ToYMDHM(object dateobj)
    {
        return DateFormat(dateobj, "yyyy/MM/dd HH:mm:ss");
    }
    private string DateFormat(object dateobj, string strFormat)
    {
        if (dateobj == null || dateobj.ToString().Trim().Length == 0) return "";
        return ((DateTime)dateobj).ToString(strFormat).Replace("-", "/");   //"yyyy/MM/dd HH:mm"     
    }

    public void SetTextReadOnly(params TextBox[] ctls)
    {
        for (int i = 0; i < ctls.Length; i++)
        {
            if (ctls[i] != null)
                ctls[i].Attributes.Add("ReadOnly", "true");
        }
    }
    //主表显示
    public DataTable SetBtnEnabled(int PageIndex, string SqlCmd, string Filter, int pageSize, GridView dgview, LinkButton btnFirst, LinkButton btnPre, LinkButton btnNext, LinkButton btnLast, LinkButton btnToPage, Label lblCurrentPage, UpdatePanel UpdatePanel1)
    {
        int pageCount = 0;
        int totalCount = 0;
        BLL.BLLBase bll = new BLL.BLLBase();
        DataTable dtView = bll.GetDataPage(SqlCmd, PageIndex, pageSize, out totalCount, out pageCount, new DataParameter[] { new DataParameter("{0}", Filter) });


        if (ViewState["CurrentPage"].ToString() == "0" || int.Parse(ViewState["CurrentPage"].ToString()) > pageCount)
            ViewState["CurrentPage"] = pageCount;



        if (dtView.Rows.Count == 0)
        {
            SetGridViewEmptyRow(dgview, dtView);

            btnFirst.Enabled = false;
            btnPre.Enabled = false;
            btnNext.Enabled = false;
            btnLast.Enabled = false;
            btnToPage.Enabled = false;
            lblCurrentPage.Visible = false;

        }
        else
        {
            dgview.DataSource = dtView;
            dgview.DataBind();

            btnLast.Enabled = true;
            btnFirst.Enabled = true;
            btnToPage.Enabled = true;

            if (int.Parse(ViewState["CurrentPage"].ToString()) > 1)
                btnPre.Enabled = true;
            else
                btnPre.Enabled = false;

            if (int.Parse(ViewState["CurrentPage"].ToString()) < pageCount)
                btnNext.Enabled = true;
            else
                btnNext.Enabled = false;

            lblCurrentPage.Visible = true;
            lblCurrentPage.Text = "共 [" + totalCount.ToString() + "] 笔记录  第 [" + ViewState["CurrentPage"] + "] 页  共 [" + pageCount.ToString() + "] 页";

        }
        ViewState[FormID + "_MainFormData"] = dtView;
        return dtView;

    }



    //子表显示
    public void MovePage(string strState, GridView dgv, int pageindex, LinkButton btnFirst, LinkButton btnPre, LinkButton btnNext, LinkButton btnLast, LinkButton btnToPage, Label lblPage)
    {
        if (strState.ToLower() == "edit")
        {
            UpdateTempSub(dgv);
        }

        int pindex = pageindex;
        if (pindex < 0) pindex = 0;

        if (pindex >= dgv.PageCount) pindex = dgv.PageCount - 1;
        DataTable dt1 = (DataTable)ViewState[FormID + "_" + strState + "_" + dgv.ID];


        if (dt1.Rows.Count > 0)
        {
            dgv.PageIndex = pindex;
            dgv.DataSource = dt1;
            dgv.DataBind();

            bool blnvalue = dgv.PageCount > 1 ? true : false;
            btnLast.Enabled = blnvalue;
            btnFirst.Enabled = blnvalue;
            btnToPage.Enabled = blnvalue;


            if (dgv.PageIndex >= 1)
                btnPre.Enabled = true;
            else
                btnPre.Enabled = false;

            if ((dgv.PageIndex + 1) < dgv.PageCount)
                btnNext.Enabled = true;
            else
                btnNext.Enabled = false;

            lblPage.Visible = true;
            lblPage.Text = "共 [" + dt1.Rows.Count.ToString() + "] 条记录  页次 " + (dgv.PageIndex + 1) + " / " + dgv.PageCount.ToString();
        }
        else
        {
            SetGridViewEmptyRow(dgv, strState);
        }

    }

    public void SetGridViewEmptyRow(GridView dgv, DataTable dt)
    {
        if (dt.Rows.Count == 0)
        {
            DataTable dtnew = dt.Clone();
            dtnew.Rows.Add(dtnew.NewRow());

            dgv.DataSource = dtnew;
            dgv.DataBind();
            int columnCount = dgv.Rows[0].Cells.Count;
            dgv.Rows[0].Cells.Clear();
            dgv.Rows[0].Cells.Add(new TableCell());
            dgv.Rows[0].Cells[0].ColumnSpan = columnCount;
            dgv.Rows[0].Cells[0].Text = "没有符合以上条件的数据,请重新查询 ";
            dgv.Rows[0].Visible = true;
        }
    }

    /// <summary>
    /// 设置无产品资料时
    /// </summary>
    /// <param name="dgv"></param>
    /// <param name="strState"></param>
    public void SetGridViewEmptyRow(GridView dgv, string strState)
    {
        DataTable dt1 = (DataTable)ViewState[FormID + "_" + strState + "_" + dgv.ID];
        if (dt1.Rows.Count == 0)
        {
            DataTable dtNew = dt1.Clone();
            dtNew.Rows.Add(dtNew.NewRow());
            dgv.DataSource = dtNew;
            dgv.DataBind();
            int columnCount = dgv.Rows[0].Cells.Count;
            dgv.Rows[0].Cells.Clear();
            dgv.Rows[0].Cells.Add(new TableCell());
            dgv.Rows[0].Cells[0].ColumnSpan = columnCount;
            dgv.Rows[0].Cells[0].Text = "没有明细资料";
            dgv.Rows[0].Visible = true;
        }
    }
    public void BtnReloadSub(int i, string value, GridView dgv)
    {

        BindDataSub(value);
        DataTable dt = (DataTable)ViewState[FormID + "_MainFormData"];
        if (dt.Rows.Count == 0)
        {
            SetGridViewEmptyRow(dgv, dt);
            return;
        }
        for (int j = 0; j < dgv.Rows.Count; j++)
        {
            if (j % 2 == 0)
                dgv.Rows[j].BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
            else
                dgv.Rows[j].BackColor = System.Drawing.ColorTranslator.FromHtml("#E9F2FF");
            if (j == i)
                dgv.Rows[j].BackColor = System.Drawing.ColorTranslator.FromHtml("#60c0ff");
        }
    }


    public virtual void UpdateTempSub(GridView dgv)
    {

    }

    public virtual void BindDataSub(string BillID)
    {

    }
}
