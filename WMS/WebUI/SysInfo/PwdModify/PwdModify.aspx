<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PwdModify.aspx.cs" Inherits="WebUI_SysInfo_PwdModify_PwdModify" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>密码修改界面</title>
    <link rel="stylesheet" href="../../../css/main.css" type="text/css" />
    <link rel="stylesheet" href="../../../css/op.css" type="text/css" />
    <script type="text/javascript" src="../../../JScript/Common.js"></script>
    
    <script type="text/javascript">
        function AckPassword() {
            var txtNewPwd = document.getElementById("txtNewPwd").value;
            var txtAckPwd = document.getElementById("txtAckPwd").value;

            if (document.getElementById("txtNewPwd").value == document.getElementById("txtAckPwd").value) // txtNewP==txtAckPwd)
            {
                return true;
            }
            else {
                document.getElementById("labMessage").innerText = "密码不一致";
                return false;
            }
        }
        function CancelModify() {
            document.getElementById("txtNewPwd").value = "";
            document.getElementById("txtAckPwd").value = "";
            document.getElementById("txtOldPwd").value = "";
            return false;
        }
        function Exit() {

            window.parent.removetab();
            return false;
        }

    </script>
</head>
<body style="text-align: left; width: 100%; height: 100%;">
    <form id="form1" runat="server" defaultfocus="txtOldPwd">  
        <div style="height:120px;"></div>
        <table id="ModifyPwd" border="0" cellpadding="3" cellspacing="0"  style="width:400px"  align="center">
            <tr>
              <td colspan="2"  style=" text-align:center; color:#1a70ad;height:55px; font-size:13pt;">
                  <span style=""> <strong>::: 用户密码修改 :::</strong></span>
              </td>
            </tr>
            <tr >
                <td   align="center" style=" width:20%;" >
                    用 户 名:</td>
                <td  >
                <asp:TextBox ID="txtUserName" runat="server" ReadOnly="True" CssClass="TextBox" 
                        Width="80%" ></asp:TextBox></td>
            </tr>
            <tr>
                <td   align="center" style=" width:20%;"> 旧 密 码:</td>
                <td>
                   <asp:TextBox ID="txtOldPwd" runat="server"  TextMode="Password" 
                        CssClass="TextBox" Width="80%"></asp:TextBox></td>
            </tr>
            <tr>
                 <td  align="center" style=" width:20%;">
                     新 密 码:</td>
                 <td>
                    <asp:TextBox ID="txtNewPwd" runat="server" TextMode="Password"  
                         CssClass="TextBox" Width="80%"></asp:TextBox></td>
            </tr>
             <tr  >
                <td   align="center" style=" width:20%;"> 确认密码:</td>
                <td>
                   <asp:TextBox ID="txtAckPwd" runat="server" TextMode="Password" 
                        CssClass="TextBox" Width="80%"></asp:TextBox></td>
            </tr>
            
            <tr >
                  <td colspan="2" style=" text-align :center; height: 35px; padding-top:5px;"> 
                         <asp:Button ID="Button1" runat="server" OnClientClick="return AckPassword();" OnClick="lbtnSave_Click"  CssClass="ButtonSave" Text="保存"/>&nbsp;&nbsp;&nbsp;&nbsp;
                      <asp:Button ID="Button2" runat="server" OnClientClick="return Exit()"   
                             CssClass="ButtonExit"  Text="取消" Height="16px"/><br />
                        
                  </td> 
            </tr>
             <tr >
                  <td colspan="2" style=" text-align :center; height: 25px; "> 
                        <asp:Label ID="labMessage" runat="server" ForeColor="Red" Width="112px" ></asp:Label>  
                  </td> 
            </tr>

        </table>
    </form>
</body>
</html>

