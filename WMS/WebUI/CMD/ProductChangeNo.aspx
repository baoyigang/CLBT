<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProductChangeNo.aspx.cs" Inherits="WebUI_CMD_ProductChangeNo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
     <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
    <link href="~/Css/op.css" type="text/css" rel="stylesheet" /> 
    <script type="text/javascript" src="../../JQuery/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src= "../../JScript/Common.js"></script>
    <script type="text/javascript">
        function ButtonBack() {
            var strID = $('#txtProductCode').val();
            window.location = '<%= FormID %>' + "View.aspx?SubModuleCode=" + '<%=SubModuleCode%>' + "&FormID=" + '<%=FormID%>' + "&SqlCmd=" + '<%=SqlCmd%>' + "&ID=" + strID;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
         <table class="maintable"  style="width:100%; height:100%;"  align="center" cellspacing="0" cellpadding="0"  border="1">
            <tr>
                <td class="musttitle" align="center" style="width:30%">
                     产品编号
                </td>
                <td>
                     <asp:textbox id="txtProductCode"   runat="server"  Width="30%" CssClass="TextRead" ></asp:textbox>
                     <asp:textbox id="txtProductName" tabIndex="1" runat="server" Width="60%" CssClass="TextRead"></asp:textbox>
                     <asp:HiddenField ID="hdnArea" runat="server" />
                </td>
            </tr>
            <tr>
               <td class="musttitle" align="center" style="width:30%" >
                新产品编号
               </td>
               <td>
                  <asp:textbox id="txtProductNewCode"   runat="server"  Width="90%" CssClass="TextBox" ></asp:textbox>
               </td>
            
            </tr>
            <tr>
                <td colspan="2" align="center" style="height:50px;">
                     <asp:Button ID="btnCreate" runat="server"  Text="修改" CssClass="ButtonModify" 
                         onclick="btnCreate_Click"/>&nbsp;
                     <input id="btnExit" value="返回" class=" ButtonBack" onclick="ButtonBack();" style=" width:40px; height:20px;" />
                </td>
            </tr>
         </table>
    </div>
    </form>
</body>
</html>