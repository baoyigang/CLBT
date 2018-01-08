<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WarehouseCellEditPage.aspx.cs" Inherits="WebUI_CMD_WarehouseCellEditPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>货架货位</title>
    <base target="_self" />
   <link href="../../css/main.css" rel="Stylesheet" type="text/css" />
    <link href="../../css/op.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript" src="../../JQuery/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src= "../../JScript/Common.js"></script>
    <script type="text/javascript">
        function RefreshParent(path) {
            alert('货位删除成功！');
            window.parent.document.getElementById('hdnRemovePath').value = path;
            window.parent.document.getElementById('btnRemoveNode').click();
        }

        function UpdateParent() {
            alert('货位修改成功！');
            //window.parent.document.getElementById('btnUpdateSelected').click();
        }

        function ReloadParent() {
            alert('货位添加成功！');
            window.parent.document.getElementById('btnReload').click();
        }
        function openwin() {
            window.open("BatchAssignedProduct.aspx", "", "height=410px, width=600px,top=200px,left=300px, toolbar=no, menubar=no, scrollbars=no, resizable=no, location=no, status=no")
        }
        function CheckBeforeSubmit() {
            var cellcode = document.getElementById('txtCellCode').value;
            var cellname = document.getElementById('txtCellName').value;

            if (cellcode == "") {
                alert('货位编码不能为空！');
                return false;
            }
            if (cellname == "") {
                alert('货位名称不能为空！');
                return false;
            }

        }

        function clear(id) {
            alert(id)
            document.getElementById(id).value = "";
        }
    </script>
     
    <style type="text/css">
        .MultilineTextBox
        {}
    </style>
     
</head>
<body >
    <form id="form1" runat="server">
          
       <table width="99%" class="maintable" align="center" cellspacing="0" cellpadding="0" bordercolor="#ffffff" border="1">
            <tr style="display:none;">
            <td colspan="4" style=" height:30px">
                <asp:TextBox ID="txtCELLID" runat="server"  CssClass="HiddenControl"></asp:TextBox>
                <asp:TextBox ID="txtAreaID" runat="server"  CssClass="HiddenControl"></asp:TextBox>
                <asp:TextBox ID="txtShelfID" runat="server"  CssClass="HiddenControl"></asp:TextBox>
                <input class="ButtonCreate" name="btnBack" onclick="openwin()" type="button" value="批量分配指定卷烟" />
            </td>
            </tr>
            <tr><td colspan="4">货位</td></tr>
            <tr>                        
                <td align="center" class="musttitle" style="width:15%;">库区名称</td>
                <td style="width:35%;">
                    <asp:DropDownList ID="ddlAreaCode" runat="server" Width="80%">
                        </asp:DropDownList>
                        
                </td> 
                <td align="center" class="musttitle" style="width:15%;">货架名称</td>
                <td   style="width:35%;"><asp:TextBox ID="txtShelfName" runat="server" CssClass="TextRead" Width="80%" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td  align="center" class="musttitle" style="width:15%;">货位编码</td> 
                <td ><asp:TextBox ID="txtCellCode" runat="server"  CssClass="TextRead"  Width="80%" ></asp:TextBox>
                </td>
                <td  align="center" class="musttitle" style="width:15%;">货位名称</td> 
                <td style="width:35%;">
                    <asp:TextBox ID="txtCellName" runat="server"  CssClass="TextRead"  Width="80%"></asp:TextBox>
                </td>
            </tr>
                      
            <tr>
                <td align="center" class="musttitle" style="width:15%;">货位层数</td> 
                <td style="width:35%;"><asp:TextBox ID="txtCellRows" runat="server"  CssClass="TextRead" Width="80%">1</asp:TextBox>
                </td>
                <td align="center" class="musttitle" style="width:15%;">货位列数</td> 
                <td><asp:TextBox ID="txtCellCols" runat="server"  CssClass="TextRead" Width="80%"></asp:TextBox>
                </td>
            </tr> 
            <tr>
                <td align="center" class="musttitle" style="width:15%;">
                    是否锁定</td> 
                <td style="width:35%;"><asp:DropDownList ID="ddlLock" runat="server" Height="16px" 
                        Width="80%" Enabled="False">
                        <asp:ListItem Selected="True" Value="1">锁定</asp:ListItem>
                        <asp:ListItem Value="0">解锁</asp:ListItem>
                    </asp:DropDownList></td>
                <td align="center" class="musttitle" style="width:15%;">
                    是否启用</td> 
                <td style="width:35%;">
                    <asp:DropDownList ID="ddlActive" runat="server" Height="16px" Width="80%">
                        <asp:ListItem Selected="True" Value="1">启用</asp:ListItem>
                        <asp:ListItem Value="0">未启用</asp:ListItem>
                    </asp:DropDownList>
                 </td>
            </tr> 
            <tr>
               <td align="center" class="musttitle" style="width:15%;">
                   设定单元</td>
               <td colspan="3">
                    <asp:RadioButton ID="rpt1" runat="server" Checked="True" GroupName="Rpt" Text="单个货位" />&nbsp;
                    <asp:RadioButton ID="rpt2" runat="server" GroupName="Rpt" Text="货位所在货架层的所有货位" />&nbsp;  
                    <asp:RadioButton ID="rpt3" runat="server" GroupName="Rpt" Text="货位所在货架列的所有货位" />&nbsp;  
               </td> 
            </tr>
            <tr>
                <td align="center" class=" smalltitle" style="width:15%;">备注</td> 
                <td colspan="3">
                    <asp:TextBox ID="txtMemo" runat="server"  
                        CssClass="MultilineTextBox" Width="92%" Rows="10" TextMode="MultiLine" 
                        Height="71px"></asp:TextBox>
                </td>
            </tr> 
    
            <tr><td colspan="4" align="center"  style="height:35px; text-align:center;">
                <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="button" OnClick="btnSave_Click" OnClientClick="return CheckBeforeSubmit()"/>
                </td></tr>                                                                                                                                                                      
        </table>  
          
    </form>

</body>
</html>
