<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConfigPlan.aspx.cs" Inherits="WebUI_SysInfo_ConfigPlan_ConfigPlan" %>
<%@ Register Assembly="YYControls" Namespace="YYControls" TagPrefix="yyc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
   <link href="../../../css/main.css" rel="Stylesheet" type="text/css" />
    <link href="../../../css/op.css" rel="Stylesheet" type="text/css" />
  
    <script type="text/javascript" src="../../../JQuery/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src="../../../JScript/Common.js"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $(window).resize(function () {
                resize();
            });
        });
        function resize() {
            var h = document.documentElement.clientHeight - 40;
            $("#dvTree").css("height", h);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <table   style="width:100%; height:30px;" >
      <tr>
        <td class="maintable" height="25px" >
            &nbsp;
            <asp:Button ID="btnSave" runat="server" CssClass="ButtonSave" Text="保存" OnClick="btnSave_Click" />
            <asp:Button ID="btnExit" runat="server" CssClass="ButtonExit" Text="退出" OnClientClick="return Exit();" />
         </td>
            
      </tr>
      <tr>
        <td  valign="top">
          <div id="dvTree" style="width:100%; height:450px; overflow:auto;">
           <yyc:SmartTreeView ID="sTreeModule" runat="server" ShowLines="True" AllowCascadeCheckbox="True"></yyc:SmartTreeView>
          </div>
       </td>
      </tr>
</table>
    </form>
</body>
</html>
