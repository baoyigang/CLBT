<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Temp.aspx.cs" Inherits="WebUI_SysInfo_UserManage_Temp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

 
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>库区管理</title>
   
</head>
<body>
    <iframe scrolling="no" frameborder="0" width="100%" height="100%" src="UserArea.aspx?UserID=<%= UserID%>" ></iframe>
</body>
</html>
