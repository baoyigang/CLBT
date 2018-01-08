<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WarehouseEditPage.aspx.cs" Inherits="WebUI_CMD_WarehouseEditPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>仓库</title>
    <base target="_self" />
     <link href="../../css/op.css" rel="Stylesheet" type="text/css" />
    <link href="../../css/Main.css" rel="Stylesheet" type="text/css" />
   <script type="text/javascript" src="../../JQuery/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src= "../../JScript/Common.js"></script>
   
    <script language="javascript" type="text/javascript">
        function RefreshParent(path) {
            alert('仓库删除成功！');
            window.parent.document.getElementById('hdnRemovePath').value = path;
            window.parent.document.getElementById('btnRemoveNode').click();
        }


        function UpdateParent() {
            alert('仓库修改成功！');
            //window.parent.document.getElementById('btnUpdateSelected').click();
        }

        function ReloadParent() {
            alert('仓库添加成功！');
            window.parent.document.getElementById('btnReload').click();
        }
        function openwin() {
            window.open("BatchAssignedProduct.aspx", "", "height=410px, width=600px,top=200px,left=300px, toolbar=no, menubar=no, scrollbars=no, resizable=no, location=no, status=no")
        }
        function CheckBeforeSubmit() {
            var code = document.getElementById('txtWhCode').value;
            var name = document.getElementById('txtWhName').value; //document.getElementById('txtTitle').value.trim();


            if (code == "") {
                alert('仓库编码不能为空！');
                return false;
            }
            if (name == "") {
                alert('仓库名称不能为空！');
                return false;
            }
        }    
    </script>
   
    
</head>
<body >
    <form id="form1" runat="server">  
    
       <table width="99%" class="maintable" align="center" cellspacing="0" cellpadding="0" bordercolor="#ffffff" border="1">
            
            <tr style="display:none;">
                <td colspan="4"><asp:TextBox ID="txtWHID" runat="server" CssClass="HiddenControl" ></asp:TextBox>
                    &nbsp;</td>
            </tr>
            <tr><td colspan="4">仓库</td></tr>
            <tr>
                <td align="center" class="musttitle" style="width:15%;">
                    仓库编码
                </td>
                <td style="width:35%">
                <asp:TextBox ID="txtWhCode" runat="server" CssClass="TextRead" Width="80%" MaxLength="2"></asp:TextBox>
                </td>
                <td align="center" class="musttitle" style="width:15%;">
                    仓库名称
                </td> 
                <td style="width:35%">
                <asp:TextBox ID="txtWhName" runat="server"  CssClass="TextBox" Width="80%" 
                        MaxLength="20"></asp:TextBox>
                </td>
            </tr>
      
            <tr>
                <td align="center" class="smalltitle" style="width:15%; height:170px">备注</td> 
                <td colspan="3">
                <asp:TextBox ID="txtMemo" runat="server" CssClass="MultilineTextbox" 
                        Width="93%" Rows="10" TextMode="MultiLine" MaxLength="246"  
                        Height="164px"></asp:TextBox>
                </td>
            </tr> 
            <tr>
	        <td align="center" style="height:35px; text-align:center;" colspan="4">
                        <asp:Button ID="btnSave" runat="server" CssClass="ButtonCss" Text="保存"   Height="26px" Width="57px"  OnClick="btnSave_Click" OnClientClick="return CheckBeforeSubmit()" />&nbsp;
                </td>  
            </tr> 
        </table>
  
    </form>
</body>
</html>
