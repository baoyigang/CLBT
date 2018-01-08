<%@ page language="C#" autoeventwireup="true" inherits="WebUI_Query_TaskQuery, WMS_deploy" theme="Default" stylesheettheme="Default" %>
 
<%@ Register Assembly="FastReport.Web" Namespace="FastReport.Web" TagPrefix="cc1" %>
<%@ Register src="../../UserControl/Calendar.ascx" tagname="Calendar" tagprefix="uc1" %>
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
               BindEvent();
           });
           function resize() {
               var h = document.documentElement.clientHeight - 60;
               $("#rptview").css("height", h);
           }
           function ProductClick() {
               var where = GetWhere();
               getMultiItems("CMD_Product", "ProductCode", $('#btnProduct'), '#HdnProduct', where);
               return false;
           }
           function BindEvent() {
               $("#txtProductCode").bind("dblclick", function () {
                   var where = GetWhere();
                   GetOtherValue("CMD_Product", "txtProductName,txtProductCode", "ProductName,ProductCode", where);
                   return false;
               });
               $("#txtProductCode").bind("change", function () {
                   var where = GetWhere();
                   where += " and ProductCode='" + $('#txtProductCode').val() + "'";
                   getWhereBaseData('CMD_Product', "txtProductName,txtProductCode", "ProductName,ProductCode", where);
               });
           }

           function GetWhere() {
               var where = "ProductCode not in ('0001','0002')";
               if ($("#ddlProductType").val() != "") {
                   where += escape(" and ProductTypeCode='" + $('#ddlProductType').val() + "'");
               }
               if ($("#ddlArea").val() != "") {
                   where += escape(" and AreaCode='" + $('#ddlArea').val() + "'");
               }
               return where;
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
                <table class="maintable"  width="100%" align="center" cellspacing="0" cellpadding="0">
                    <tr  style=" border-bottom:1px solid #ffffff;" >
                        <td   align="center" class="musttitle" style=" width:6%" >
                            作业日期 
                        </td>
                        <td align="left"   style="width:115px;" >
                            <uc1:Calendar ID="txtStartDate" runat="server"  /> 
                        </td> 
                        <td align="center" style=" width:3%">
                        至
                        </td>                                
                        <td align="left"   style="width:115px;" >
                             <uc1:Calendar ID="txtEndDate" runat="server" />
                         </td>
                         
                        <td align="center" class="musttitle" style=" width:6%">
                            任务类型 
                        </td>
                        <td align="left" style="width:10%;">
                            <asp:DropDownList ID="ddlBillType" runat="server" Width="96%">
                            </asp:DropDownList>
                        </td>
                             
                        <td align="center" class="musttitle" style=" width:6%">
                            任务状态</td>
                        <td align="left" style="width:10%;">
                           
                            <asp:DropDownList ID="ddlState" runat="server" Width="96%">
                                <asp:ListItem Selected="True" Value="0">请选择</asp:ListItem>
                                <asp:ListItem Value="1">未完成</asp:ListItem>
                                <asp:ListItem Value="2">完成</asp:ListItem>
                                <asp:ListItem Value="3">取消</asp:ListItem>
                            </asp:DropDownList>
                           
                        </td>
                         <td align="center" class="musttitle" style=" width:6%">
                             库区</td>
                        <td align="left" style="width:10%;">
                           
                            <asp:DropDownList ID="ddlArea" runat="server" Width="96%" AutoPostBack="True" 
                                onselectedindexchanged="ddlAreaCode_SelectedIndexChanged">
                            </asp:DropDownList>
                           
                        </td>
                        <td align="center" class="musttitle" style=" width:6%">  
                            产品类型
                        </td>
                            
                        <td align="left"  >
                            <asp:DropDownList ID="ddlProductType" runat="server" Width="96%">
                            </asp:DropDownList>
                           
                        </td>                                         
                    </tr>
                    <tr>
                        <td   align="center" class="musttitle">
                            产品
                        </td>
                         <td colspan="3">
                             <asp:TextBox ID="txtProductCode" runat="server" CssClass="TextBox" Width="30%"></asp:TextBox>
                            <asp:TextBox ID="txtProductName" runat="server" CssClass="TextRead" 
                                tabIndex="1" Width="63%"></asp:TextBox>
                         </td>
                         
                        <td  >
                             <asp:Button ID="btnProduct" runat="server" CssClass="ButtonOption" 
                                Height="23px" OnClientClick="return ProductClick();" Text="指定" Width="70px" />
                        </td>
                        <td align="center"  colspan="5" >
                             &nbsp;<asp:Button ID="btnSearch" runat="server" CssClass="ButtonQuery" 
                                onclick="btnPreview_Click" onclientclick="return PrintClick();" tabIndex="2" 
                                Text="查询" Width="58px" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnRefresh" runat="server" CssClass="ButtonReset" 
                                OnClientClick="return Refresh()" tabIndex="2" Text="重新过滤" Width="80px" />
                        </td>
                        <td colspan="2">
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