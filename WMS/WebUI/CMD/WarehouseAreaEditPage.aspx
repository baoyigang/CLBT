<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WarehouseAreaEditPage.aspx.cs" Inherits="WebUI_CMD_WarehouseAreaEditPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>库区</title>
    <base target="_self" />
    <link href="../../css/main.css" rel="Stylesheet" type="text/css" />
    <link href="../../css/op.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript" src="../../JQuery/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src= "../../JScript/Common.js"></script>
    <script type="text/javascript">
        function RefreshParent(path) {
            alert('库区删除成功！');
            window.parent.document.getElementById('hdnRemovePath').value = path;
            window.parent.document.getElementById('btnRemoveNode').click();
        }

        function UpdateParent() {
            alert('库区修改成功！');
            //window.parent.document.getElementById('btnUpdateSelected').click();
        }

        function ReloadParent() {
            alert('库区添加成功！');
            window.parent.document.getElementById('btnReload').click();
        }
        function openwin() {
            window.open("BatchAssignedProduct.aspx", "", "height=410, width=600,top=200px,left=300px, toolbar=no, menubar=no, scrollbars=no, resizable=no, location=no, status=no")
        }
        function CheckBeforeSubmit() {
            var areacode = document.getElementById('txtAreaCode').value;
            var areaname = document.getElementById('txtAreaName').value; //document.getElementById('txtTitle').value.trim();
            if (areacode == "") {
                alert('库区编码不能为空！');
                return false;
            }
            if (areacode.length != 3) {
                alert('库区编码长度为三码！');
                return false;
            }
            if (areaname == "") {
                alert('库区名称不能为空！');
                return false;
            }
        }
    </script>
</head>
<body >
    <form id="form1" runat="server" style=" height:98%">
            <table width="99%" class="maintable" align="center" cellspacing="0" cellpadding="0" bordercolor="#ffffff" border="1">
                <tr style="display:none; height:20px">
                <td colspan="4" >
                    <asp:TextBox ID="txtAreaID" runat="server" CssClass="HiddenControl" 
                        Width="71px"></asp:TextBox>
                    <input class="ButtonCreate" name="btnBack" onclick="openwin()" type="button" value="批量分配指定卷烟" />
                </td>
                </tr>
                <tr><td colspan="4">库区</td></tr>
                <tr>
                    <td  align="center" class="musttitle" style="width:15%;">仓库编码</td>
                    <td style="width:35%;" >
                        <asp:TextBox ID="txtWHID" runat="server" CssClass="TextRead" Width="80%" ></asp:TextBox>
                    </td>
                          
                    <td align="center" class="musttitle" style="width:15%;">仓库名称</td>
                    <td style="width:35%;" >
                        <asp:TextBox ID="txtWhName" runat="server" CssClass="TextRead" Width="80%"></asp:TextBox>
                    </td>
                </tr>                   
                <tr>
                    <td align="center" class="musttitle" style="width:15%;">
                    库区编码
                    </td> 
                    <td style="width:35%;">
                        <asp:TextBox ID="txtAreaCode" runat="server" CssClass="TextRead" Width="80%"></asp:TextBox>   
                    </td>
                    <td align="center" class="musttitle" style="width:15%;">
                        库区名称
                    </td> 
                    <td style="width:35%;" >
                        <asp:TextBox ID="txtAreaName" runat="server"  CssClass="TextBox" Width="80%" ></asp:TextBox>
                    </td>
                </tr>
                     
                <tr>
                    <td align="center" class="smalltitle" style="width:15%;">备注</td>
                    <td colspan="3" align="left">
                        <asp:TextBox ID="txtMemo" runat="server" Width="92%" Rows="10" TextMode="MultiLine" Height="116px"></asp:TextBox>
                    </td>
                </tr>
                <tr><td  align="center" colspan="4" style="height:35px; text-align:center;">
                    &nbsp;
                    <asp:Button ID="btnSave" runat="server" CssClass="button" Text="保存"  Height="26px" Width="57px" OnClick="btnSave_Click"  OnClientClick="return CheckBeforeSubmit()"/>
                          
                </td></tr>
            </table>  
    </form>
</body>
</html>
