<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProductView.aspx.cs" Inherits="WebUI_CMD_ProductView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
    <link href="~/Css/op.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../../JQuery/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src= "../../JScript/Common.js"></script> 
    <script type="text/javascript">
         
        function ChangeNo() {
            var strID = $("#txtID").val();
            window.location = "ProductChangeNo.aspx?SubModuleCode=" + '<%=SubModuleCode%>' + "&FormID=" + '<%=FormID%>' + "&SqlCmd=" + '<%=SqlCmd%>' + "&ID=" + strID;
            return false;
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
                            <asp:Button ID="btnAdd" runat="server" Text="新增" CssClass="ButtonCreate" OnClientClick="return Add();"  />
                            <asp:Button ID="btnDelete" runat="server" Text="刪除" CssClass="ButtonDel" OnClientClick="return ViewDelete();"
                                onclick="btnDelete_Click"  />
                            <asp:Button ID="btnEdit" runat="server" Text="修改" CssClass="ButtonModify" 
                                OnClientClick="return ViewEdit();" />
                            <asp:Button ID="btnChange" runat="server" CssClass="ButtonModify"
                                Text="批次变更编号"  OnClientClick="return ChangeNo();" />

                            
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
                    <td align="center" class="musttitle" style="width:12%;">
                        产品类别
                    </td>
                    <td width="21%">
                        &nbsp;<asp:DropDownList ID="ddlCategoryCode" Enabled="false" runat="server" 
                            Width="91%"></asp:DropDownList>
                    </td>
                    <td align="center" class="musttitle" style="width:12%;"  >
                            产品编号
                    </td>
                    <td  width="21%">
                            &nbsp;<asp:TextBox ID="txtID" runat="server"  CssClass="TextRead" Width="90%" MaxLength="20" ReadOnly="True"></asp:TextBox>
                    </td>
                    <td align="center" class="musttitle" style="width:12%;"  >
                           品名
                    </td>
                    <td width="21%">
                            &nbsp;<asp:TextBox ID="txtProductName" CssClass="TextRead" runat="server"   Width="90%" MaxLength="50" ReadOnly="True"></asp:TextBox> 
                          

                    </td>
                    
                </tr>
                <tr>
                    
                    <td align="center" class="musttitle" style="width:12%;"  >
                           英文品名 
                    </td>
                    <td width="21%">
                            &nbsp;<asp:TextBox ID="txtProductEName" runat="server"    Width="90%" 
                                CssClass="TextRead" MaxLength="20" ReadOnly="True"></asp:TextBox> 

                    </td>
                    <td align="center" class="musttitle" style="width:12%;">
                        型号</td>
                    <td width="21%">
                        &nbsp;<asp:TextBox ID="txtModelNo" runat="server" CssClass="TextRead"   
                            Width="90%" MaxLength="25" ReadOnly="True"></asp:TextBox>
                    </td>
                    <td   align="center" class="smalltitle">
                        规格
                    </td>
                    <td >
                        &nbsp;<asp:TextBox ID="txtSpec" runat="server" CssClass="TextRead" MaxLength="25" 
                            ReadOnly="True" Width="90%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="center" class="musttitle" style="width:12%;">
                        供应商</td>
                    <td width="21%">
                        &nbsp;<asp:DropDownList ID="ddlFactory" runat="server" Enabled="false" 
                            Width="91%"></asp:DropDownList>

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                            产品条码</td>
                    <td  width="21%">
                        &nbsp;<asp:TextBox ID="txtBarcode" runat="server"   Width="90%" 
                            CssClass="TextRead" MaxLength="50" ReadOnly="True"></asp:TextBox> 

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                           产品属性</td>
                    <td width="21%">
                           &nbsp;<asp:TextBox ID="txtPropertity" runat="server"  CssClass="TextRead" 
                               Width="90%"  ReadOnly="True"></asp:TextBox>
                    </td>
                    
                </tr>
                <tr>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                            计量单位
                    </td>
                    <td  width="21%">
                        &nbsp;<asp:TextBox ID="txtUnit" runat="server"   Width="90%" 
                            CssClass="TextRead" MaxLength="25" ReadOnly="True"></asp:TextBox> 

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                           长度 
                    </td>
                    <td width="21%">
                            &nbsp;<asp:TextBox ID="txtLength" runat="server"  Width="90%" 
                                CssClass="TextRead" MaxLength="25" ReadOnly="True"></asp:TextBox> 

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;">
                        宽度</td>
                    <td width="21%">
                        &nbsp;<asp:TextBox ID="txtWidth" runat="server"  Width="90%" 
                            CssClass="TextRead" MaxLength="25" ReadOnly="True"></asp:TextBox> 

                    </td>
                </tr>
                 <tr>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                            高度
                    </td>
                    <td  width="21%">
                        &nbsp;<asp:TextBox ID="txtHeight" runat="server"  Width="90%" 
                            CssClass="TextRead" MaxLength="25" ReadOnly="True"></asp:TextBox> 

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                           重量</td>
                    <td width="21%">
                            &nbsp;<asp:TextBox ID="txtWeight" runat="server"   Width="90%" 
                                CssClass="TextRead"  MaxLength="25" ReadOnly="True"></asp:TextBox> 

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;">
                        材质</td>
                    <td width="21%">
                        &nbsp;<asp:TextBox ID="txtMaterial" runat="server"   Width="90%" 
                            CssClass="TextRead" MaxLength="25" ReadOnly="True"></asp:TextBox> 

                    </td>
                </tr>
                 <tr>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                            颜色</td>
                    <td  width="21%">
                        &nbsp;<asp:TextBox ID="txtColor" runat="server"  Width="90%" 
                            CssClass="TextRead" MaxLength="20" ReadOnly="True"></asp:TextBox> 

                    </td>
                   
                    <td align="center" class="smalltitle" style="width:12%;">
                        标准号</td>
                    <td width="21%">
                        &nbsp;<asp:TextBox ID="txtStandardNo" runat="server"   Width="90%" 
                            CssClass="TextRead" MaxLength="25" ReadOnly="True"></asp:TextBox> 

                    </td>
                     <td align="center" class="smalltitle" style="width:12%;">
                        部品号</td>
                    <td width="21%">
                        &nbsp;<asp:TextBox ID="txtPartNo" runat="server"   Width="90%" 
                            CssClass="TextRead" MaxLength="25" ReadOnly="True"></asp:TextBox> 

                    </td>

                </tr>
                 
                 
                 <tr>
                  
                   
                 
                   <td align="center"  class="smalltitle" style="width:12%;">
                       描述</td>
                  <td>
                      &nbsp;<asp:TextBox ID="txtDescription" runat="server"  CssClass="TextRead" 
                          Width="90%" ReadOnly="True"  ></asp:TextBox> 
                  </td>
                   <td align="center" class="smalltitle" style="width:12%;"  >
                           有效期 
                    </td>
                    <td width="21%">
                            &nbsp;<asp:TextBox ID="txtValidPeriod" runat="server"  Width="90%" 
                                CssClass="TextRead" MaxLength="25" ReadOnly="True"></asp:TextBox> 

                    </td>
                   
                  <td align="center"  class="smalltitle" style="width:12%;">
                        建单人员
                  </td> 
                  <td>
                    &nbsp;<asp:TextBox ID="txtCreator" runat="server"  CssClass="TextRead" Width="90%" ReadOnly="True" ></asp:TextBox> 
                  </td>
                </tr>
                <tr>
                   <td align="center" class="smalltitle" style="width:12%;">
                        建单日期
                  </td> 
                  <td>
                    &nbsp;<asp:TextBox ID="txtCreatDate" runat="server"  CssClass="TextRead" Width="90%" ReadOnly="True" ></asp:TextBox> 
                  </td>
                  <td align="center"  class="smalltitle" style="width:12%;">
                        修改人员
                  </td> 
                  <td>
                     &nbsp;<asp:TextBox ID="txtUpdater" runat="server"  CssClass="TextRead" Width="90%"  ReadOnly="True"></asp:TextBox> 
                  </td>
                  <td align="center"  class="smalltitle" style="width:12%;">
                        修改日期
                  </td> 
                  <td>
                    &nbsp;<asp:TextBox ID="txtUpdateDate" runat="server"  CssClass="TextRead" Width="90%" ReadOnly="True" ></asp:TextBox> 
                  </td>
                  
                </tr>	
                <tr>
                    <td align="center"  class="smalltitle" style="width:12%;">
                        备注
                    </td>
                    <td colspan="5">
                        &nbsp;<asp:TextBox ID="txtMemo" runat="server" CssClass="TextRead" 
                            TextMode="MultiLine" Height="72px" Width="97%" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                	
		</table>
                
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
