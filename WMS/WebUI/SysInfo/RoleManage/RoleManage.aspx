<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RoleManage.aspx.cs" Inherits="WebUI_SysInfo_RoleManage_RoleManage" %>
<%@ Register Assembly="YYControls" Namespace="YYControls" TagPrefix="yyc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
     
     <link href="../../../css/main.css" type="text/css" rel="Stylesheet" />
      <link href="../../../css/op.css" type="text/css" rel="Stylesheet" />
     <script type="text/javascript" src="../../../JQuery/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src="../../../JScript/Common.js"></script>
    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            $(window).resize(function () {
                resize();
            });
        });

        function resize() {
            var h = document.documentElement.clientHeight - 5;
            $("#table-container").css("height", h);
            $("#dvGroupUser").css("height", h - 277);

            $('#plTree').css("height", h - 30);
        }


        function RoleSet(GroupID, GroupName) {
            var date = new Date();
            var t = date.getMinutes() + date.getSeconds() + date.getMilliseconds();
            var iframeRoleSet = document.getElementById("iframeRoleSet");
            iframeRoleSet.src = "RoleSet.aspx?GroupID=" + GroupID + "&GroupName=" + encodeURI(GroupName) + "&time=" + t;
        }

        function UserSet() {
            var GroupID = $('#hdnRowValue').val();
            var GroupName = $('#hdnRowGroupName').val();
            var returnvalue = window.showModalDialog('GroupUserManage.aspx?GroupID=' + GroupID + '&GroupName=' + encodeURI(GroupName), window, 'top=0;left=0;toolbar=no;menubar=yes;scrollbars=no;resizable=yes;location=no;status=no;dialogWidth=450px;dialogHeight=500px');
            if (returnvalue == "1")
                return true;
            else
                return false;
        }

        function ShowGroupUserList(GroupID, GroupName) {
            //            var d = new Date();
            //            var t = d.getMinutes() + d.getMilliseconds();
            //            document.getElementById("iframeGroupUserList").src = "GroupUserList.aspx?GroupName=" + encodeURI(GroupName) + "&GroupID=" + GroupID + "&t=" + t;
            //            RoleSet(GroupID, GroupName);
        }




        function HandleCheckbox() {
            var element = event.srcElement;
            if (element.tagName == "INPUT" && element.type == "checkbox") {
                var checkedState = element.checked;
                while (element.tagName != "TABLE") // Get wrapping table               
                {
                    element = element.parentElement;
                }
                var parentElement = element;

                if (checkedState) {
                    CheckParents(element);
                }

                element = element.nextSibling; //element.tagName = DIV

                if (element != null && typeof (element) != "undefined" && typeof (element.tagName) != "undefined") // If no childrens then exit 
                {
                    var childTables = element.getElementsByTagName("TABLE");

                    for (var tableIndex = 0; tableIndex < childTables.length; tableIndex++) {
                        CheckTable(childTables[tableIndex], checkedState);
                    }
                }
                if (checkedState == false) {
                    UnCheckParents(parentElement);
                }

            }
        }

        // Uncheck the parents of the given table, Can remove the recurse (redundant)
        function CheckParents(table) {
            if (table == null || table.rows[0].cells.length == 2) // This is the root 
            {
                return;
            }
            var parentTable = table.parentElement.previousSibling;
            CheckTable(parentTable, true);
            CheckParents(parentTable);
        }

        // Check the parents of the given table, Can remove the recurse (redundant)
        function UnCheckParents(table) {

            if (table == null || table.rows[0].cells.length == 2) // This is the root  
            {
                return;
            }
            var parentTable = table.parentElement.previousSibling;

            var checkedCount = GetCheckedCount(table.parentElement);
            if (checkedCount == 0) {
                CheckTable(parentTable, false);
            }
            UnCheckParents(parentTable);
        }

        // Handle the set of checkbox checked state
        function CheckTable(table, checked) {
            var checkboxIndex = table.rows[0].cells.length - 1;
            var cell = table.rows[0].cells[checkboxIndex];
            var checkboxes = cell.getElementsByTagName("INPUT");
            if (checkboxes.length == 1) {
                checkboxes[0].checked = checked;
            }

        }
        //Get checked children count
        function GetCheckedCount(table) {
            var checkedCount = 0;
            var element = table.nextSibling;
            var childTable = table.getElementsByTagName("TABLE");

            for (var tableIndex = 0; tableIndex < childTable.length; tableIndex++) {
                var childTables = childTable[tableIndex];
                var checkboxIndex = childTables.rows[0].cells.length - 1;
                var cell = childTables.rows[0].cells[checkboxIndex];
                var checkboxes = cell.getElementsByTagName("INPUT");
                if (checkboxes.length == 1 && checkboxes[0].checked == true) {
                    checkedCount++;
                }
            }
            return checkedCount;
        }                                                                          
 

                  
  </script>  
</head>
<body>
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
              <table id="table-container" cellpadding="0" cellspacing="0" >
                <tr>
                  <td style=" vertical-align:top; width: 300px;"><!--GroupList-->
                        <div style="overflow: auto; WIDTH: 100%; height:200px">
                            <asp:GridView ID="gvGroupList" runat="server" SkinID="GridViewSkin" AllowPaging="false" Width="100%"
                             AutoGenerateColumns="False" OnRowDataBound="gvGroupList_RowDataBound"  >
                                <Columns>
                                    <asp:BoundField DataField="GroupID" HeaderText="ID">
                                        <HeaderStyle Width="0px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="GroupName" HeaderText="用户组名称">
                                        <ItemStyle Width="85px" />
                                    </asp:BoundField>
                                      
                                </Columns>
                                 <PagerSettings Visible="False" />
                            </asp:GridView>
                        </div>
                          <div>
                            &nbsp;<asp:LinkButton ID="btnFirst" runat="server" OnClick="btnFirst_Click" Text="首页"></asp:LinkButton> 
                            &nbsp;<asp:LinkButton ID="btnPre" runat="server" OnClick="btnPre_Click" Text="上一页"></asp:LinkButton> 
                            &nbsp;<asp:LinkButton ID="btnNext" runat="server" OnClick="btnNext_Click" Text="下一页"></asp:LinkButton> 
                            &nbsp;<asp:LinkButton ID="btnLast" runat="server" OnClick="btnLast_Click" Text="尾页"></asp:LinkButton> 
                            &nbsp;<asp:textbox id="txtPageNo" onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))"
					                onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))"
					                runat="server" Width="30px" CssClass="TextBox" ></asp:textbox>
                            &nbsp;<asp:linkbutton id="btnToPage" runat="server" onclick="btnToPage_Click" Text="跳转"></asp:linkbutton>
                            &nbsp;<asp:Label ID="lblCurrentPage" runat="server" Visible="False" ></asp:Label>
                        </div>
                 
                       <table class="maintable" style="width:298px; height:30px;">
                            <tr>
                                <td>
                                    <b>用户组成员</b>
                                </td>
                            </tr>
                        </table>
             
                        <div id="dvGroupUser" style="overflow: auto; WIDTH: 100%; height:200px">
                            <asp:GridView ID="gvGroupListUser" runat="server"   
                                OnRowDataBound="gvGroupListUser_RowDataBound"  AutoGenerateColumns="False" 
                                AllowPaging="True" PageSize="5" SkinID="GridViewSkin" Width="100%" >
                                <Columns>
                                    <asp:BoundField DataField="UserID" HeaderText="ID"></asp:BoundField>
                                      <asp:BoundField DataField="UserName" HeaderText="用户名">
                                          <HeaderStyle Width="20%" />
                                      </asp:BoundField>
                                      <asp:BoundField DataField="GroupName" HeaderText="用户组">
                                          <HeaderStyle Width="30%" />
                                      </asp:BoundField>

                                      
                                       <asp:TemplateField HeaderText="操作" >
                                        <ItemTemplate>
                                          <asp:Button ID="btnDeleteUser" CommandName="btnDeleteUser" CommandArgument= '<%# DataBinder.Eval(Container.DataItem, "UserID")%> ' CssClass="ButtonDel" 
                                           runat="server"  Text="删除用户" OnClientClick="return confirm('确定要删除此用户？', '删除提示');" OnClick="btnDeleteUser_Click"/>   
                                        </ItemTemplate>
                                        <ItemStyle Width="85px" Wrap="False" />
                                        <HeaderStyle Width="85px" Wrap="False" />
                                    </asp:TemplateField>
                                </Columns>
                                 <PagerSettings Visible="False" />
                            </asp:GridView>
                        </div>
                         <div style="height:30px;">
                            &nbsp;&nbsp;<asp:LinkButton ID="btnFirstSub1" runat="server" 
                                OnClick="btnFirstSub1_Click" Text="首页"></asp:LinkButton> 
                            &nbsp;<asp:LinkButton ID="btnPreSub1" runat="server" OnClick="btnPreSub1_Click" 
                                Text="上一页"></asp:LinkButton> 
                            &nbsp;<asp:LinkButton ID="btnNextSub1" runat="server" 
                                OnClick="btnNextSub1_Click" Text="下一页"></asp:LinkButton> 
                            &nbsp;<asp:LinkButton ID="btnLastSub1" runat="server" 
                                OnClick="btnLastSub1_Click" Text="尾页"></asp:LinkButton> 
                            &nbsp;<asp:textbox id="txtPageNoSub1" onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))"
					                onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))"
					                runat="server" Width="30px" CssClass="TextBox" ></asp:textbox>
                            &nbsp;<asp:linkbutton id="btnToPageSub1" runat="server" 
                                onclick="btnToPageSub1_Click" Text="跳转"></asp:linkbutton>
                            &nbsp;<asp:Label ID="lblCurrentPageSub1" runat="server" Visible="False" ></asp:Label>
                        </div>
                  </td>
          
                  <td style=" vertical-align:top; width: 100%; border-left:2px solid #5384bb;"><!--RoleSet-->
                       <table class="maintable" style="width:100%; height:25px" >
                        <tr style="font-size:10pt; font-weight:bold; color:Black;">
                            <td style=" width:60%" valign="middle">  
                                <asp:Label ID="lbTitle" runat="server" Font-Bold="True" >用户组权限设置</asp:Label> &nbsp;&nbsp;
                                <asp:LinkButton ID="lnkBtnExpand" runat="server" OnClick="lnkBtnExpand_Click" BorderStyle="None">展开</asp:LinkButton> &nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="lnkBtnCollapse" runat="server" OnClick="lnkBtnCollapse_Click">折叠</asp:LinkButton>
                            </td>
                             
                        
                            <td align="right" valign="middle" style="width:40%; height: 25px;">
                                <asp:Button ID="lnkBtnSave" runat="server" CssClass="ButtonSave" OnClick="lnkBtnSave_Click"  Text="保存" />&nbsp;
                                <asp:Button ID="btnAddUser" CssClass="ButtonCreate"  runat="server"  Text="添加用户"  OnClientClick="return UserSet();" OnClick="btnAddUser_Click"/> &nbsp;
                                <asp:Button ID="lnkBtnCancle" runat="server" CssClass="ButtonExit" Text="离开" OnClientClick="return Exit();"/>&nbsp;
                            </td>
         
                        </tr>
                      </table>
                      <div id="plTree"  style="height:443px; width:100%; overflow: auto; " >
                       <yyc:smarttreeview id="sTreeModule" runat="server" allowcascadecheckbox="True"  onclick="HandleCheckbox();"
                            showlines="True" Font-Size="9pt" ExpandDepth="1">
                           <LeafNodeStyle ForeColor="MidnightBlue" />
                       </yyc:smarttreeview>
                    </div>
                  </td>
                </tr>
              </table>  
    
            <div id="divHiden" style="display:none;">
            <div>
                <asp:Button ID="btnReload" runat="server" Text="" OnClick="btnReload_Click"  CssClass="HiddenControl" />
                <asp:HiddenField ID="hdnRowIndex" runat="server" Value="0" />
                <asp:HiddenField ID="hdnRowValue" runat="server"  />
                <asp:HiddenField ID="hdnRowGroupName" runat="server"  />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
  
</body>
</html>

