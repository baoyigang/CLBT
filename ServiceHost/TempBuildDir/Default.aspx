<%@ page language="C#" autoeventwireup="true" inherits="_Default, WMS_deploy" theme="Default" stylesheettheme="Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <title>自动化仓储管理系统</title>
    <link rel="stylesheet" type="text/css" href="Css/icon.css" />  
    <link rel="stylesheet" type="text/css" href="Css/default.css" />  
   
    <script type="text/javascript" src="JQuery/jquery-1.8.3.min.js"></script>  
    <script type="text/javascript" src= "JScript/Common.js"></script>
    <script type="text/javascript" src="Ext/ext-all.js"></script>  
    <script type="text/javascript" src="Ext/packages/ext-theme-crisp/build/ext-theme-crisp.js"></script>
     <link rel="stylesheet" type="text/css" href="ext/packages/ext-theme-crisp/build/resources/ext-theme-crisp-all.css" /> 
<%--    <link rel="stylesheet" type="text/css" href="ext/packages/ext-theme-classic/build/resources/ext-theme-classic-all.css" />  
    <script type="text/javascript" src="Ext/ext-all.js"></script>  
    <script type="text/javascript" src="Ext/packages/ext-theme-classic/build/ext-theme-classic.js"></script>--%>
<%--    <link rel="stylesheet" type="text/css" href="ext/packages/ext-theme-gray/build/resources/ext-theme-gray-all.css" />  
    <script type="text/javascript" src="Ext/ext-all.js"></script>  
    <script type="text/javascript" src="Ext/packages/ext-theme-gray/build/ext-theme-gray.js"></script>--%>
    <style type="text/css">
        .x-panel-header-title
        {
            font-family:微软雅黑;
            font-size:14px;
            font-weight:300;
            line-height:16px;
        }
       .x-accordion-hd .x-panel-header-title
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
         .x-tab-inner-default
        { 
            font-family:微软雅黑;
            font-size:14px;
        }
        .x-accordion-item .x-accordion-hd
        {
        	 background:#EAF2F4;
        	 border-width:0 0 1px;
        	 border-color:White #cecece #cecece;
        	 padding:8px 10px;
        	 color:Red;
        }
        .x-tree-icon
        {
            height:20px;
            width:16px;
            vertical-align:bottom;
        }
       .x-accordion-hd .x-tool-img
       {
           background-color:#EAF2F4;
       }
       
    </style>

    <script language="javascript" type="text/javascript">
        var tabPanel;
        function removetab() {
            var tab = tabPanel.getActiveTab();
            tabPanel.remove(tab);
        }
        function ChangePassword() {
            addTab(33, 'WebUI/SysInfo/pwdModify/PwdModify.aspx?t=now', '密码修改');
            return false;
        }
        function addTab(id, url, title) {
            var exist = false;
            tabPanel.items.each(function (item) {
                if (item.title == title) {
                    var tab = Ext.getCmp(item.id);
                    tabPanel.setActiveTab(tab);
                    exist = true;
                    return;
                }
            });
            if (!exist) {
                tabPanel.add({
                    id: 'tab_' + id,
                    title: title,
                    html: '<iframe id="frmMain_' + id + '" scrolling="auto" frameborder="1" width="100%" height="100%" src="' + url + '"> </iframe>',
                    closable: true
                }).show();
            }
        }
        Ext.onReady(function () {
            var topPanel = {
                region: "north",
                height: 83,
                title: '', //罗伯泰克自动化科技(苏州)有限公司
                bodyStyle: 'border:true;border-width:1px 0 1px 0;background:#d8d8d8',
                collapsible: false,

                html: '<div class="header"><table width="100%" height="80" border="0" cellpadding="0" cellspacing="0"  background="Images/top_bg.jpg" style="top: 0px; z-index: inherit"><tr><th align="left" valign="top" scope="col" style="height:80px; background-repeat:no-repeat; width: 100%;" background="Images/banner_1.jpg"><div class="topNav"><a href="Login.aspx" id="changeUser">切换用户</a><span>|</span><a href="javascript:void(0)" onclick="return ChangePassword();" id="changePassword">修改密码</a><span>|</span><a href="javascript:window.close();"/" id="loginOut">退出</a></div></th></tr></table></div>'
            };

            var leftPanel = Ext.create('Ext.panel.Panel', {
                region: 'west',
                title: '导航栏',
                bodyStyle: 'border:true;border-width:1px 1 1px 1;background:blue',
                width: 230,
                minWidth: 90,
                //split: true,                
                split: {
                    size: 3
                },

                collapsible: true,
                //collapseMode: 'mini',
                defaults: {
                    // applied to each contained panel
                    //bodyStyle: 'padding:15px'
                },
                layout: {
                    // layout-specific configs go here
                    type: 'accordion',
                    //titleCollapse: false,
                    animate: true
                    //activeOnTop: true
                }
            });


            /** 
            * 组建树 
            */
            var buildTree = function (json) {
                return Ext.create('Ext.tree.Panel', {
                    rootVisible: false,
                    border: false,
                    store: Ext.create('Ext.data.TreeStore', {
                        root: {
                            expanded: true,
                            children: json.children
                        }
                    }),
                    listeners: {
                        'itemclick': function (view, record, item, index, e) {
                            var id = record.get('id');
                            var text = record.get('text');
                            var url = record.get('url');

                            if (record.childNodes.length > 0)
                                return;

                            addTab(id, url, text)
                        },
                        scope: this
                    }
                });
            };

            /** 
            * 加载菜单树 
            */
            Ext.Ajax.request({
                url: 'LeftTreeJson.ashx',
                async: false,
                method: 'get',
                success: function (response) {
                    var json = Ext.JSON.decode(response.responseText)
                    Ext.each(json, function (el) {
                        // debugger;
                        var panel = Ext.create('Ext.panel.Panel', {
                            //id: el.id, id不能加，加了会出错
                            title: el.text,
                            layout: 'fit'
                        });

                        var treePanel = buildTree(el);
                        panel.add(treePanel);

                        leftPanel.add(panel);
                    });
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
            //center
            tabPanel = new Ext.TabPanel({
                region: "center",
                layout: "fit",
                plit: true,
                border: true,
                id: "main",
                enableTabScroll: true,
                deferredRender: false,
                activeTab: 0,
                items: [{
                    title: '首页',
                    html: '<iframe id="frmMain" scrolling="auto" frameborder="1" width="100%" height="100%" src="Index/Main.aspx"> </iframe>'
                }]
            });
            //south
            var bottomPanel = {
                region: "south",
                //bodyStyle: 'border:1px solid #FF0000',
                height: 30,
                titleAlign: null,
                //bodyStyle: 'background:#cbe4f3',<a href="http://www.unitechnik.com.cn" target="_blank">罗伯泰克自动化科技（苏州）有限公司</a>
                html: '<div style="border:1px solid #cbe4f3;line-height:30px;text-align: center;background:url(Images/bottom.jpg) repeat-x;"></div>'

            };
            //布局
            var viewport = new Ext.create('Ext.container.Viewport', {
                enableTabScroll: true,
                layout: "border",
                items: [
                topPanel,
                leftPanel,
                //accordion2,
              tabPanel,
              bottomPanel]
            });
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
