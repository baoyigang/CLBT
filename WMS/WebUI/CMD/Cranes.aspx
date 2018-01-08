<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Cranes.aspx.cs" Inherits="WebUI_CMD_Cranes" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
    <link href="~/Css/op.css" type="text/css" rel="stylesheet" /> 
    <script type="text/javascript" src="../../JQuery/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src= "../../JScript/Common.js"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $(window).resize(function () {
                resize();
            });
        });
        function resize() {
            var h = document.documentElement.clientHeight - 58;
            $("#table-container").css("height", h);
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
                        <img alt="Loading" src="../../images/loading.gif" />
                </div>      
            </ProgressTemplate> 
        </asp:UpdateProgress>  
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">                
            <ContentTemplate>
                <div>
                    <table  style="width: 100%; height: 20px;" >
                    <tr>
						    <td class="smalltitle" align="center" width="7%" >
                                <asp:Literal ID="Literal1" Text="查询栏位" runat="server"  ></asp:Literal>
                             </td>
						    <td  width="15%" height="20">&nbsp;<asp:dropdownlist id="ddlField" runat="server" Width="85%" >
                                    <asp:ListItem Selected="True" Value="CraneNo">堆垛机编码</asp:ListItem>
                                    <asp:ListItem  Value="CraneName">堆垛机名称</asp:ListItem>
                                    <asp:ListItem Value="Memo">备注</asp:ListItem>
                                 </asp:dropdownlist>
                            </td>
						    <td class="smalltitle" align="center" width="7%">
                                <asp:Literal ID="Literal2" Text="查询内容" runat="server"></asp:Literal>
                            </td>
						    <td  width="26%" height="20" valign="middle">&nbsp;<asp:textbox id="txtSearch" 
                                    tabIndex="1" runat="server" Width="90%" CssClass="TextBox"  
                                    heigth="16px" ></asp:textbox>
                               
                          </td>
                          <td width="15%" align="left">
                           &nbsp;<asp:button id="btnSearch" tabIndex="2" runat="server" Width="58px" 
                                    CssClass="ButtonQuery" Text="查询" OnClientClick="return Search()" 
                                    onclick="btnSearch_Click"></asp:button>&nbsp;&nbsp;
                              <asp:Button ID="btnRefresh" runat="server" CssClass="ButtonRefresh" 
                                  onclick="btnSearch_Click" OnClientClick="return Refresh()" tabIndex="2" 
                                  Text="刷新" Width="58px" />
                          
                          </td>
                          <td align="right"  style="width:30%" valign="middle">
                             <%-- <asp:Button ID="btnPrint" runat="server" Text="导出" CssClass="ButtonPrint" OnClientClick="return print();"/>--%>
                           
                              &nbsp; &nbsp;
                           
                            <asp:Button ID="btnPrint" runat="server" Text="打印" CssClass="ButtonPrint"/>&nbsp;
                            <asp:Button ID="btnExit" runat="server" Text="离开" CssClass="ButtonExit" OnClientClick="return Exit()" Width="51px" />&nbsp;&nbsp;
                            
                          </td>
                    </tr>
                </table>
                    
                </div>
                <div id="table-container" style="overflow: auto; WIDTH: 100%; HEIGHT: 470px">
                
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SkinID="GridViewSkin" Width="100%" OnRowDataBound="GridView1_RowDataBound">
                <Columns>
                    <asp:TemplateField >
                        <HeaderTemplate>
                        <input type="checkbox" onclick="selectAll('GridView1',this.checked);" />                    
                        </HeaderTemplate>
                        <ItemTemplate>
                        <asp:CheckBox id="cbSelect" runat="server" ></asp:CheckBox>                   
                        </ItemTemplate>
                      <HeaderStyle Width="60px"></HeaderStyle>
                     <ItemStyle Width="60px" HorizontalAlign="Center"></ItemStyle>
                   </asp:TemplateField>
                  <asp:TemplateField HeaderText="堆垛机编码" SortExpression="CraneNo">
                        <ItemTemplate>
                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# FormID+"View.aspx?SubModuleCode=" + SubModuleCode+"&FormID=" + FormID +"&SqlCmd="+SqlCmd+ "&ID="+DataBinder.Eval(Container.DataItem, "CraneNo") %>'
                                Text='<%# DataBinder.Eval(Container.DataItem, "CraneNo")%>'></asp:HyperLink>
                        </ItemTemplate>
                        <ItemStyle Width="12%" Wrap="False" />
                        <HeaderStyle Width="12%" Wrap="False" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="CraneName" HeaderText="堆垛机名称" SortExpression="CraneName">
                        <ItemStyle HorizontalAlign="Left" Width="20%" Wrap="False" />
                        <HeaderStyle Wrap="False" />
                    </asp:BoundField>
                     <asp:BoundField DataField="StateDesc" HeaderText="状态" SortExpression="StateDesc">
                        <ItemStyle HorizontalAlign="Left" Width="20%" Wrap="False" />
                        <HeaderStyle Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Memo" HeaderText="备注" 
                        SortExpression="Memo" >
                        <ItemStyle HorizontalAlign="Left" Width="40%" Wrap="False" />
                        <HeaderStyle Wrap="False" />
                    </asp:BoundField>
                     
               
                </Columns>
            <PagerSettings Visible="False" />
        </asp:GridView>
            </div>
                <div>
                    &nbsp;&nbsp;<asp:LinkButton ID="btnFirst" runat="server" OnClick="btnFirst_Click" Text="首页"></asp:LinkButton> 
                    &nbsp;<asp:LinkButton ID="btnPre" runat="server" OnClick="btnPre_Click" Text="上一页"></asp:LinkButton> 
                    &nbsp;<asp:LinkButton ID="btnNext" runat="server" OnClick="btnNext_Click" Text="下一页"></asp:LinkButton> 
                    &nbsp;<asp:LinkButton ID="btnLast" runat="server" OnClick="btnLast_Click" Text="尾页"></asp:LinkButton> 
                    &nbsp;<asp:textbox id="txtPageNo" onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))"
					        onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))"
					        runat="server" Width="56px" CssClass="TextBox" ></asp:textbox>
                    &nbsp;<asp:linkbutton id="btnToPage" runat="server" onclick="btnToPage_Click" Text="跳转"></asp:linkbutton>
                    &nbsp;<asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True" onselectedindexchanged="ddlPageSize_SelectedIndexChanged" Visible="false"></asp:DropDownList>
                    &nbsp;<asp:Label ID="lblCurrentPage" runat="server" ></asp:Label>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
