<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProductEdit.aspx.cs" Inherits="WebUI_CMD_ProductEdit" %>
<%@ Register src="../../UserControl/Calendar.ascx" tagname="Calendar" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
        <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
        <link href="~/Css/op.css" type="text/css" rel="stylesheet" /> 
        <script type="text/javascript" src="../../JQuery/jquery-1.8.3.min.js"></script>
        <script type="text/javascript" src= "../../JScript/Common.js"></script>
         <script type="text/javascript" src= "../../JScript/DataProcess.js"></script>
        <script type="text/javascript">


            function Save() {
                if (trim($("#txtID").val()) == "") {
                    alert("产品编号不能为空!");
                    $("#txtID").focus();
                    return false;
                }
                if (trim($("#txtProductName").val()) == "") {
                    alert("品名不能为空!");
                    $("#txtProductName").focus();
                    return false;
                }
                if (trim($("#ddlCategoryCode").val()) == "") {
                    alert("产品类型不能为空!");
                    $("#ddlCategoryCode").focus();
                    return false;
                }
                 

                return true;
            }
           
        </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table  style="width: 100%; height: 25px;" class="OperationBar">
            <tr>
                <td align="right">
                    <asp:Button ID="btnCancel" runat="server" Text="放弃" 
                        OnClientClick="return Cancel();" CssClass="ButtonCancel" />
                    <asp:Button ID="btnSave" runat="server" Text="保存" OnClientClick="return Save()" 
                        CssClass="ButtonSave" onclick="btnSave_Click"  />
                    <asp:Button ID="btnExit" runat="server" Text="离开" OnClientClick="return Exit();" 
                        CssClass="ButtonExit" />
                </td>
            </tr>
        </table>
        <table id="Table1" class="maintable"  width="100%" align="center" cellspacing="0" cellpadding="0" bordercolor="#ffffff" border="1"
				 runat="server">			
				<tr>
                    <td align="center" class="musttitle" style="width:12%;">
                        产品类别</td>
                    <td width="21%">
                        &nbsp;<asp:DropDownList 
                            ID="ddlCategoryCode" runat="server" Width="91%">
                        </asp:DropDownList>
                    </td>
                    <td align="center" class="musttitle" style="width:12%;"  >
                            产品编号
                    </td>
                    <td  width="21%">
                            &nbsp;<asp:TextBox 
                                ID="txtID" runat="server"  CssClass="TextBox" Width="90%" 
                                MaxLength="20"  ></asp:TextBox>
                    </td>
                    <td align="center" class="musttitle" style="width:12%;"  >
                           品名
                    </td>
                    <td width="21%">
                            &nbsp;<asp:TextBox 
                                ID="txtProductName" runat="server"  CssClass="TextBox" Width="90%" 
                                MaxLength="50" ></asp:TextBox> 
                          

                    </td>
                    
                </tr>
                <tr>
                    
                    <td align="center" class="musttitle" style="width:12%;"  >
                           英文品名 
                    </td>
                    <td width="21%">
                            &nbsp;<asp:TextBox ID="txtProductEName" 
                                runat="server"    Width="90%" 
                                CssClass="TextBox" MaxLength="20"></asp:TextBox> 

                    </td>
                    <td align="center" class="musttitle" style="width:12%;">
                        型号</td>
                    <td width="21%">
                        &nbsp;<asp:TextBox ID="txtModelNo" runat="server" CssClass="TextBox"   
                            Width="90%" MaxLength="25"></asp:TextBox>
                    </td>
                    <td   align="center" class="smalltitle">
                        规格
                    </td>
                    <td >
                        &nbsp;<asp:TextBox ID="txtSpec" runat="server" 
                            CssClass="TextBox" MaxLength="25" Width="90%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="center" class="musttitle" style="width:12%;">
                        供应商</td>
                    <td width="21%">
                        &nbsp;<asp:DropDownList ID="ddlFactory" runat="server" 
                            Width="91%"></asp:DropDownList>

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                            产品条码</td>
                    <td  width="21%">
                        &nbsp;<asp:TextBox ID="txtBarcode" runat="server"   Width="90%" 
                            CssClass="TextBox" MaxLength="50"></asp:TextBox> 

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                           产品属性</td>
                    <td width="21%">
                           &nbsp;<asp:TextBox ID="txtPropertity" 
                               runat="server"  CssClass="TextBox" 
                               Width="90%"></asp:TextBox>
                    </td>
                    
                </tr>
                <tr>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                            计量单位
                    </td>
                    <td  width="21%">
                        &nbsp;<asp:TextBox ID="txtUnit" runat="server"   Width="90%" 
                            CssClass="TextBox" MaxLength="25"></asp:TextBox> 

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                           长度 
                    </td>
                    <td width="21%">
                            &nbsp;<asp:TextBox ID="txtLength" runat="server"  Width="90%" 
                                CssClass="TextBox" MaxLength="25"></asp:TextBox> 

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;">
                        宽度</td>
                    <td width="21%">
                        &nbsp;<asp:TextBox ID="txtWidth" runat="server"  Width="90%" 
                            CssClass="TextBox" MaxLength="25"></asp:TextBox> 

                    </td>
                </tr>
                 <tr>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                            高度
                    </td>
                    <td  width="21%">
                        &nbsp;<asp:TextBox ID="txtHeight" runat="server"  Width="90%" 
                            CssClass="TextBox" MaxLength="25"></asp:TextBox> 

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                           重量</td>
                    <td width="21%">
                            &nbsp;<asp:TextBox ID="txtWeight" 
                                runat="server"   Width="90%" 
                                CssClass="TextBox"  MaxLength="25"></asp:TextBox> 

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;">
                        材质</td>
                    <td width="21%">
                        &nbsp;<asp:TextBox ID="txtMaterial" runat="server"   Width="90%" 
                            CssClass="TextBox" MaxLength="25"></asp:TextBox> 

                    </td>
                </tr>
                 <tr>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                            颜色</td>
                    <td  width="21%">
                        &nbsp;<asp:TextBox ID="txtColor" runat="server"  Width="90%" 
                            CssClass="TextBox" MaxLength="20"></asp:TextBox> 

                    </td>
                    <td  align="center" class="smalltitle" style="width:12%;" >
                        标准号</td>
                    <td>
                        &nbsp;<asp:TextBox ID="txtStandardNo" 
                                runat="server"   Width="90%" 
                                CssClass="TextBox"  MaxLength="25"></asp:TextBox> 

                    </td>
                   <td  align="center" class="smalltitle" style="width:12%;" >
                       部品号</td>
                    <td>
                       &nbsp;<asp:TextBox ID="txtPartNo" 
                                runat="server"   Width="90%" 
                                CssClass="TextBox"  MaxLength="25"></asp:TextBox> 

                     </td>
                </tr>
                 
                 
                 <tr>
                  
                   
                 
                  <td align="center"  class="smalltitle" style="width:12%;">
                       描述</td>
                  <td>
                      &nbsp;<asp:TextBox ID="txtDescription" runat="server"  CssClass="TextBox" 
                          Width="90%"  ></asp:TextBox> 
                  </td>

                  <td align="center" class="smalltitle" style="width:12%;"  >
                           有效期 
                    </td>
                    <td width="21%">
                            &nbsp;<asp:TextBox ID="txtValidPeriod" 
                                runat="server"  Width="90%" 
                                CssClass="TextBox" MaxLength="25"></asp:TextBox> 

                    </td>
                   
                  <td align="center"  class="smalltitle" style="width:12%;">
                        建单人员
                  </td> 
                  <td>
                    &nbsp;<asp:TextBox ID="txtCreator" runat="server"  
                          CssClass="TextRead" Width="90%" ></asp:TextBox> 
                  </td>
                </tr>
                <tr>
                   <td align="center" class="smalltitle" style="width:12%;">
                        建单日期
                  </td> 
                  <td>
                    &nbsp;<asp:TextBox ID="txtCreatDate" runat="server"  
                          CssClass="TextRead" Width="90%" ></asp:TextBox> 
                  </td>
                  <td align="center"  class="smalltitle" style="width:12%;">
                        修改人员
                  </td> 
                  <td>
                     &nbsp;<asp:TextBox ID="txtUpdater" runat="server"  
                          CssClass="TextRead" Width="90%"></asp:TextBox> 
                  </td>
                  <td align="center"  class="smalltitle" style="width:12%;">
                        修改日期
                  </td> 
                  <td>
                    &nbsp;<asp:TextBox ID="txtUpdateDate" runat="server"  
                          CssClass="TextRead" Width="90%" ></asp:TextBox> 
                  </td>
                  
                </tr>	
                <tr>
                    <td align="center"  class="smalltitle" style="width:12%;">
                        备注
                    </td>
                    <td colspan="5">
                        &nbsp;<asp:TextBox 
                            ID="txtMemo" runat="server" CssClass="TextBox" 
                            TextMode="MultiLine" Height="72px" Width="97%"></asp:TextBox>
                    </td>
                </tr>
                	
		</table>
    </div>
    </form>
</body>
</html>
