<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProductStateEdit.aspx.cs" Inherits="WebUI_CMD_ProductStateEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
    <title></title>
        <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
        <link href="~/Css/op.css" type="text/css" rel="stylesheet" /> 
        <script type="text/javascript" src="../../JQuery/jquery-1.8.3.min.js"></script>
        <script type="text/javascript" src= "../../JScript/Common.js"></script>
        <script type="text/javascript">
            function Save() {

                if (trim($("#txtID").val()) == "") {
                    alert("状态编码不能为空!");
                    $("#txtID").focus();
                    return false;
                }

                if (trim($("#txtStateName").val()) == "") {
                    alert("状态名称不能为空!");
                    $("#txtFactoryName").focus();
                    return false;
                }

                return true;
            }
           
        </script>
</head>
<body>
    <form id="form2" runat="server">
    <div>
        <table style="width: 100%; height: 25px;" class="OperationBar">
            <tr>
                <td align="right">
                    <asp:Button ID="Button1" runat="server" Text="放弃" 
                        OnClientClick="return Cancel();" CssClass="ButtonCancel" />
                    <asp:Button ID="Button2" runat="server" Text="保存" OnClientClick="return Save()" 
                        CssClass="ButtonSave" onclick="btnSave_Click" Height="16px" />
                    <asp:Button ID="Button3" runat="server" Text="离开" OnClientClick="return Exit();" 
                        CssClass="ButtonExit" />
                </td>
            </tr>
        </table>
        <table id="Table1" class="maintable"  width="100%" align="center" cellspacing="0" cellpadding="0" bordercolor="#ffffff" border="1" runat="server">			
					<tr>
                        <td align="center" class="musttitle" style="width:8%;"  >
                             状态编码
                        </td>
                        <td  style="width:30%;"  >
                             &nbsp;<asp:TextBox ID="txtID" runat="server"   
                                  CssClass="TextBox" Width="42%" MaxLength="10" ></asp:TextBox>
                        </td>
                         <td  align="center" class="musttitle" style="width:8%;" >
                             状态名称
                        </td>
                        <td style="width:30%;">
                         &nbsp;<asp:TextBox 
                                ID="txtStateName" runat="server"  
                                CssClass="TextBox" Width="43%" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    
                    <tr>
                        <td align="center"  class="smalltitle" style="width:8%;">
                            备注
                        </td>
                        <td colspan="3">
                            &nbsp;<asp:TextBox ID="txtMemo" runat="server" CssClass="MultiLineTextBox" 
                                TextMode="MultiLine" Height="102px" Width="75%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                  <td align="center"  class="smalltitle" style="width:8%;">
                        建单人员
                  </td> 
                  <td style="width:30%;">
                    &nbsp;<asp:TextBox ID="txtCreator" runat="server"  
                          CssClass="TextRead" Width="42%"  ></asp:TextBox> 
                  </td>
                  <td align="center" class="smalltitle" style="width:8%;">
                        建单日期
                  </td> 
                  <td style="width:30%;">
                    &nbsp;<asp:TextBox ID="txtCreatDate" runat="server"  
                          CssClass="TextRead" Width="43%"  ></asp:TextBox> 
                  </td>
                </tr>
                <tr>
                  <td align="center"  class="smalltitle" style="width:8%;">
                        修改人员
                  </td> 
                  <td style="width:30%;">
                     &nbsp;<asp:TextBox ID="txtUpdater" runat="server"  
                          CssClass="TextRead" Width="42%" Height="16px"  ></asp:TextBox> 
                  </td>
                  <td align="center"  class="smalltitle" style="width:8%;">
                        修改日期
                  </td> 
                  <td style="width:30%;">
                    &nbsp;<asp:TextBox ID="txtUpdateDate" runat="server"  
                          CssClass="TextRead" Width="43%"  ></asp:TextBox> 
                  </td>
                </tr>				
			</table>
    </div>
    </form>
</body>
</html>

