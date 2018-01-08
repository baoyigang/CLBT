<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WarehouseCellQuery.aspx.cs" Inherits="WebUI_Query_WarehouseCellQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../Css/op.css" />  

    <link rel="stylesheet" type="text/css" href="../../Css/main.css" />  
    <link rel="stylesheet" type="text/css" href="../../ext/packages/ext-theme-crisp/build/resources/ext-theme-crisp-all.css" /> 
    <script type="text/javascript" src="../../JQuery/jquery-1.8.3.min.js"></script>  
    <script type="text/javascript" src= "../../JScript/Common.js"></script>
    <script type="text/javascript" src="../../Ext/ext-all.js"></script>  
    <script type="text/javascript" src="../../Ext/packages/ext-theme-crisp/build/ext-theme-crisp.js"></script>
    <style type="text/css">
        .x-panel-header-title-default
        {
            font-family:微软雅黑;
            font-size:14px;
            font-weight:300;
            line-height:16px;
             
        }
        .x-grid-item
        {
             font-family:微软雅黑;
        }
    </style>
    <script language="javascript" type="text/javascript">
        var ShowCellTag = "<td style=\"width:30px; height:20px; background-color:Red\"></td><td>异常货位</td>" +
                          "<td style=\"width:30px; height:20px; background-color:Blue\"></td><td>有货且未锁定</td>" +
                          "<td style=\"width:30px; height:20px; background-color:Green\"></td><td>有货且锁定</td>" +
                          "<td style=\"width:30px; height:20px; background-color:Gray\"></td><td>禁用货位</td>" +
                          "<td style=\"width:30px; height:20px; background-color:Yellow\"></td><td>锁定的空货位</td>" +
                          "<td style=\"width:30px; height:20px; background-color:Orange\"></td><td>未锁定的托盘</td>" +
                          "<td style=\"width:30px; height:20px; background-color:Gold\"></td><td>锁定的托盘</td>" +
                          "</tr></table>";
        Ext.onReady(function () {
            var blnReload = false;
            var tree;

            var topPanel = {
                region: "north",
                height: 29,
                //bodyStyle: 'border:true;border-width:1px 0 1px 0;background:gray',
                collapsible: false,

                html: '<table style="width: 100%; height: 28px;" cellpadding="0" cellspacing="0"><tr class="maintable"><td style="height:28px" colspan="2" align="right"><input type="button" id="btnNewWarehouse" value="增加仓库" class="HiddenControl" onclick="OpenEditWarehouse()"  /><input type="button" id="btnNewArea" value="增加库区" class="HiddenControl" onclick="OpenEditArea()" /><input type="button" id="btnNewShelf" value="增加货架" class="HiddenControl" onclick="OpenEditShelf()" /><input type="button" id="btnNewCell" value="增加货位" class="HiddenControl" onclick="OpenEditCell()" /><input type="button" id="btnExit" value="退出" class="ButtonExit" onclick="Exit()" /></td></tr></table>'
            };
            var leftPanel = Ext.create('Ext.panel.Panel', {
                region: 'west',
                //title: '仓库资料',
                bodyStyle: 'border:true;border-width:1px 1 1px 1;background:white',
                width: 180,
                minWidth: 90,
                layout: 'fit',
                //split: true,                
                split: {
                    size: 3
                },

                collapsible: false

            });
            var centerPanel = Ext.create('Ext.panel.Panel', {
                region: 'center',
                title: '仓库资料',
                //bodyStyle: 'border:true;border-width:1px 1 1px 1;background:blue',
                layout: "fit",
                plit: true,
                border: true,
                html: '<iframe id="frmMain_warehouse" scrolling="auto" frameborder="0" width="100%" height="100%" src=""> </iframe>',
                collapsible: false
            });
            var buildTree = function (json) {
                return Ext.create('Ext.tree.Panel', {
                    rootVisible: false,

                    title: '仓库资料',
                    border: true,
                    bodyStyle: 'background:white;',
                    store: Ext.create('Ext.data.TreeStore', {
                        root: {
                            expanded: true,
                            children: json.children
                        }
                    }),
                    listeners: {
                        'itemclick': function (view, record, item, index, e) {
                            var tNodeID = record.get('id');
                            var text = record.get('text');
                            var tNodeIDLen = tNodeID.length;
                            var sShelfCode = "";
                            var sAreaCode = "";
                            var sWareHouse = "";

                            centerPanel.setTitle("<table style=\"width:100%;\" valign=\"middle\"><tr><td style=\"width:20%;\">当前选中的节点：" + text + " </td>" + ShowCellTag);
                            $("#hdnNodeID").val(tNodeID);
                            if (tNodeIDLen == 2) {
                                sWareHouse = tNodeID;
                            }
                            if (tNodeIDLen == 3) {
                                sWareHouse = record.parentNode.id;
                                sAreaCode = tNodeID;
                            }
                            if (tNodeIDLen == 6) {
                                sAreaCode = record.parentNode.id;
                                sShelfCode = tNodeID;
                            }
                            $("#frmMain_warehouse").attr("src", "WarehouseCell.aspx?WareHouse=" + sWareHouse + "&AreaCode=" + sAreaCode + "&ShelfCode=" + sShelfCode);

                        },
                        scope: this
                    }
                });
            };
            Ext.Ajax.request({
                url: 'WareHouseTree.ashx',
                async: false,
                method: 'get',
                success: function (response) {
                    if (response.responseText == "-1") {
                        alert('对不起,操作时限已过,请重新登入！');
                        window.opener = null;
                        window.top.location = "../../Login.aspx?Logout=true";

                    }
                    else {
                        var json = Ext.JSON.decode(response.responseText)
                        Ext.each(json, function (el) {
                            //debugger
                            tree = buildTree(el);
                            leftPanel.add(tree);
                        });
                    }

                },
                failure: function (request) {
                    Ext.MessageBox.show({
                        title: '操作提示',
                        msg: "连接服务器失败",
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.ERROR
                    });
                }

            });


            //布局
            var viewport = new Ext.create('Ext.container.Viewport', {
                enableTabScroll: true,
                layout: "border",
                items: [topPanel,
                leftPanel,
              centerPanel
              ]
            });
            if (typeof (tree) != "undefined") {
                var root = tree.getRootNode().firstChild;
                centerPanel.setTitle("<table style=\"width:100%;\" valign=\"middle\"><tr><td style=\"width:20%;\">当前选中的节点：" + root.data.text + " </td>" + ShowCellTag);
                $("#frmMain_warehouse").attr("src", " WarehouseCell.aspx?WareHouse=" + root.id + "&AreaCode=&ShelfCode=");
            }

        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
     <div>
     
     </div>
    </form>
</body>
</html>
