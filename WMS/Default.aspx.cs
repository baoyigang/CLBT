using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string loginName = Session["G_user"].ToString();
        HiddenField1.Value = loginName;
        string dateTime = DateTime.Now.ToString();
        HiddenField2.Value = dateTime;
    }
}