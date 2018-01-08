<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CraneEdit.aspx.cs" Inherits="WebUI_CMD_CraneEdit" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
        <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
        <link href="~/Css/op.css" type="text/css" rel="stylesheet" /> 
        <script type="text/javascript" src="../../JQuery/jquery-1.8.3.min.js"></script>
        <script type="text/javascript" src= "../../JScript/Common.js"></script>
        <script type="text/javascript">
            function Save() {

                if (trim($("#txtID").val()) == "") {
                    alert("堆垛机编码不能为空!");
                    $("#txtID").focus();
                    return false;
                }

                if (trim($("#txtTypeName").val()) == "") {
                    alert("堆垛机名称不能为空!");
                    $("#txtTypeName").focus();
                    return false;
                }

                return true;
            }
           
        </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table   style="width: 100%; height: 25px;" class="OperationBar">
            <tr>
                <td align="right">
                    <asp:Button ID="btnCancel" runat="server" Text="放弃" 
                        OnClientClick="return Cancel();" CssClass="ButtonCancel" />
                    <asp:Button ID="btnSave" runat="server" Text="保存" OnClientClick="return Save()" 
                        CssClass="ButtonSave" onclick="btnSave_Click" Height="16px" />
                    <asp:Button ID="btnExit" runat="server" Text="离开" OnClientClick="return Exit();" 
                        CssClass="ButtonExit" />
                </td>
            </tr>
        </table>
        <table id="Table1" class="maintable"  width="100%" align="center" cellspacing="0" cellpadding="0" bordercolor="#ffffff" border="1" 
				 runat="server">			
				<tr>
                    <td align="center" class="musttitle" style="width:8%;"  >
                            堆垛机编码
                    </td>
                    <td  width="25%">
                            &nbsp;<asp:TextBox 
                                ID="txtID" runat="server"  CssClass="TextBox" Width="90%" 
                                MaxLength="10"  ></asp:TextBox>
                    </td>
                    <td align="center" class="musttitle" style="width:8%;"  >
                           堆垛机名称
                    </td>
                    <td width="25%">
                            &nbsp;<asp:TextBox ID="txtTypeName" 
                                runat="server"  CssClass="TextBox" Width="90%" MaxLength="30" ></asp:TextBox> 
                    </td>
                     <td align="center" class="musttitle" style="width:8%;"  >
                                状态
                        </td>
                        <td width="26%">
                                &nbsp;<asp:DropDownList ID="ddlActive" runat="server"  Width="72%" 
                                  >
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
                        &nbsp;<asp:TextBox ID="txtMemo" runat="server" CssClass="MultiLineTextBox" 
                            TextMode="MultiLine" Height="102px" Width="92%"></asp:TextBox>
                    </td>
                </tr>
                
		</table>
    </div>
    </form>
</body>
</html>
