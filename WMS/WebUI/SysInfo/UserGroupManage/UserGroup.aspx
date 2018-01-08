<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserGroup.aspx.cs" Inherits="WebUI_SysInfo_UserGroupManage_UserGroup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>用户信息</title>
    <script type="text/javascript" src="../../../JQuery/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src="../../../JScript/Check.js?t=00"></script>
    <script type="text/javascript" src="../../../JScript/Common.js"></script>
    <link href="../../../css/main.css" rel="Stylesheet" type="text/css" />
    
    <link href="../../../css/op.css" rel="Stylesheet" type="text/css" />
   
    
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $(window).resize(function () {
                resize();
            });
        });
        function resize() {
            var h = document.documentElement.clientHeight - 55;
            $("#table-container").css("height", h);
        }
        //删除确认
        function DelConfirm(btnID) {
            var table = document.getElementById('gvMain');
            var hasChecked = false;
            for (var i = 1; i < table.rows.length; i++) {
                var cell = table.rows[i].cells[0];
                var chk = cell.getElementsByTagName("INPUT");
                if (chk[0].checked) {
                    hasChecked = true;
                    break;
                }
            }

            if (!hasChecked) {
                alert('请选择要删除的数据！');
                return false;
            }
            if (confirm('确定要删除选择的数据？', '删除提示')) {
                var btn = document.getElementById(btnID);
                btn.click();
                //window.location.reload();
            }
            else {
                return false;
            }
        }


        function CheckBeforeSubmit() {
            var groupname = document.getElementById('txtGroupName').value.trim();
            if (groupname == "") {
                alert('用户组名称不能为空！');
                return false;
            }
        }

        function SelectDialog2(strTarget, strTableName, strReturnField) {
            var date = new Date();
            var time = date.getMilliseconds();
            var aryTarget = strTarget.split(',');
            var aryField = strReturnField.split(',');
            if (aryTarget.length != aryField.length) {
                alert('参数有错！');
                return false;
            }

            if (window.document.all)//IE判断window.showModalDialog!=null
            {
                var returnvalue;
                returnvalue = window.showModalDialog("../../../Common/SelectDialog.aspx?TableName=" + strTableName + "&ReturnField=" + strReturnField + "&time=" + time, "",
                                         "top=0;left=0;toolbar=no;menubar=no;scrollbars=no;resizable=no;location=no;status=no;dialogWidth=680px;dialogHeight=450px");

                if (returnvalue == null) {
                    return false;
                }
                else if (returnvalue != '') {
                    var aryValue = new Array();
                    aryValue = returnvalue.split('|');
                    for (var i = 0; i < aryTarget.length; i++) {
                        var e = document.getElementById(aryTarget[i]);
                        if (e != null) {
                            e.value = aryValue[i];
                        }
                    }
                    return false;
                }
            }
            else {
                //参数
                var strPara = "height=450px;width=500px;help=off;resizable=off;scroll=no;status=off;modal=yes;dialog=yes";
                //打开窗口
                var url = "../../../Common/SelectDialog.aspx?TableName=" + strTableName + "&ReturnField=" + strReturnField + "&time=" + time + "&targetControls=" + strTarget;
                var DialogWin = window.open(url, "myOpen", strPara, true);
            }

        }
    </script>
</head>

<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" Visible="true">
        <ContentTemplate>
            <!--数据显示-->
               <div id="dvList" runat="server">
                <!--工具栏-->
                
               <table class="maintable"  width="100%" align="center" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="smalltitle" align="center" width="5%" >
                            查询栏位
                        </td>
                        <td  width="10%"> 
                            &nbsp; <asp:DropDownList ID="ddl_Field" runat="server">
                                    <asp:ListItem Selected="True" Value="GroupName">用户组名称</asp:ListItem>
                                    <asp:ListItem Value="Memo">备注</asp:ListItem>
                                </asp:DropDownList>
                        </td>
                            <td class="smalltitle" align="center" width="5%">
                            查询内容
                        </td>
                        <td height="20" width="10%" valign="middle" >
                                &nbsp;<asp:TextBox ID="txtKeyWords" runat="server" CssClass="TextBox" Width="85%"></asp:TextBox>
                        </td>
                        
                        
                        <td  width="15%">
                            <asp:Button ID="btnQuery" runat="server" Text="查询" OnClick="btnQuery_Click" CssClass="ButtonQuery" />
                            <asp:Button ID="btnRefresh" runat="server" CssClass="ButtonRefresh" OnClientClick="return Refresh()" tabIndex="2"  Text="刷新" Width="58px" />
                        </td>
                           
                        <td align="right" style="width:15%;">
                              <asp:Button ID="btnAdd" runat="server" Text="新增" CssClass="ButtonCreate" OnClick="btnCreate_Click"
                                    />
                            <asp:Button ID="btnDelete" runat="server" Text="删除" CssClass="ButtonDel" OnClick="btnDelete_Click"
                                OnClientClick="return DelConfirm('btnDelete')" />
                            
                            <asp:Button ID="btnExit" runat="server" Text="退出" OnClientClick="return Exit();" CssClass="ButtonExit" />
                        </td>
                         
                    </tr>
                         
                </table>
                   
                
                <!--数据-->
               <div id="table-container" style="overflow: auto; WIDTH: 100%; HEIGHT: 470px">
                    <asp:GridView ID="gvMain" runat="server"  Width="100%" OnRowEditing="gvMain_RowEditing" OnRowDataBound="gvMain_RowDataBound"
                          SkinID="GridViewSkin"  AutoGenerateColumns="False">
                        <RowStyle BackColor="WhiteSmoke" Height="28px" />
                        <HeaderStyle CssClass="GridHeader" />
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField HeaderText="操作">
                                <HeaderStyle Width="60px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="GroupName" HeaderText="用户组名称">
                                <HeaderStyle Width="120px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Memo" HeaderText="备注"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
               </div>
                <!--分页导航-->
                  <div>
                     &nbsp;&nbsp;<asp:LinkButton ID="btnFirst" runat="server" OnClick="btnFirst_Click" Text="首页"></asp:LinkButton> 
                    &nbsp;<asp:LinkButton ID="btnPre" runat="server" OnClick="btnPre_Click" Text="上一页"></asp:LinkButton> 
                    &nbsp;<asp:LinkButton ID="btnNext" runat="server" OnClick="btnNext_Click" Text="下一页"></asp:LinkButton> 
                    &nbsp;<asp:LinkButton ID="btnLast" runat="server" OnClick="btnLast_Click" Text="尾页"></asp:LinkButton> 
                    &nbsp;<asp:textbox id="txtPageNo" onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))"
					        onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))"
					        runat="server" Width="56px" CssClass="TextBox" ></asp:textbox>
                    &nbsp;<asp:linkbutton id="btnToPage" runat="server" onclick="btnToPage_Click" Text="跳转"></asp:linkbutton>
                    &nbsp;<asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True" Visible="false"></asp:DropDownList>
                    &nbsp;<asp:Label ID="lblCurrentPage" runat="server" ></asp:Label>
                </div>
            </div>
            <!--编辑-->
           <div id="dvEdit"   runat="server"  height="400px" width="100%" Visible="false">
                  <table class="maintable" width="100%" align="center" cellspacing="0" cellpadding="0" >
                    <tr>
                        <td colspan="2" style="height:30px">
                            <asp:Button ID="btnSave" Text=" 保 存" runat="server" OnClick="btnSave_Click" CssClass="ButtonSave"
                                OnClientClick="return CheckBeforeSubmit()" />
                            <asp:Button ID="btnCancel" Text="取 消" runat="server" CssClass="ButtonCancel" OnClick="btnCancel_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td class="musttitle" style="width:8%">
                            <font color="red">*</font>用户组名称

                        </td>
                        <td>
                            &nbsp;<asp:TextBox ID="txtGroupName" runat="server" CssClass="TextBox" MaxLength="20" 
                                Width="160px"></asp:TextBox>
                            <asp:TextBox ID="txtGroupID" runat="server" CssClass="HiddenControl" Width="0" Height="0" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="smalltitle" style="width:8%">
                            备注
                        </td>
                        <td>
                            &nbsp;<asp:TextBox ID="txtMemo" runat="server" Width="160px" Height="88px" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!--隐藏数据-->
    <div>
        <asp:HiddenField ID="hdnXGQX" Value="0" runat="server" />
        <asp:HiddenField ID="hdnOpFlag" Value="0" runat="server" />
    </div>
    </form>
</body>
</html>
