<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CarView.aspx.cs" Inherits="WebUI_CMD_CarView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
    <link href="~/Css/op.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../../JQuery/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src= "../../JScript/Common.js"></script> 
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />  
        <asp:UpdateProgress ID="updateprogress" runat="server" AssociatedUpdatePanelID="updatePanel">
            <ProgressTemplate>            
                <div id="progressBackgroundFilter" style="display:none"></div>
                <div id="processMessage"> Loading...<br /><br />
                     <img alt="Loading" src="../../images/loading.gif" />
                </div>            
             </ProgressTemplate>
        </asp:UpdateProgress>  
        <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">                
            <ContentTemplate>
                 <table   style="width: 100%; height: 25px;" class="OperationBar">
                    <tr>
                        <td align="right" style="width:60%">
                            <asp:Button ID="btnFirst" runat="server" Text="首笔" CssClass="ButtonFirst" 
                                onclick="btnFirst_Click"  />
                            <asp:Button ID="btnPre" runat="server" Text="上一笔" CssClass="ButtonPre" 
                                onclick="btnPre_Click"  />
                            <asp:Button ID="btnNext" runat="server" Text="下一笔" CssClass="ButtonNext" 
                                onclick="btnNext_Click"  />
                            <asp:Button ID="btnLast" runat="server" Text="尾笔" CssClass="ButtonLast" 
                                onclick="btnLast_Click"  />
                        </td>
                        <td align="right">
                           <%-- <asp:Button ID="btnPrint" runat="server" Text="导出" CssClass="ButtonPrint" 
                                OnClientClick="return print();" />--%>
                            <asp:Button ID="btnEdit" runat="server" Text="修改" CssClass="ButtonModify" 
                                OnClientClick="return ViewEdit();" />
                            <asp:Button ID="btnBack" runat="server" Text="返回" OnClientClick="return Back();" 
                                CssClass="ButtonBack" />
                            <asp:Button ID="btnExit" runat="server" Text="离开" OnClientClick="return Exit();" 
                                CssClass="ButtonExit"  />
                        
                        </td>
                    </tr>
                 </table>
                 <table id="Table1" class="maintable"  width="100%" align="center" cellspacing="0" cellpadding="0" bordercolor="#ffffff" border="1" 
				     runat="server">			
				     <tr>
                        <td align="center" class="musttitle" style="width:8%;"  >
                                小车编码
                        </td>
                        <td  width="25%">
                                &nbsp;<asp:TextBox ID="txtID" runat="server"   CssClass="TextRead" Width="40%" MaxLength="10" 
                                    ReadOnly="True"  ></asp:TextBox>
                        </td>
                        <td align="center" class="musttitle" style="width:8%;"  >
                                小车名称
                        </td>
                        <td width="25%">
                                &nbsp;<asp:TextBox ID="txtTypeName" runat="server"  
                                    CssClass="TextRead" Width="42%" MaxLength="30" ReadOnly="True"></asp:TextBox> 
                        </td>
                         <td align="center" class="musttitle" style="width:8%;"  >
                                小车名称
                        </td>
                        <td width="26%">
                               &nbsp;<asp:DropDownList ID="ddlActive" runat="server"  Width="72%" 
                                   Enabled="False">
                                    <asp:ListItem Selected="True" Value="1">启用</asp:ListItem>
                                    <asp:ListItem Value="0">禁用</asp:ListItem>
                                </asp:DropDownList>
                        </td>
                         
                    </tr>
                    
                    
                    <tr>
                        <td align="center"  class="smalltitle" style="width:8%;">
                            备注
                        </td>
                        <td colspan="5">
                            &nbsp;<asp:TextBox ID="txtMemo" runat="server" CssClass="TextRead" 
                                TextMode="MultiLine" Height="102px" Width="92%" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    	
		        </table>
                
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
