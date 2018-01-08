<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WarehouseShelfEditPage.aspx.cs" Inherits="WebUI_CMD_WarehouseShelfEditPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>库区货架</title>
    <base target="_self" />
    <link href="../../css/main.css" rel="Stylesheet" type="text/css" />
    <link href="../../css/op.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript" src="../../JQuery/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src= "../../JScript/Common.js"></script>
   
   
    <script type="text/javascript">
        function RefreshParent(path) {
            alert('货架删除成功！');
            window.parent.document.getElementById('hdnRemovePath').value = path;
            window.parent.document.getElementById('btnRemoveNode').click();
        }
        function UpdateParent() {
            alert('货架修改成功！');
            //window.parent.document.getElementById('btnUpdateSelected').click();
        }
        function ReloadParent() {
            alert('货架添加成功！');
            window.parent.document.getElementById('btnReload').click();
        }
        function openwin() {
            window.open("BatchAssignedProduct.aspx", "", "height=410, width=600,top=200px,left=300px, toolbar=no, menubar=no, scrollbars=no, resizable=no, location=no, status=no")
        }

        function CheckBeforeSubmit() {
            var shelfcode = document.getElementById('txtShelfCode').value;
            var shelfname = document.getElementById('txtShelfName').value; //document.getElementById('txtTitle').value.trim();
            var cols = document.getElementById('txtCellCols').value;
            var rows = document.getElementById('txtCellRows').value;

            if (shelfcode == "") {
                alert('货架编码不能为空！');
                return false;
            }
            if (shelfcode.length != 6) {
                alert('货架编码为6码！');
                return false;
            }
            if (shelfname == "") {
                alert('货架名称不能为空！');
                return false;
            }



        }
      

    </script>

    

</head>
<body>
    <form id="form1" runat="server">
        <table width="99%" class="maintable" align="center" cellspacing="0" cellpadding="0" bordercolor="#ffffff" border="1">
            <tr><td colspan="4">货架</td></tr>
            <tr style="display:none;"><td colspan="4">
            <asp:TextBox ID="txtShelfID" runat="server" CssClass="HiddenControl" Width="61px"></asp:TextBox>
                <asp:TextBox ID="txtWHID" runat="server" CssClass="HiddenControl" Width="61px"></asp:TextBox>
                <asp:TextBox ID="txtAreaID" runat="server" CssClass="HiddenControl" 
                    Width="61px"></asp:TextBox>
                <input class="ButtonCreate" name="btnBack" onclick="openwin()" type="button" value="批量分配指定卷烟" /></td>
            </tr>
            <tr>
                <td align="center" class="musttitle" style="width:15%;">仓库名称</td>
                         
                <td style="width:35%;">
                    <asp:TextBox ID="txtWhName" runat="server" CssClass="TextRead" Width="80%"></asp:TextBox></td>
                <td align="center" class="musttitle" style="width:15%;">货架编码</td>
                <td style="width:35%;"><asp:TextBox ID="txtShelfCode" runat="server"  CssClass="TextRead" Width="80%" 
                        MaxLength="10"></asp:TextBox>
                </td>
                         
            </tr>
            <tr>
                <td align="center" class="musttitle" style="width:15%;">货架名称</td> 
                <td><asp:TextBox ID="txtShelfName" runat="server"  CssClass="TextBox" Width="80%" MaxLength="10"></asp:TextBox>
                </td>
                <td align="center" class=" smalltitle" style="width:15%;">货架层数</td> 
                <td><asp:TextBox ID="txtCellRows" runat="server"  CssClass="TextBox" Width="80%">1</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="center" class="smalltitle" style="width:15%;">是否启用</td> 
                <td>
                    <asp:DropDownList ID="ddlActive" runat="server" Width="80%">
                        <asp:ListItem Selected="True" Value="1">启用</asp:ListItem>
                        <asp:ListItem Value="0">未启用</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td align="center" class="smalltitle" style="width:15%;">货架列数</td> 
                <td><asp:TextBox ID="txtCellCols" runat="server"  CssClass="TextBox" Width="80%" >1</asp:TextBox>
                </td>
            </tr>  
                      
            
            <tr>
                <td align="center" class="smalltitle" style="width:15%;">备注</td> 
                <td colspan="3" style="text-align: left; height:75px">
                    <asp:TextBox ID="txtMemo" runat="server"  
                        CssClass="TextBox" Width="92%" Rows="10" TextMode="MultiLine" 
                        Height="65px"></asp:TextBox>
                </td>
            </tr>        
            <tr><td colspan="4" align="center"  style="height:35px; text-align:center;">
                <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="ButtonCss" 
                    OnClick="btnSave_Click" OnClientClick="return CheckBeforeSubmit()" 
                    Height="26px" Width="57px"/>
                </td>
                </tr>                                                                                
        </table>  
    </form>
    
</body>
</html>
