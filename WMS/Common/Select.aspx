<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Select.aspx.cs" Inherits="Common_Select" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
    <link href="~/Css/op.css" type="text/css" rel="stylesheet" /> 
    <script type="text/javascript" src="../JQuery/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src="../JScript/Common.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(window).resize(function () {
                resize();
            });
        });
        function resize() {
            var h = document.documentElement.clientHeight - 65;

            $("#SelectDiv").css("width", document.documentElement.clientWidth);
            $("#tbSearch").css("width", document.documentElement.clientWidth);
            $("#tbBottom").css("width", document.documentElement.clientWidth);
            $("#SelectDiv").css("height", h);

        }
        function AddValues(oChk, strReturn) {
            var chkSelect = document.getElementById(oChk);
            if (chkSelect.checked) {
                (chkSelect.parentElement).parentElement.className = "bottomtable";
                if (SelectPage.HdnSelectedValues.value == '')
                    SelectPage.HdnSelectedValues.value += strReturn;
                else
                    SelectPage.HdnSelectedValues.value += ',' + strReturn;
            }
            else {
                (chkSelect.parentElement).parentElement.className = "";
                SelectPage.HdnSelectedValues.value = SelectPage.HdnSelectedValues.value.replace(',' + strReturn, '');
                SelectPage.HdnSelectedValues.value = SelectPage.HdnSelectedValues.value.replace(strReturn + ',', '');
                SelectPage.HdnSelectedValues.value = SelectPage.HdnSelectedValues.value.replace(strReturn, '');
            }
        }
        function SelectAll(tempControl) {
            var theBox = tempControl;
            xState = theBox.checked;

            elem = theBox.form.elements;
            for (i = 0; i < elem.length; i++)
            //if(elem[i].type=="checkbox" && elem[i].id!=theBox.id && elem[i].id.substring(elem[i].id.length-2,elem[i].id.length)==theBox.id.substring(theBox.id.length-2,theBox.id.length))
                if (elem[i].id.indexOf("chkSelect") >= 0) {
                    if (elem[i].checked != xState) {
                        elem[i].click();
                    }
                }
        }
        function SelectedCell() {
            elem = SelectPage.elements;
            for (i = 0; i < elem.length; i++)
                if (elem[i].type == 'checkbox' && elem[i].checked == true) {
                    (elem[i].parentElement).parentElement.className = "bottomtable";
                }
        }
        function AllClear(bln) {
            elem = SelectPage.elements;
            for (i = 0; i < elem.length; i++)
                if (elem[i].type == "checkbox" && elem[i].id.substring(elem[i].id.length - 6, elem[i].id.length) == "Select") {
                    if (bln == 0)
                        elem[i].checked = true;
                    else
                        elem[i].checked = false;
                    elem[i].click();
                }
        }
        function Close() {
            SelectPage.HdnSelectedValues.value = "";
            window.returnValue = document.getElementById('HdnSelectedValues').value;
            window.close();
        }
        function Select() {
            window.returnValue = "[" + document.getElementById('HdnSelectedValues').value + "]";
            window.close();
        }
        function SelectSearch() {
            if (trim(document.getElementById("txtSearch").value) == "") {
                alert("请输入查询内容！");
                document.getElementById("txtSearch").focus();
                return false;
            }
        }
  </script>
  </head>
<body>
    <form id="SelectPage" runat="server" >
     <asp:ScriptManager ID="ScriptManager1" runat="server" />  
    <asp:UpdateProgress ID="updateprogress" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>            
            <div id="progressBackgroundFilter" style="display:none"></div>
            <div id="processMessage"> Loading...<br /><br />
                 <img alt="Loading" src="../images/loading.gif" />
            </div>            
            </ProgressTemplate>
        </asp:UpdateProgress>  
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">                
            <ContentTemplate>
                <table id="tbSearch" class="maintable" width="100%" align="center" cellspacing="0" cellpadding="0" bordercolor="#ffffff" border="1">
			        <tr> 
				        <td class="smalltitle" align="center" width="10%" >查询栏位</td>
				        <td  width="15%" height="20">&nbsp;
					        <asp:dropdownlist id="ddlFieldName" runat="server" Width="90%"></asp:dropdownlist>
                        </td>
				        <td class="smalltitle" align="center" width="10%">查询内容</td>
				        <td  width="40%" height="20" valign="middle">&nbsp;
                            <asp:textbox id="txtSearch" tabIndex="1" runat="server" Width="90%" 
                                CssClass="TextBox" ></asp:textbox>
                        </td>
                        <td width="25%" align="left">
                            &nbsp;<asp:button id="btnSearch" tabIndex="2" runat="server" Width="60px" CssClass="ButtonQuery" Text="查询" OnClientClick="return SelectSearch()" onclick="btnSearch_Click"></asp:button>
                            &nbsp;<asp:button id="btnRefresh" tabIndex="2" runat="server" Width="60px" CssClass="ButtonRefresh" Text="刷新" onclick="btnRefresh_Click"></asp:button>
                        </td>
			        </tr>
				    
		        </table>
            <div id="SelectDiv" style="overflow: auto; WIDTH: 100%; HEIGHT: 470px" >
                 
                <asp:GridView ID="GridView1" runat="server" SkinID="GridViewSkin" Width="100%" AllowPaging="True"  PageSize="12"  
                 OnRowDataBound="GridView1_RowDataBound"  >
                    <RowStyle Wrap="False"></RowStyle>
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>                        
				                <input type="checkbox" runat="server" id="chkHeadSelect" onclick="SelectAll(this);"/>                    
                            </HeaderTemplate>
			                <ItemTemplate>
				                <input type="checkbox" runat="server" id="chkSelect"/>
			                </ItemTemplate>
                            <HeaderStyle Width="20px" />
                            <ItemStyle HorizontalAlign="Center" />
		                </asp:TemplateField>
		                <asp:TemplateField HeaderText="选取">
			                <ItemTemplate>
                                <asp:Button ID="btnSingle" runat="server" Text="选取" CssClass="ButtonSelect"  />
			                </ItemTemplate>                    
                            <ControlStyle Width="50px" Height="20px" />
		                </asp:TemplateField>		        
               
	                </Columns>
	                 <PagerSettings Visible="False" />
                </asp:GridView>
        
            </div>
            <table width="100%" id="tbBottom" class="table_bgcolor">
                <tr>
                    <td style=" height:3px;"></td>
                </tr>
                <tr>
                     <td align="left" style=" width:25%"  >
                        &nbsp;&nbsp;<asp:Button ID="btnSelect" runat="server" Text="确定" Width="60px" CssClass="ButtonOk" OnClientClick="return Select();" />
                    &nbsp;<asp:Button ID="btnClose" runat="server" Text="关闭" Width="60px" CssClass="ButtonExit" OnClientClick="return Close();" /></td>

                    <td align= "right">
                        <asp:LinkButton ID="btnFirst" runat="server" OnClick="btnFirst_Click" >首页</asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="btnPre" runat="server"   OnClick="btnPre_Click" >上一页</asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="btnNext" runat="server"  OnClick="btnNext_Click" >下一页</asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="btnLast" runat="server"  OnClick="btnLast_Click" >尾页</asp:LinkButton>&nbsp;&nbsp;
                        <asp:TextBox ID="txtPageNo" runat="server" Width="34px" CssClass="pagetext" onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode)) ;"
                         ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'));" onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'));"
                         
                            onkeydown="if(event.keyCode==13){event.keyCode=9;document.getElementById('btnToPage').click();return false;}" ></asp:TextBox>
                         <asp:LinkButton ID="btnToPage" runat="server"  Width="40px" OnClick="btnToPage_Click" >跳转</asp:LinkButton>&nbsp;&nbsp;  
                        <asp:Label ID="lblPage" runat="server" Text=""></asp:Label>&nbsp;     
                    </td>
                    
                </tr>
            </table>
           
            <input id="HdnSelectedValues" type="hidden" name="HdnSelectedValues" runat="server" />
            <asp:HiddenField ID="hideTargetControls" runat="server" />
         </ContentTemplate>
     </asp:UpdatePanel> 
    </form>
</body>
</html>