<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MoveView.aspx.cs" Inherits="WebUI_Stock_MoveView" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
    <link href="~/Css/op.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../../JQuery/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src= "../../JScript/Common.js"></script> 
    <script type="text/javascript">
        $(document).ready(function () {
            $(window).resize(function () {
                resize();

            });
        });
        function resize() {
            var h = document.documentElement.clientHeight - 170;
            $("#Sub-container").css("height", h);
        }
     </script>
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
                 <table  style="width: 100%; height: 25px;" class="OperationBar">
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
                            <asp:Button ID="btnCheck" runat="server" Text="审核" CssClass="ButtonAudit" onclick="btnCheck_Click"  />
                            <asp:Button ID="btnAdd" runat="server" Text="新增" CssClass="ButtonCreate" OnClientClick="return Add();"  />
                            <asp:Button ID="btnDelete" runat="server" Text="刪除" CssClass="ButtonDel" OnClientClick="return ViewDelete();"
                                onclick="btnDelete_Click"  />
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
                                移库日期
                        </td>
                        <td  width="25%">
                                &nbsp;<asp:TextBox ID="txtBillDate" runat="server"  CssClass="TextRead" 
                                    Width="90%" MaxLength="20" ReadOnly="True" ></asp:TextBox>
                        </td>
                        <td align="center" class="musttitle" style="width:8%;"  >
                                移库单号
                        </td>
                        <td width="25%">
                                &nbsp;<asp:TextBox ID="txtID" 
                                    runat="server"  CssClass="TextRead" Width="90%" 
                                    MaxLength="20" ReadOnly="True" ></asp:TextBox> 
                        </td>
                            <td align="center" class="musttitle" style="width:8%;">
                                库区</td>
                        <td width="26%">
                            &nbsp;<asp:DropDownList ID="ddlAreaCode" runat="server" Width="90%" 
                                    Enabled="False">
                            </asp:DropDownList>
                    
                        </td>
                    </tr>
                  
                   
              
                    <tr style="height:50px">
                        <td align="center" class="smalltitle"  >
                            备注
                        </td>
                        <td colspan="5"  valign="middle" >
                            &nbsp;<asp:TextBox ID="txtMemo" runat="server" CssClass="TextRead" 
                                TextMode="MultiLine" Height="40px" Width="97%" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                 <div id="Sub-container" style="overflow: auto; width: 100%; height: 280px" >
                    <asp:GridView ID="dgViewSub1" runat="server" AutoGenerateColumns="False" SkinID="GridViewSkin"
                        AllowPaging="True" Width="100%" PageSize="10" onrowdatabound="dgViewSub1_RowDataBound" >
                        <Columns>
                            

                             <asp:BoundField DataField="RowID" HeaderText="序号" SortExpression="RowID">
                                <ItemStyle HorizontalAlign="Left" Width="7%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ProductCode" HeaderText="产品编号" SortExpression="ProductCode">
                                <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ProductName" HeaderText="品名" SortExpression="ProductName">
                                <ItemStyle HorizontalAlign="Left" Width="12%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Spec" HeaderText="规格" SortExpression="Spec">
                                <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Propertity" HeaderText="牌号状态" SortExpression="Propertity">
                                <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ModelNo" HeaderText="产品型号" SortExpression="ModelNo">
                                <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="StandardNo" HeaderText="标准号" SortExpression="StandardNo">
                                <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PartNo" HeaderText="部件号" SortExpression="PartNo">
                                <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Barcode" HeaderText="熔次卷号" SortExpression="Barcode">
                                <ItemStyle HorizontalAlign="Left" Width="10%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Quantity" HeaderText="数量" SortExpression="Quantity">
                                <ItemStyle HorizontalAlign="Left" Width="10%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CellCode" HeaderText="货位" SortExpression="CellCode">
                                <ItemStyle HorizontalAlign="Left" Width="10%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NewCellCode" HeaderText="新货位" SortExpression="NewCellCode">
                                <ItemStyle HorizontalAlign="Left" Width="10%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Memo" HeaderText="备注" SortExpression="Memo">
                                <ItemStyle HorizontalAlign="Left" Width="38%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                        </Columns>
                        <PagerSettings Visible="false" />
                    </asp:GridView>
                </div>
                <table  class=" maintable" style="width:100%; height:25px" > 
                    <tr>
                         

                        <td align="center"  style="width:7%;" class="smalltitle">
                            数量统计
                        </td>
                        <td style="width:17%">
                            &nbsp;<asp:TextBox ID="txtTotalQty" runat="server" CssClass="TextRead"  ReadOnly="True" Width="90%" style="text-align:right"></asp:TextBox>
                        </td>
                       
                        <td align="right">
                            
                            <asp:LinkButton ID="btnFirstSub1" runat="server"  
                                        Text="首页" onclick="btnFirstSub1_Click"></asp:LinkButton> 
                                            &nbsp;<asp:LinkButton ID="btnPreSub1" runat="server"  
                                    Text="上一页" onclick="btnPreSub1_Click"></asp:LinkButton> 
                                    &nbsp;<asp:LinkButton ID="btnNextSub1" runat="server" 
                                    Text="下一页" onclick="btnNextSub1_Click"></asp:LinkButton> 
                            &nbsp;<asp:LinkButton ID="btnLastSub1" runat="server"  
                                        Text="尾页" onclick="btnLastSub1_Click"></asp:LinkButton> 
                    &nbsp;<asp:textbox id="txtPageNoSub1" onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))"
					                onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))"
					                runat="server" Width="56px" CssClass="TextBox" >
                                    </asp:textbox>&nbsp;<asp:linkbutton id="btnToPageSub1" runat="server" Text="跳转" onclick="btnToPageSub1_Click"></asp:linkbutton>
                                &nbsp;
                                <asp:Label ID="lblCurrentPageSub1" runat="server" ></asp:Label>&nbsp;&nbsp; 
                            </td>
                        <td width="1%">
                        </td>
                    </tr>
                </table>
                <table  class="maintable"   style=" width:100%; height:25px" align="center" cellspacing="0" cellpadding="0" border="1">
                    <tr>
                        <td align="center"  style="width:7%;" class="smalltitle">
                            审核人员
                        </td>
                        <td style="width:9%">
                            &nbsp;<asp:TextBox ID="txtChecker" runat="server" CssClass="TextRead" ReadOnly="True" Width="90%"></asp:TextBox>
                        </td> 

                         <td align="center"  style="width:7%;" class="smalltitle">
                            审核日期
                        </td>
                        <td style="width:9%">
                            &nbsp;<asp:TextBox ID="txtCheckDate" runat="server" CssClass="TextRead" ReadOnly="True" Width="90%" ></asp:TextBox>
                        </td> 
                        <td align="center"  class="smalltitle" style="width:7%;">
                            建单人员
                        </td> 
                        <td style="width:9%">
                        &nbsp;<asp:TextBox ID="txtCreator" runat="server"  CssClass="TextRead" Width="90%" 
                                ReadOnly="True"  ></asp:TextBox> 
                        </td>
                        <td align="center" class="smalltitle" style="width:7%;">
                            建单日期
                        </td> 
                        <td style="width:9%">
                        &nbsp;<asp:TextBox ID="txtCreatDate" runat="server"  CssClass="TextRead" 
                                Width="90%" ReadOnly="True"  ></asp:TextBox> 
                        </td>
                        <td align="center"  class="smalltitle" style="width:7%;">
                            修改人员
                        </td> 
                        <td style="width:9%">
                            &nbsp;<asp:TextBox ID="txtUpdater" runat="server"  CssClass="TextRead" 
                                Width="90%" ReadOnly="True"  ></asp:TextBox> 
                        </td>
                        <td align="center"  class="smalltitle" style="width:7%;">
                            修改日期
                        </td> 
                        <td style="width:13%">
                        &nbsp;<asp:TextBox ID="txtUpdateDate" runat="server" CssClass="TextRead" 
                                Width="90%" ReadOnly="True"  ></asp:TextBox> 
                        </td>
                </tr>
		    </table>
            <asp:HiddenField ID="hdnState" runat="server" />    
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
