<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MoveEdit.aspx.cs" Inherits="WebUI_Stock_MoveEdit" %>

<%@ Register src="../../UserControl/Calendar.ascx" tagname="Calendar" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
        <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
        <link href="~/Css/op.css" type="text/css" rel="stylesheet" /> 
        <script type="text/javascript" src="../../JQuery/jquery-1.8.3.min.js"></script>
        <script type="text/javascript" src= "../../JScript/Common.js"></script>
        <script type="text/javascript" src="../../JScript/DataProcess.js"></script>
        <script type="text/javascript">
            $(document).ready(function () {
                $(window).resize(function () {
                    resize();
                });
                BindEvent();
            });
            function resize() {
                var h = document.documentElement.clientHeight - 200;
                $("#Sub-container").css("height", h);
            }


            //            function NewCellClick() {
            //                var txtID = this.id;
            //                var strCellCode = "'-1',";

            //                var ctls = $("[id$='dgViewSub1']").find("[id$='NewCellCode']");
            //                ctls.each(function () {
            //                    if ($(this).val() != "") {
            //                        strCellCode += "'" + $(this).val() + "',";
            //                    }
            //                });
            //                if ($('#hdnNewCellCode').val() != '')
            //                    strCellCode += $('#hdnNewCellCode').val();
            //                strCellCode += "'-1'";

            //                var where = "AreaCode='" + $('#ddlAreaCode').val() + "' and IsLock=0 and ProductCode='' and CellCode not in (" + strCellCode + ") ";
            //                GetOtherValue('CMD_Cell', txtID.replace("btnCellCode", "NewCellCode"), 'CellCode', where);
            //            }

            function BindEvent() {
                $("[ID$='btnCellCode']").bind("click", function () {
                    var txtID = this.id;
                    var strCellCode = "'-1',";

                    var ctls = $("[id$='dgViewSub1']").find("[id$='NewCellCode']");
                    ctls.each(function () {
                        if ($(this).val() != "") {
                            strCellCode += "'" + $(this).val() + "',";
                        }
                    });
                    if ($('#hdnNewCellCode').val() != '')
                        strCellCode += $('#hdnNewCellCode').val();
                    strCellCode += "'-1'";

                    var where = "AreaCode='" + $('#ddlAreaCode').val() + "' and IsLock=0 and ProductCode='' and IsActive=1 and CellCode not in (" + strCellCode + ") ";
                    GetOtherValue('CMD_Cell', txtID.replace("btnCellCode", "NewCellCode"), 'CellCode', where);


                });
                $("[ID$='NewCellCode']").bind("dbclick", function () {
                    var txtID = this.id;
                    $('#' + txtID.replace("NewCellCode", "btnCellCode"))[0].click();

                });
            }

            function AddDetail() {
                var strCellCode = "'-1',";

                var ctls = $("[id$='dgViewSub1']").find("[id$='OldCellCode']");
                ctls.each(function () {
                    if ($(this)[0].innerText != "") {
                        strCellCode += "'" + $(this)[0].innerText + "',";
                    }
                });
                if ($('#hdnOldCellCode').val() != '')
                    strCellCode += $('#hdnOldCellCode').val();
                strCellCode += "'-1'";
                var where = "AreaCode='" + $('#ddlAreaCode').val() + "' and IsLock=0 and ProductCode !='' and CellCode not in (" + strCellCode + ") ";

                return GetMulSelectValue('CMD_Cell', 'hdnMulSelect', where);
            }

            function Save() {
                if ($("#txtBillDate_txtDate").val() == "") {
                    alert("日期不能为空，请输入！");
                    $("#txtBillDate_txtDate").focus();
                    return false;
                }
                if (trim($("#txtID").val()) == "") {
                    alert("移库单号不能为空!");
                    $("#txtID").focus();
                    return false;
                }
                if (trim($("#ddlAreaCode").val()) == "") {
                    alert("库区不能为空!");
                    $("#ddlAreaCode").focus();
                    return false;
                }
                if (!ChkDelMustValue("dgViewSub1", "NewCellCode", "新货位"))
                    return false;

                return true;
            }
           
        </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager2" runat="server"  />  
        <asp:UpdateProgress ID="updateprogress1" runat="server" AssociatedUpdatePanelID="updatePanel1">
            <ProgressTemplate>            
                <div id="progressBackgroundFilter" style="display:none"></div>
                <div id="processMessage"> Loading...<br /><br />
                        <img alt="Loading" src="../../images/loading.gif" />
                </div>            
                </ProgressTemplate>
        </asp:UpdateProgress>  
        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional">                
            <ContentTemplate>
                <table style="width: 100%; height: 25px;" class="OperationBar">
                    <tr>
                        <td align="right">
                            <asp:Button ID="btnCancel" runat="server" Text="放弃" OnClientClick="return Cancel();" CssClass="ButtonCancel"     />   
                            <asp:Button ID="btnSave" runat="server" Text="保存" OnClientClick="return Save();" 
                                CssClass="ButtonSave" onclick="btnSave_Click" Height="16px" />
                            <asp:Button ID="btnExit" runat="server" Text="离开" OnClientClick="return Exit();" CssClass="ButtonExit"/>
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
                                &nbsp;<uc1:Calendar ID="txtBillDate" runat="server"  /></td>
                        <td align="center" class="musttitle" style="width:8%;"  >
                                移库单号
                        </td>
                        <td width="25%">
                                &nbsp;<asp:TextBox ID="txtID" 
                                    runat="server"  CssClass="TextBox" Width="90%" 
                                    MaxLength="20" ></asp:TextBox> 
                        </td>
                            <td align="center" class="musttitle" style="width:8%;"  >
                                库区</td>
                        <td  width="25%">
                                &nbsp;<asp:DropDownList ID="ddlAreaCode" runat="server" Width="90%">
                            </asp:DropDownList>
                        </td>
                    </tr>
                   
              
                    <tr style="height:50px">
                        <td align="center" class="smalltitle"  >
                            备注
                        </td>
                        <td colspan="5"  valign="middle" >
                            &nbsp;<asp:TextBox ID="txtMemo" runat="server" CssClass="MultiLineTextBox" 
                                TextMode="MultiLine" Height="45px" Width="97%"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table style="width:100%; height:25px">
                    <tr>
                        <td class="table_titlebgcolor" height="25px">
                            <asp:Button  id="btnAddDetail" CssClass="ButtonCreate" runat="server" 
                                Text="新增明细" OnClientClick="return AddDetail();" onclick="btnAddDetail_Click"  Width="75px" Height="16px"  />  
                                &nbsp;&nbsp;
                                <asp:Button  id="btnDelDetail" CssClass=" ButtonDel" 
                                runat="server" Text="删除明细" onclick="btnDelDetail_Click" 
                                Width="75px" Height="16px" />
                               
                    </td>
                    </tr>
                </table> 
                <div id="Sub-container" style="overflow: auto; width: 100%; height: 280px" >
                    <asp:GridView ID="dgViewSub1" runat="server" AutoGenerateColumns="False" SkinID="GridViewSkin"
                        AllowPaging="True" Width="100%" PageSize="10" onrowdatabound="dgViewSub1_RowDataBound" >
                        <Columns>
                            <asp:TemplateField  >
                                <HeaderTemplate>
                                <input type="checkbox" onclick="selectAll('dgViewSub1',this.checked);" />                    
                                </HeaderTemplate>
                                <ItemTemplate>
                                <asp:CheckBox id="cbSelect" runat="server" ></asp:CheckBox>                   
                                </ItemTemplate>
                                <HeaderStyle Width="3%"></HeaderStyle>
                                <ItemStyle Width="3%" HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="序号">
                                <ItemTemplate>
                                    <asp:Label ID="RowID" runat="server" Text=""></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle Width="4%"  />
                            </asp:TemplateField>
                             <asp:BoundField DataField="ProductCode" HeaderText="产品编号" SortExpression="ProductCode">
                                <ItemStyle HorizontalAlign="Left" Width="10%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ProductName" HeaderText="品名" SortExpression="ProductName">
                                <ItemStyle HorizontalAlign="Left" Width="10%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Quantity" HeaderText="数量" SortExpression="Quantity">
                                <ItemStyle HorizontalAlign="Left" Width="10%" Wrap="False" />
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
                                <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                             <asp:TemplateField HeaderText="货位">
                                <ItemTemplate>
                                    <asp:Label ID="OldCellCode" runat="server" Text=""></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle Width="8%"  />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="新货位">
                               <ItemTemplate>
                                    <asp:TextBox ID="NewCellCode" runat="server"  Width="80%" CssClass="TextBox"></asp:TextBox><asp:Button
                                        ID="btnCellCode"  CssClass="ButtonOption" Width="20px" runat="server"  Text="..."  />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderStyle Width="15%"  />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="备注">
                                <ItemTemplate>
                                    <asp:TextBox ID="SubMemo" runat="server" Width="98%"  CssClass="TextBox" ></asp:TextBox> 
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderStyle  Width="12%" />
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings Visible="false" />
                    </asp:GridView>
                </div>
                <table  class=" maintable" style="width:100%; height:25px" > 
                    <tr>
                        <td align="center"  style="width:8%;" class="smalltitle">
                            数量合计
                        </td>
                        <td style="width:17%">
                            &nbsp;<asp:TextBox ID="txtTotalQty" runat="server" CssClass="TextRead" Height="16px" ReadOnly="True" Width="90%" style="text-align:right"></asp:TextBox>
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
                        <td align="center"  class="smalltitle" style="width:8%;">
                            建单人员
                        </td> 
                        <td style="width:17%">
                        &nbsp;<asp:TextBox ID="txtCreator" runat="server"  CssClass="TextRead" Width="90%"  ></asp:TextBox> 
                        </td>
                        <td align="center" class="smalltitle" style="width:8%;">
                            建单日期
                        </td> 
                        <td style="width:17%">
                        &nbsp;<asp:TextBox ID="txtCreatDate" runat="server"  CssClass="TextRead" Width="90%"  ></asp:TextBox> 
                        </td>
                        <td align="center"  class="smalltitle" style="width:8%;">
                            修改人员
                        </td> 
                        <td style="width:17%">
                            &nbsp;<asp:TextBox ID="txtUpdater" runat="server"  CssClass="TextRead" Width="90%"  ></asp:TextBox> 
                        </td>
                        <td align="center"  class="smalltitle" style="width:8%;">
                            修改日期
                        </td> 
                        <td style="width:17%">
                        &nbsp;<asp:TextBox ID="txtUpdateDate" runat="server" CssClass="TextRead" Width="90%"  ></asp:TextBox> 
                        </td>
                </tr>
		    </table>

            <input type="hidden" runat="server" id="hdnMulSelect" /> 
            <input type="hidden" runat="server" id="hdnOldCellCode" /> 
            <input type="hidden" runat="server" id="hdnNewCellCode" /> 
              
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
