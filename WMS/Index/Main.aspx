<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Main.aspx.cs" Inherits="Index_Main" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server"> 
<style type="text/css">
</style>
    <title>主页</title>
        <link href="../Css/Main.css" type="text/css" rel="stylesheet" />
       <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/Common.js") %>'></script>
        <script type="text/javascript" language="javascript">
            function SetNewColor(source) {
                _oldColor = source.style.backgroundColor;
                source.style.backgroundColor = '#C0E4EE';
                source.style.cursor = "pointer";
                source.style.border = "1px solid #5384bb";
            }
            function SetOldColor(source) {
                source.style.backgroundColor = _oldColor;
                source.style.cursor = "default";
                source.style.border = "1px solid #EAF2F4";

            }
   
        </script>
</head>
<body bgcolor="#F8FCFF" style="margin-top:30px;">
    <form id="form1" runat="server">
         <input type="hidden" runat="server" id="hdnMsg" /> 
         <input type="hidden" runat="server" id="hdnProduct" /> 
         <input type="hidden" runat="server" id="hdnTask" />
    </form>
        
</body>
</html>
 