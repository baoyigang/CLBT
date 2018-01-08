<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GroupUserManage.aspx.cs" Inherits="WebUI_SysInfo_RoleManage_GroupUserManage" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <base target="_self" />
    <script type="text/javascript" src="../../../JQuery/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src="../../../JScript/Common.js"></script>
    <link href="../../../css/main.css" rel="Stylesheet" type="text/css" />
    <link href="../../../css/op.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            $(window).resize(function () {
                resize();
            });
        });
        function resize() {
            var h = document.documentElement.clientHeight - 55;
            $("#Sub-container").css("height", h);
        }

        function okFunc(url) {
            //      var topFrame =parent.frames['mainFrame'];
            //      if(topFrame!=null)
            //         parent.frames['mainFrame'].location=url;
            //      else location.href=url;
        }

        function Close(obj) {
            window.returnValue = obj;
            window.close();
        }
    </script>
     
</head>
<body >
    <form id="form1" runat="server">
         <asp:ScriptManager ID="ScriptManager1" runat="server" /> 
          <asp:UpdateProgress ID="updateprogress" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>            
                <div id="progressBackgroundFilter" style="display:none"></div>
                <div id="processMessage"> Loading...<br /><br />
                        <img alt="Loading" src="../../../images/loading.gif" />
                </div>      
            </ProgressTemplate> 
        </asp:UpdateProgress>  
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">                
            <ContentTemplate>
                <table id="Table1"  class="maintable" cellspacing="0" cellpadding="0" bordercolor="#ffffff" width="100%" height="25px" runat="server">
                    <tr>
                        <td style="width:50%; height:25px">
                           &nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Label" Font-Size="10pt"></asp:Label>
                   
                        </td>
                       <td style="width:50%" align="right" >
                         <asp:Button ID="btnSave" runat="server" CssClass="ButtonSave" OnClick="btnSave_Click" Text="保存" />&nbsp;
                         <asp:Button ID="btnClose" runat="server" Text="关闭" Width="60px" CssClass="ButtonExit" OnClientClick="return Close('0');" />&nbsp;
                        </td>
                    </tr>
                </table>
                <div id="Sub-container"  style="overflow: auto; width: 100%; height: 400px;">
                    <asp:GridView ID="gvGroupList" runat="server" SkinID="GridViewSkin" AllowPaging="false" Width="100%"
                                     AutoGenerateColumns="False" OnRowDataBound="gvGroupList_RowDataBound"  >
                        <Columns>
                            <asp:BoundField DataField="UserName" HeaderText="用户名"></asp:BoundField>
                            <asp:BoundField DataField="GroupName" HeaderText="所属用户组"></asp:BoundField>
                            <asp:TemplateField >
                                <HeaderTemplate>
                                    <input type="checkbox" onclick="selectAll('GridView1',this.checked);" />                    
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox id="cbSelect" runat="server" ></asp:CheckBox>                   
                                </ItemTemplate>
                                <HeaderStyle  ></HeaderStyle>
                                <ItemStyle   HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            <asp:BoundField DataField="UserID"></asp:BoundField>
                        </Columns>
                        <PagerSettings Visible="False" />
                    </asp:GridView>
                 </div>
                  <div>
                    &nbsp;&nbsp;<asp:LinkButton ID="btnFirst" runat="server" OnClick="btnFirst_Click" Text="首页"></asp:LinkButton> 
                    &nbsp;<asp:LinkButton ID="btnPre" runat="server" OnClick="btnPre_Click" Text="上一页"></asp:LinkButton> 
                    &nbsp;<asp:LinkButton ID="btnNext" runat="server" OnClick="btnNext_Click" Text="下一页"></asp:LinkButton> 
                    &nbsp;<asp:LinkButton ID="btnLast" runat="server" OnClick="btnLast_Click" Text="尾页"></asp:LinkButton> 
                    &nbsp;<asp:textbox id="txtPageNo" onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))"
					        onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))"
					        runat="server" Width="56px" CssClass="TextBox" ></asp:textbox>
                    &nbsp;<asp:linkbutton id="btnToPage" runat="server" onclick="btnToPage_Click" Text="跳转"></asp:linkbutton>
                    &nbsp;<asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True"  Visible="false"></asp:DropDownList>
                    &nbsp;<asp:Label ID="lblCurrentPage" runat="server" ></asp:Label>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
        
    </form>
</body>
</html>