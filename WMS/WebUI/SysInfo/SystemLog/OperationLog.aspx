<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OperationLog.aspx.cs" Inherits="WebUI_SysInfo_SystemLog_OperationLog" %>
<%@ Register src="../../../UserControl/Calendar.ascx" tagname="Calendar" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
   <title>操作日志</title>
    <script type="text/javascript" src="../../../JQuery/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src="../../../JScript/Common.js"></script>
    <link href="../../../css/main.css" rel="Stylesheet" type="text/css" />
    <link href="../../../css/op.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            $(window).resize(function () {
                resize();
            });
        });
        function resize() {
            var h = document.documentElement.clientHeight - 55;
            $("#table-container").css("height", h);
        }
        function DelConfirm(btnID) {
            var table = document.getElementById('gvMain');
            var hasChecked = false;
            for (var i = 1; i < table.rows.length; i++) {
                var cell = table.rows[i].cells[0];
                var chk = cell.getElementsByTagName("INPUT");
                if (chk[0].checked) {
                    hasChecked = true;
                    break;
                }
            }

            if (!hasChecked) {
                alert('请选择要删除的数据！');
                return false;
            }
            if (confirm('确定要删除选择的数据？', '删除提示')) {
                var btn = document.getElementById(btnID);
                btn.click();
                //window.location.reload();
            }
            else {
                return false;
            }
        }

        function ClearConfirm() {
            if (confirm('确定要清空所有日志记录', '清空提示')) {

            }
            else {
                return false;
            }
        }
        </script>
</head>
<body>
    <form id="form1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server" />  
        <asp:UpdateProgress ID="updateprogress" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>            
                <div id="progressBackgroundFilter" style="display:none"></div>
                <div id="processMessage"> Loading...<br /><br />
                        <img alt="Loading" src="../../../images/loading.gif" />
                </div>      
            </ProgressTemplate> 
        </asp:UpdateProgress>  
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">                
            <ContentTemplate>
                <!--工具栏-->
            <div>
                 <table class="maintable"  width="100%" align="center" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="smalltitle" align="center" width="5%" >
                            查询栏位
                        </td>
                        <td  width="8%"> 
                            &nbsp;<asp:DropDownList ID="ddl_Field" runat="server"  Width="89%">
                                    <asp:ListItem Selected="True" Value="LoginUser">操作用户</asp:ListItem>
                                    <asp:ListItem Value="LoginModule">操作模块</asp:ListItem>
                                </asp:DropDownList>
                        </td>
                            <td class="smalltitle" align="center" width="5%">
                            查询内容
                        </td>
                        <td height="20" width="10%" valign="middle" >
                                &nbsp;<asp:TextBox ID="txtKeyWords" runat="server" CssClass="TextBox" Width="85%"></asp:TextBox>
                        </td>
                        <td align="center" class="smalltitle" width="4%">
                            日期从
                        </td>
                        <td  style="width:120px;">
                            &nbsp;<uc1:Calendar ID="txtStartDate" runat="server" />
                        </td>
                        <td align="center" class="smalltitle" width="1%">
                            至
                        </td>
                        <td   style="width:120px;">
                             &nbsp;<uc1:Calendar ID="txtEndDate" runat="server" />
                        </td>
                        <td  width="15%">
                            <asp:Button ID="btnQuery" runat="server" Text="查询" OnClick="btnQuery_Click" CssClass="ButtonQuery" />
                            <asp:Button ID="btnRefresh" runat="server" CssClass="ButtonRefresh" OnClientClick="return Refresh()" tabIndex="2"  Text="刷新" Width="58px" />
                        </td>
                           
                        <td align="right">
                            <asp:Button ID="btnDelete" runat="server" Text="删除" CssClass="ButtonDel" OnClick="btnDelete_Click"
                                OnClientClick="return DelConfirm('btnDelete')" Enabled="False" />
                            <asp:Button ID="btnDeleteAll" runat="server" Text="清空" CssClass="ButtonClearAll"
                                OnClientClick="return ClearConfirm();" Enabled="False" OnClick="btnDeleteAll_Click" />
                            <asp:Button ID="btnExit" runat="server" Text="退出" OnClientClick="return Exit();" CssClass="ButtonExit" />
                        </td>
                         
                    </tr>
                         
                </table>  
                    
                   
                <!--数据-->
                  <div id="table-container" style="overflow: auto; WIDTH: 100%; HEIGHT: 470px">
                   <asp:GridView ID="gvMain" runat="server"  Width="100%"  OnRowDataBound="gvMain_RowDataBound"
                          SkinID="GridViewSkin"  AutoGenerateColumns="False">
                        <RowStyle BackColor="WhiteSmoke" Height="28px" />
                        <HeaderStyle CssClass="GridHeader" />
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                           <asp:TemplateField>
                                <HeaderTemplate>
                                <input type="checkbox" onclick="selectAll('gvMain',this.checked);" />                    
                                </HeaderTemplate>
                                <ItemTemplate>
                                <asp:CheckBox id="cbSelect" runat="server" ></asp:CheckBox>                   
                                </ItemTemplate>
                              <HeaderStyle Width="60px"></HeaderStyle>
                             <ItemStyle Width="60px" HorizontalAlign="Center"></ItemStyle>
                           </asp:TemplateField>
                            <asp:BoundField DataField="LoginUser" HeaderText="操作用户">
                                <HeaderStyle Width="120px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="LoginTime" HeaderText="操作时间">
                                <HeaderStyle Width="120px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="LoginModule" HeaderText="操作模块">
                                <HeaderStyle Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ExecuteOperator" HeaderText="操作内容"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                  </div>
                <!--分页导航-->
                  <div>
                     &nbsp;&nbsp;<asp:LinkButton ID="btnFirst" runat="server" OnClick="btnFirst_Click" Text="首页"></asp:LinkButton> 
                    &nbsp;<asp:LinkButton ID="btnPre" runat="server" OnClick="btnPre_Click" Text="上一页"></asp:LinkButton> 
                    &nbsp;<asp:LinkButton ID="btnNext" runat="server" OnClick="btnNext_Click" Text="下一页"></asp:LinkButton> 
                    &nbsp;<asp:LinkButton ID="btnLast" runat="server" OnClick="btnLast_Click" Text="尾页"></asp:LinkButton> 
                    &nbsp;<asp:textbox id="txtPageNo" onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))"
					        onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))"
					        runat="server" Width="56px" CssClass="TextBox" ></asp:textbox>
                    &nbsp;<asp:linkbutton id="btnToPage" runat="server" onclick="btnToPage_Click" Text="跳转"></asp:linkbutton>
                    &nbsp;<asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True" Visible="false"></asp:DropDownList>
                    &nbsp;<asp:Label ID="lblCurrentPage" runat="server" ></asp:Label>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!--隐藏数据-->
    
    </form>
</body>
</html>
