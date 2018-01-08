using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_Calendar : System.Web.UI.UserControl
{
    protected string JsDateFormat;
    private string _dateFormat;
    bool _ym = false;
    bool _readonly = false;
    string _changed = "";

    public bool ShowYm
    {
        get
        {
            return _ym;
        }
        set
        {
            _ym = value;
        }
    }


    /// <summary>
    /// 只讀
    /// </summary>
    public bool ReadOnly
    {
        get
        {
            return _readonly;
        }
        set
        {
            _readonly = value;
        }
    }

    public TextBox tDate
    {
        get
        {
            return txtDate;
        }
    }
    public string DateFormat
    {
        get
        {
            return _dateFormat;
        }
        set
        {
            _dateFormat = value;
        }
    }
    public string Text
    {
        get
        {
            return this.txtDate.Text;
        }
        set
        {
            this.txtDate.Text = value;
        }
    }
    /// <summary>
    /// 日期專用
    /// </summary>
    public object DateValue
    {
        get
        {
            if (_ym) return DateTime.Parse("0001/01/01");
            if (this.txtDate.Text == "")
                return DBNull.Value;
            return ParseDateTime(this.txtDate.Text, _dateFormat);
        }
        set
        {
            if (_ym)
            {
                this.txtDate.Text = "";
                return;
            }
            this.txtDate.Text = GetDateTimeString(value, _dateFormat);
            this.txtDate.Attributes.Add("datevalue", GetDateTimeString(value, "yyyy/MM/dd"));
        }
    }

    /// <summary>
    /// 日期專用
    /// </summary>
    public string YmValue
    {
        get
        {
            if (_ym)
            {
                if (_dateFormat.IndexOf('g') != -1)
                    return "" + (int.Parse(this.txtDate.Text.Substring(0, this.txtDate.Text.Length - 3)) + 1911) + "/" + this.txtDate.Text.Substring(this.txtDate.Text.Length - 2, 2);

                return this.txtDate.Text;
            }

            return "";
        }
        set
        {
            if (_ym)
            {
                this.txtDate.Attributes.Add("datevalue", value);
                DateTime t = DateTime.Parse("" + value + "/01");
                if (_dateFormat.IndexOf('g') != -1)
                    this.txtDate.Text = "" + (t.Year - 1911) + _dateFormat.ToLower().Replace("g", "").Replace("m", "").Replace("y", "").Replace("d", "").Substring(1) + (t.Month > 9 ? "" + t.Month : "0" + t.Month);
                else
                    this.txtDate.Text = value;
            }
        }
    }

    public string changed
    {
        set
        {
            _changed = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //this.DateFormat = Js.Common.User.strDateFormat;  
        if (_dateFormat == null || _dateFormat.Length == 0) _dateFormat = "yyyy/MM/dd";
        JsDateFormat = _dateFormat.Replace("yyyy", "y");
        JsDateFormat = JsDateFormat.Replace("MM", "mm");
        JsDateFormat = JsDateFormat.Replace("yy", "y");
        if (_ym)
        {
            string sp = JsDateFormat.Replace("y", "").Replace("m", "").Replace("d", "").Replace("g", "").Substring(1);
            this.txtDate.ToolTip = "输入年月格式为:[YYYY" + sp + "MM] 例如:" + (_dateFormat.IndexOf('g') >= 0 ? DateTime.Now.Year - 1911 : DateTime.Now.Year) + sp + (DateTime.Now.Month > 9 ? "" + DateTime.Now.Month : "0" + DateTime.Now.Month);
        }
        else
            this.txtDate.ToolTip = "输入日期格式为:[" + _dateFormat + "] 例如:" + GetDateTimeString(DateTime.Now, _dateFormat);

        //virtualPath = Js.Com.ConfigHelper.GetConfigString("VirtualPath");
        //if (!Page.ClientScript.IsClientScriptIncludeRegistered("headjquery"))
        //    Page.ClientScript.RegisterClientScriptInclude("headjquery", @"/" + virtualPath + @"/Date_Js/jquery-1.5.1.js ");
        // Define the name and type of the client scripts on the page.

        if (!Page.ClientScript.IsClientScriptIncludeRegistered("headcalendar"))
            Page.ClientScript.RegisterClientScriptInclude("headcalendar", ResolveUrl("~/JScript/Date/lyz.calendar.min.js"));


        //if (!Page.ClientScript.IsClientScriptIncludeRegistered("headcalendar1"))
        //    Page.ClientScript.RegisterClientScriptInclude("headcalendar1", @"/" + virtualPath + @"/Date_Js/calendar.js");
        //this.tDate.Attributes.Add("onblur", "Validate('" + JsDateFormat + "',this);");
        //this.btnDate.Attributes.Add("onclick", " return showCalendar('" + txtDate.ClientID + "', '" + JsDateFormat + "');");

        //Response.Write("<link rel='stylesheet' href='/" + virtualPath + @"/Date_Js/date_style.css' type='text/css'/>");

        //String csname1 = "PopupScript";
        String csname2 = "MyUserLink_date_style";
        Type cstype = this.GetType();

        // Get a ClientScriptManager reference from the Page class.
        ClientScriptManager cs = Page.ClientScript;
        // Check to see if the client script is already registered.
        if (!cs.IsClientScriptBlockRegistered(cstype, csname2))
        {
            System.Text.StringBuilder cstext2 = new System.Text.StringBuilder();
            cstext2.Append("<link href=\"" + ResolveUrl("~/JScript/Date/lyz.calendar.css") + "\" rel=\"stylesheet\" type=\"text/css\" />");
            //cstext2.Append("Form1.Message.value='Text from client script.'} </");
            //cstext2.Append("script>");
            cs.RegisterClientScriptBlock(cstype, csname2, cstext2.ToString(), false);
        }

        //// Check to see if the startup script is already registered.
        //if (!cs.IsStartupScriptRegistered(cstype, csname1 + this.txtDate.ClientID))
        //{
        //    string str = "";
        //    _changed = _changed.Replace("this.value", "$(\"#" + this.txtDate.ClientID + "\").val()");
        //    if (_changed.Length > 0)
        //        str = ",callback:function() {" + _changed + "}";
        //    String cstext1 = "$(\"#" + this.txtDate.ClientID + "\").calendar({readonly: " + (_readonly ? "true" : "false") + ", DateFormat: '" + JsDateFormat + "' ,Ym: " + (_ym ? "true" : "false") + str + "});";
        //    cs.RegisterStartupScript(cstype, csname1 + this.txtDate.ClientID, cstext1, true);
        //}


    }

    protected override void Render(HtmlTextWriter writer)
    {
        base.Render(writer);
        string str = "";
        _changed = _changed.Replace("this.value", "$(\"#" + this.txtDate.ClientID + "\").val()");
        if (_changed.Length > 0)
            str = ",callback:function() {" + _changed + "}";
        String cstext1 = "$(\"#" + this.txtDate.ClientID + "\").calendar({readonly: " + (_readonly ? "true" : "false") + ", DateFormat: '" + JsDateFormat + "' ,Ym: " + (_ym ? "true" : "false") + str + "});" +
                         "if ($(\"#" + this.txtDate.ClientID + "\").attr(\"readonly\")) $(\"#" + this.txtDate.ClientID + "\").attr(\"class\",\"TextRead\"); else $(\"#" + this.txtDate.ClientID + "\").attr(\"class\",\"TextBox\");";

        //cs.RegisterStartupScript(cstype, csname1 + this.txtDate.ClientID, cstext1, true); class="TextRead"  class="TextBox"
        if (ScriptManager.GetCurrent(this.Page) != null)
        {
            ScriptManager.RegisterStartupScript(this, base.GetType(), "init" + this.ClientID, cstext1.ToString(), true);
        }
        else
        {
            this.Page.ClientScript.RegisterStartupScript(base.GetType(), "init" + this.ClientID, cstext1.ToString(), true);
        }
    }

    private string GetDateTimeString(object dttime1, string strFormat)
    {
        if (string.IsNullOrEmpty(strFormat))
            strFormat = "yyyy/MM/dd";
        string strDateValue = "";

        DateTime dttime = new DateTime(1912, 1, 1, 0, 0, 1);
        if (!(dttime1 == null || dttime1.ToString().Trim().Length == 0 || dttime1 == DBNull.Value))
            dttime = (DateTime)dttime1;


        if (dttime <= new DateTime(1912, 1, 1, 0, 0, 1))
            return strDateValue;
        string spliter = "/";

        string strNewFormat = strFormat.Replace("gyyyy", "").Replace("yyyy", "").Replace("MM", "").Replace("dd", "").Trim();
        if (strNewFormat.Length > 0)
            spliter = strNewFormat.Substring(0, 1);
        string[] s = new string[3];
        int y = dttime.Year;
        if (strFormat.IndexOf("g") >= 0)
        {
            y = y - 1911;
        }
        s[0] = y.ToString();
        s[1] = dttime.Month.ToString().PadLeft(2, '0');
        s[2] = dttime.Day.ToString().PadLeft(2, '0');

        if (strFormat.Replace("g", "") == "yyyy" + spliter + "MM" + spliter + "dd")
        {
            strDateValue = s[0] + spliter + s[1] + spliter + s[2];
        }
        if (strFormat.Replace("g", "") == "MM" + spliter + "dd" + spliter + "yyyy")
        {
            strDateValue = s[1] + spliter + s[2] + spliter + s[0];
        }
        if (strFormat.Replace("g", "") == "dd" + spliter + "MM" + spliter + "yyyy")
        {
            strDateValue = s[2] + spliter + s[1] + spliter + s[0];
        }
        //strDateValue = newDate.ToString(strFormat.Replace("g",""), new System.Globalization.CultureInfo("zh-TW", true));

        return strDateValue;
    }
    private DateTime ParseDateTime(string inputDate, string DateFormat)
    {
        DateTime dtime = new DateTime(1912, 1, 1, 0, 0, 1);
        if (!string.IsNullOrEmpty(inputDate))
        {
            try
            {
                int y = 1, m = 1, d = 1;
                char cSpliter = '/';
                if (DateFormat.Replace("gyyyy", "").Replace("yyyy", "").Replace("MM", "").Replace("dd", "").Trim().Length > 0)
                    cSpliter = char.Parse(DateFormat.Replace("gyyyy", "").Replace("yyyy", "").Replace("MM", "").Replace("dd", "").Trim().Substring(0, 1));

                char[] Spliter = { cSpliter, cSpliter };
                string[] a = inputDate.Split(Spliter);
                string[] b = DateFormat.Split(Spliter);
                for (int index = 0; index < b.Length; index++)
                {
                    if (b[index] == "gyyyy")
                    {
                        y = int.Parse(a[index]) + 1911;
                    }
                    if (b[index] == "yyyy")
                    {
                        y = int.Parse(a[index]);
                    }
                    if (b[index] == "MM")
                    {
                        m = int.Parse(a[index]);
                    }
                    if (b[index] == "dd")
                    {
                        d = int.Parse(a[index]);
                    }
                }
                dtime = new DateTime(y, m, d);
            }
            catch
            {

            }
        }
        return dtime;

    }
}