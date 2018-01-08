<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserArea.aspx.cs" Inherits="WebUI_SysInfo_UserManage_UserArea" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    
    <title></title>
    <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
    <link href="~/Css/op.css" type="text/css" rel="stylesheet" /> 
    <script type="text/javascript" src="../../../JQuery/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src= "../../../JScript/Common.js"></script>
    <script type="text/javascript">
        function Close() {
            window.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table class="maintable" style="width:100%;">
            <tr>
                <td class="musttitle" style=" width:120px">
                    用户账号
                </td>
                <td >
                  &nbsp;<asp:TextBox ID="txtUserName" runat="server"  CssClass="TextRead" Width="92%" ></asp:TextBox>
                  <asp:HiddenField ID="txtUserID" runat="server" />
                </td>
            </tr>
            <tr style="height:80px">
                <td class="musttitle">
                    管理库区
                </td>
                <td >
                     &nbsp;<asp:CheckBoxList 
                            ID="chkArea" runat="server"  Width="92%" RepeatColumns="2"  CssClass="CheckBoxList"
                            RepeatDirection="Horizontal" BorderStyle="None" CellPadding="5" 
                         CellSpacing="5"></asp:CheckBoxList>
                </td>
            </tr>
            <tr style="height:40px">
                <td align="center" colspan="2">
                     <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="button" OnClick="btnSave_Click"/>&nbsp;
                      <asp:Button ID="Button1" runat="server" Text="取消" CssClass="button" OnClientClick="Close();"/>
                </td>
            </tr>

        </table>
    
    </div>
    </form>
</body>
</html>
