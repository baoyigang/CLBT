<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WarehouseCell.aspx.cs" Inherits="WebUI_Query_WarehouseCell" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>货位信息显示</title>
     <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
        <link href="~/Css/op.css" type="text/css" rel="stylesheet" /> 
        <script type="text/javascript" src="../../JQuery/jquery-1.8.3.min.js"></script>
         <script type="text/javascript" src="../../JScript/DataProcess.js"></script>
         <script type="text/javascript" src= "../../JScript/Common.js"></script>
        <script type="text/javascript" language="javascript">
            var oldcell;
            $(document).ready(function () {
                $(window).resize(function () {
                    resize();
                });
            });
            function resize() {
                var h = document.documentElement.clientHeight;
                $("#pnlCell").css("height", h - 20);
            }

            function selectedcell(cellobj) {
                var obj = document.getElementById(cellobj);
                if (oldcell != null) {
                    $("#" + oldcell.id).removeClass("cellfalg");

                }
                //选中货位
                $("#" + obj.id).addClass("cellfalg");
                oldcell = obj;
            }

  

            function ShowCellInfo(obj) {
                closeinfo();
                selectedcell(obj);
                var product = document.getElementById("productinfo");
                var row = new Object();
                row.CellCode = obj;
                var json = eval(Ajax("GetCellInfo", row));

                if (json) {
                    document.getElementById("Spec").innerText = json[0].Spec == null ? "" : json[0].Spec;
                    document.getElementById("Barcode").innerText = json[0].Barcode;
                    document.getElementById("Weight").innerText = json[0].Weight == null ? "" : json[0].Weight;

                  
                    document.getElementById("BillNo").innerText = json[0].BillNo;

                    document.getElementById("Indate").innerText = json[0].InDate == null ? "" : new Date(parseInt(json[0].InDate.replace(/\D/igm, ""))).Format("yyyy-MM-dd"); 
                    document.getElementById("AreaName").innerText = json[0].AreaName;
                    document.getElementById("ShelfName").innerText = json[0].ShelfName;
                    document.getElementById("CellColumn").innerText = json[0].CellColumn;
                    document.getElementById("CellRow").innerText = json[0].CellRow;
                    document.getElementById("CellCode").innerText = json[0].CellCode;

                    if (json[0].IsLock == "0")
                        document.getElementById("State").innerText = "正常";
                    else
                        document.getElementById("State").innerText = "锁定";
                    if (json[0].ErrorFlag == "1")
                        document.getElementById("State").innerText = "异常";
                    showinfo(obj);
                }
                else {
                    closeinfo();
                }
            }
            function showinfo(cellobj) {
                var obj = document.getElementById(cellobj);
                var product = document.getElementById("productinfo");
                var objtop = obj.offsetTop;
                var objheight = obj.clientHeight;
                var objleft = obj.offsetLeft;
                // while (obj = obj.offsetParent) { objtop += obj.offsetTop; objleft += obj.offsetLeft; }

                product.style.top = parseFloat(objtop + objheight) + "px";
                if ((objleft + parseFloat(product.style.width)) > document.body.clientWidth) {
                    product.style.left = parseFloat(objleft) - parseFloat(product.style.width) + "px";
                }
                else
                    product.style.left = parseFloat(objleft) + "px";

                product.style.display = "block";
            }

            function closeinfo() {
                var product = document.getElementById("productinfo");
                product.style.display = "none";
            }
    </script>
    <style type="text/css">
    .cellfalg
    {
         background-image:url(../../images/flag.png);
         background-repeat:no-repeat;   
    }
    .cellinfo
    {
   width:65px;
   font-size:12px;
    }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Panel ID="pnlCell" runat="server"  Width="100%" Height="450px"  style="overflow:auto;" >
    </asp:Panel>
    <div id="productinfo"  style=" width:380px; height:180px; position:absolute; background-color:#dbe7fd;opacity:0.9; display:none; border:1px solid #000;">
        <div id="btclose" style=" width:100%; height:20px;">
          <span id="cellcode" style="float:left"></span>
          <span onclick="closeinfo()"  style=" float:right; width:15px; height:20px;  cursor:pointer">X</span>
        </div>
        <div>
             <table>
                   <tr>
                      <td colspan="4" class="cellinfo">
                        <b>产品信息</b>
                      </td>
                   </tr>
                     <tr>
                      <td  class="cellinfo" style="width:20%;">
                            &nbsp;规格:
                      </td>
                      <td colspan="3" id="Spec">
                        
                      </td>
                      
                   </tr>
                   <tr>
                      <td  class="cellinfo" style="width:20%;">
                            &nbsp;熔次卷号:
                      </td>
                      <td id="Barcode">
                        
                      </td>

                       <td   class="cellinfo" style="width:20%;">
                             &nbsp;重量:
                      </td>
                      <td id="Weight">
                        
                      </td>
                      
                     
                   </tr>
                   
                 
                    
                    <tr>
                        <td   class="cellinfo"  style="width:20%;">
                             &nbsp;单据号:
                      </td>
                      <td id="BillNo">
                           
                      </td>
                      <td   class="cellinfo"  style="width:20%;">
                             &nbsp;入库时间:
                      </td>
                      <td id="Indate">
                          
                      </td>

                     
                   </tr>
                    <tr>
                      <td  colspan="4" class="cellinfo">
                        <b>货架信息</b>
                      </td>
                   </tr>
                    <tr>
                      <td class="cellinfo" style="width:20%;" >
                             &nbsp;库区名称:
                      </td>
                      <td id="AreaName">
                          
                      </td>
                       <td   class="cellinfo"  style="width:20%;">
                             &nbsp;货架名称:
                      </td>
                      <td id="ShelfName"> 
                          
                      </td>
                   </tr>
                    <tr>
                      <td  class="cellinfo"  style="width:20%;">
                             &nbsp;货位:
                      </td>
                      <td id="CellCode">
                          
                      </td>
                      <td  class="cellinfo"  style="width:20%;">
                             &nbsp;状态:
                      </td>
                      <td id="State">
                          
                      </td>
                     
                   </tr>
                    
                   <tr>
                      <td   class="cellinfo" style="width:20%;">
                             &nbsp;列:
                      </td>
                      <td id="CellColumn">
                          
                      </td>
                      <td  class="cellinfo"  style="width:20%;">
                             &nbsp;层:
                      </td>
                      <td id="CellRow">
                          
                      </td>
                   </tr>
                   
                   
               </table>
        </div>
    </div>




    </form>
</body>
</html>
