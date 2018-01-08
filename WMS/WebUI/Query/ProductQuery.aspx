<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProductQuery.aspx.cs" Inherits="WebUI_Query_ProductQuery" %>

<%@ Register Assembly="FastReport.Web" Namespace="FastReport.Web" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title> 
        <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
        <link href="~/Css/op.css" type="text/css" rel="stylesheet" /> 
   
        <script type="text/javascript" src='<%=ResolveClientUrl("~/JQuery/jquery-1.8.3.min.js") %>'></script>
        <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/Common.js") %>'></script>
       <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/DataProcess.js") %>'></script>
       <script type="text/javascript">
           $(document).ready(function () {
               $(window).resize(function () {
                   resize();
               });
           });
           function resize() {
               var h = document.documentElement.clientHeight - 30;
               $("#rptview").css("height", h);
           }
           function PrintClick() {
               $('#HdnWH').val(document.documentElement.clientWidth + "#" + document.documentElement.clientHeight);
               return true;
           }
        </script>
   
    </head>
<body  style="overflow:hidden;">
  <form id="form1" runat="server"> 
    <table  style="width:100%;height:100%;" >
        <tr runat ="server" id = "rptform" valign="top">
            <td align="left" style="width:100%; height:30px;" >
                <table class="maintable"  width="100%" align="center" >
                    <tr>
                         <td   align="center" class="musttitle" style="width:6%;">
                             产品规格 
                        </td>
                        <td align="left"   style="width:12%;" >
                         <asp:textbox id="txtSpec"   runat="server"  Width="98%" 
                                CssClass="TextBox" ></asp:textbox>
                        </td>
                        
                        <td   align="center" class="musttitle" style="width:5%;">
                            熔次卷号
                        </td>
                        <td align="left"   style="width:18%;" >
                         <asp:textbox id="txtBarCode"   runat="server"  Width="97%" CssClass="TextBox" ></asp:textbox>
                        </td>
                        <td   align="center" class="musttitle" style="width:5%;">
                            库区
                        </td>
                        <td align="left"   style="width:18%;" >
                         <asp:textbox id="txtAreaCode"   runat="server"  Width="95%" CssClass="TextBox" ></asp:textbox>
                        </td>
                        
                        <td class="musttitle" align="center" style="width:5%;" >
                            报表
                         </td>
                         <td width="15%" align="left">
                             <asp:RadioButton ID="rpt1" runat="server" Checked="True" GroupName="Rpt" Text="明细表" />&nbsp;
                             <asp:RadioButton ID="rpt2" runat="server" GroupName="Rpt" Text="统计表" />&nbsp;               
                        </td>
                        <td align= "left" style="border-left:2px solid #ffffff;">
                             &nbsp;<asp:Button ID="btnSearch" runat="server" CssClass="ButtonQuery" 
                                 onclick="btnPreview_Click" tabIndex="2" Text="查询" Width="58px" 
                                onclientclick="return PrintClick();" /> &nbsp;&nbsp;
                             <asp:Button ID="btnRefresh" runat="server" CssClass="ButtonReset" 
                                 OnClientClick="return Refresh()" tabIndex="2" 
                                 Text="重新过滤" Width="80px" />
                        </td>  
                                                                     
                    </tr>
                </table>  
            </td>
        </tr>
        <tr>
            <td runat ="server" id = "rptview" valign="top" align="left">
                <table style="width:90%;height:100%;">
                    <tr>
                        <td >           
                            <cc1:WebReport ID="WebReport1" runat="server" BackColor="White" ButtonsPath="images\buttons1"
                                Font-Bold="False" Height = "100%" OnStartReport="WebReport1_StartReport"
                                ToolbarColor="Lavender" Width="100%" Zoom="1" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
         
        <input id="HdnProduct" type="hidden" runat="server" />
      
       <input id="HdnWH" type="hidden" runat="server" value="0#0" />
         
   </form>
</body>
</html>