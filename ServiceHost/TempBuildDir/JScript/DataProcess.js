

//获取单个数据，并绑定数据集。

function GetOtherValue(XmlTableName, ctlName, fldName, strWhere) {
    var strReturn = showdialog(XmlTableName, strWhere, 0);
    if (strReturn != null && strReturn != "undefined") {
        if (arguments[4] != undefined) {
            $("#" + arguments[4]).val(strReturn);
        }
        var ReturnValue = jQuery.parseJSON(strReturn[0]);
        var obj = ctlName.split(',');
        var fld = fldName.split(',');
        for (var i = 0; i < obj.length; i++) {
            var currValue = unescape(ReturnValue[0][fld[i]]);
            //            if ($("#" + obj[i])[0].tagName == "SPAN") {
            //                $("#" + obj[i]).html(currValue);
            //                $("#" + obj[i]).val(currValue);
            //            }
            //            else {
            $("#" + obj[i]).val(currValue);
            //            }
        }
    }
}
//无返回值时，清空原有值。
function GetOtherValueNullClear(XmlTableName, ctlName, fldName, strWhere) {
    var strReturn = showdialog(XmlTableName, strWhere, 0);
    if (strReturn != null && strReturn != "undefined" ) {
        if (arguments[4] != undefined) {
            $("#" + arguments[4]).val(strReturn);
        }
        var ReturnValue = jQuery.parseJSON(strReturn[0]);
        var obj = ctlName.split(',');
        var fld = fldName.split(',');
        for (var i = 0; i < obj.length; i++) {
            var currValue = unescape(ReturnValue[0][fld[i]]);
            $("#" + obj[i]).val(currValue);
        }
    }
    else {
        var obj = ctlName.split(',');
        for (var i = 0; i < obj.length; i++) {
            $("#" + obj[i]).val('');
        }

    }
}   
 //顯示(單/多)選對話框 //返回值包含﹒欄位名稱+內容;傳回所有值
 //sysid.length>=2;  第一位表示(0:單選／1:多選);第二位表示(系統sysid);
 //PermissionID:系統表單id
 //'@|$' 表示 <<|>>; @,# 表示 <<,>> @^&  表示 <<'>>
function showdialog(XmlTableName, strWhere, Option) {
    var returnvalue;
    var Where;

    if (strWhere == null)
        Where = '';
    else
        Where = escape(strWhere);
    
    returnvalue = window.showModalDialog('../../Common/Select.aspx?TableName=' + XmlTableName + '&Option=' + Option + '&Where=' + Where, window, 'DialogHeight:400px;DialogWidth:900px;help:no;scroll:no;location:no;Resizable:yes;');
    //alert(returnvalue);
    if (returnvalue != "undefined" && returnvalue != null && returnvalue!="[]") {
        if (returnvalue != "") {
            var t1 = returnvalue.replace(/&nbsp;/g, "");
            var t2 = t1.replace(/&quot;/g, '"');
            var t3 = t2.replace(/&lt;/g, '<');
            var t4 = t3.replace(/&gt;/g, '>');
            var t5 = t4.replace("@^&", "'");
            var t6 = t5.replace(/&amp;/g, '&');
            return t6.split("@,#");
        }
        else
            return null;
    }
    else {
        return null;
    }
}

function GetMulSelectValue(XmlTableName, objName, strWhere) {
    
    $("#" + objName).val("");
    //showdlg("/KC_Forms/Sys/Select.aspx?SID=&PID=", "", 300, 180); window.returnValue
    var strReturn = showdialog(XmlTableName, strWhere, 1);
    if (strReturn == null || strReturn.length == 0) {
        return false; 
     }
    $("#" + objName).val(strReturn);
    return true;
}

//報表多選共用函數; KeyField:要查询的欄位名稱,如DepartmentID; ButtonValue:多選按鈕Name,如"#btnMulSel1";  HdnValue:回傳回去的隱藏控件Name,如"#hdnMulSel1"
function getMultiItems(FormID, KeyField, ButtonValue, HdnValue, strWhere) {
    if ($(ButtonValue)[0].value == "取消指定") {
        $(HdnValue).val("");
        $(ButtonValue)[0].value = "指定";
        return;
    }
    var sValue = "";
    var Where;
    if (strWhere == null)
        Where = '';
    else
        Where = strWhere;
    var returnvalue = showdialog(FormID, strWhere, 1);
    if (returnvalue != "" && returnvalue != null && returnvalue != "undefined") {//json數據，處理賦值時，必須用unescape()解碼。
        var strReturn = jQuery.parseJSON(returnvalue[0]);
    }
    if (strReturn != null) {
        for (var i = 0; i < strReturn.length; i++) {
            if (sValue == "")
                sValue += "'" + escape(strReturn[i][KeyField]) + "'";
            else
                sValue += ",'" + escape(strReturn[i][KeyField]) + "'";
        }
    }
    //按鈕顏色變紅色
    if (sValue == "")
        $(ButtonValue)[0].value = "指定";
    else
        $(ButtonValue)[0].value = "取消指定";
    $(HdnValue).val(sValue);
}

 






//根据条件获取值。
function getWhereBaseData(TableName, ctlName, fieldName) {
    var strWhere = '';
    if (arguments[3] != undefined) {
        strWhere = arguments[3];
    }
    var row = new Object();

    row.strWhere = strWhere;
    row.strFieldName = fieldName;
    row.TableName = TableName;
    var returnvalue = Ajax("strBaseData", row);
    var ctl = ctlName.split(",");
    var fl = fieldName.split(",");

    if (returnvalue.length > 0 && returnvalue != "[]") {
        var strReturn = jQuery.parseJSON(returnvalue);
        for (var i = 0; i < ctl.length; i++) {
            currValue = unescape(strReturn[0][fl[i]].toString());
            $("#" + ctl[i]).val(currValue);
        }
    }
    else {
        for (var i = 0; i < ctl.length; i++) {
            $("#" + ctl[i]).val('');
        }
    }


}
//获取WebServices中的方法。
function Ajax(code, para) {
    //创建异步对象
    var xmlhttp = createxmlhttp();
    if (xmlhttp == null) {
        alert("你的浏览器不支持 XMLHttpRequest");
        return;
    }
    //同步
    xmlhttp.open("POST", HostPath + "WebServices/BaseService.asmx", false); //如果是异步通信方式(true)，客户机就不等待服务器的响应
//    xmlhttp.open("POST", getRootPath() + "/WebServices/BaseService.asmx", false); //如果是异步通信方式(true)，客户机就不等待服务器的响应
    xmlhttp.setRequestHeader("Content-Type", "application/soap+xml");
    var strPara= parseXmlPara(para);
    xmlhttp.send(getData(code, strPara));

    return getResult(xmlhttp);
}
function parseXmlPara(para) {
    if (para == null) return "";
    return "<xmlpara>" + parseJosn(para) + "</xmlpara>";
}
function createxmlhttp() {
    //创建异步对象
    var xmlhttp = window.XMLHttpRequest ? new window.XMLHttpRequest() : new ActiveXObject("Microsoft.XMLHttp");
    return xmlhttp;
}
function getResult(xmlhttp) {
    var result = ""; //result.toUpperCase
    result = xmlhttp.responseText;
    //        result.indexOf("<" + code + "Result>");
    //        result.indexOf("</" + code + "Result>");
    var type = result.substring(result.indexOf("<type>") + 6, result.indexOf("</type>"))
    kcajax_rowcount = result.substring(result.indexOf("<rows>") + 6, result.indexOf("</rows>"))
    if (result.indexOf("<data>") == -1) return "";
    result = result.substring(result.indexOf("<data>") + 6, result.indexOf("</data>"))
    result = $.trim(result).replace(/&amp;/g, '&');
    
    //    if (xmlhttp.responseXML.documentElement.nodeTypedValue == null) {
    //        //alert("11");
    //        //Chrome
    //        result = xmlhttp.responseText;
    ////        result.indexOf("<" + code + "Result>");
    //        //        result.indexOf("</" + code + "Result>");
    //        var type = result.substring(result.indexOf("<type>") + 6, result.indexOf("</type>"))
    //        result = result.substring(result.indexOf("<data>") + 6, result.indexOf("</data>"))
    //        
    //    } else {
    //        result = xmlhttp.responseXML.documentElement.nodeTypedValue;
    //    }

    //    System.Int16;
    //    System.Int32;
    //    System.Int64;
    //    System.DateTime;
    //    System.Decimal;
    //    System.Boolean;
    //    System.Byte;

    switch (type) {
        case "System.Byte":
        case "System.Int16":
        case "System.Int32":
        case "System.Int64":
            return parseInt(result);
        case "System.Decimal":
            return parseFloat(result);
        case "System.DateTime":
            return new Date(result);
        case "System.Boolean":
            return result == "True";
        case "ErrMsg":
            alert(result);
            return null;
        case "null":
            return null;
    }
    return result;
}
function parseJosn(para) {
    return "[{" + getJosnRowStr(para) + "}]";
}
function getJosnRowStr(para) {
    if (para == null) return "";
    var strpara = "";
    for (key in para) {
        //alert(key);
        if (strpara.length == 0)
            strpara = strpara + "\"" + key + "\":\"" + para[key] + "\"";
        else
            strpara = strpara + ",\"" + key + "\":\"" + para[key] + "\"";
    }
    return strpara;
}
function getData(code, strpara) {
    //在这处我们拼接
    var data;
    data = '<?xml version="1.0" encoding="utf-8"?>';
    data = data + '<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">';
    data = data + '<soap12:Body>';
    data = data + '<' + code + ' xmlns="http://tempuri.org/" >';
    if (strpara != null) {
        data = data + strpara;
        //strpara 例
        //無參
        //code() 
        //null
        //有參
        //code(str1,str2)
        //<str1>你好1</str1>
        //<str2>你好2</str2>        
    }
    data = data + '</' + code + '>'
    data = data + '</soap12:Body>';
    data = data + '</soap12:Envelope>';
    return data;
}
//js获取网站根路径(站点及虚拟目录)，获得网站的根目录或虚拟目录的根地址
function getRootPath() {
    var strFullPath = window.document.location.href;
    var strPath = window.document.location.pathname;
    var pos = strFullPath.indexOf(strPath);
    var prePath = strFullPath.substring(0, pos);
    var postPath = strPath.substring(0, strPath.substr(1).indexOf('/') + 1);
    return (prePath + postPath);
}



//單號自動編號
function autoCode(PreName, Filter, ctrl_dateteime) {
    var dtTime = $("#" + ctrl_dateteime + "_txtDate").attr('datevalue');
    var row = new Object();

    row.PreName = PreName;
    row.dtTime = dtTime;
    row.Filter = Filter;
    return Ajax("autoCode", row);
}

function autoCodeByTableName(PreName, Filter,TableName, ctrl_dateteime) {
    var dtTime = $("#" + ctrl_dateteime + "_txtDate").attr('datevalue');
    var row = new Object();

    row.PreName = PreName;
    row.dtTime = dtTime;
    row.TableName = TableName;
    row.Filter = Filter;
    return Ajax("autoCodeByTableName", row);
}



 

 



























 
 
function updateAjax(strSQL,isSysData) {
    $.ajax({
        type: "Post",
        url: "../../WebService/UpdateData.asmx/Update",
        data: "{'strSQL':'" + escape(strSQL) + "','isSysData':'" + isSysData + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(data) {
            debugger;
            //你可以 alert(data.d)看数据返回的格式
            data = jQuery.parseJSON(data.d);
            
        },
        error: function(err) {
            alert(err + "err");
        }
    });

}
function DateDiff(sDate1, sDate2) { //sDate1和sDate2是2002-12-18格式      
    var aDate, oDate1, oDate2, iDays;
    aDate = sDate1.split("/");
    oDate1 = new Date(aDate[0], aDate[1] - 1, aDate[2]);
    aDate = sDate2.split("/");
    oDate2 = new Date(aDate[0], aDate[1] - 1, aDate[2]);

    iDays = parseInt(Math.abs(oDate1 - oDate2) / 1000 / 60 / 60 / 24);
    if ((oDate1 - oDate2) < 0) {
        return -iDays;
    }
    return iDays;
}
//多選單擊取消多選,改變Button顏色
//ButtonValue:多選按鈕Name,如"#btnMulSel1";  HdnValue:回傳回去的隱藏控件Name,如"#hdnMulSel1"
function ColClick(ButtonValue, HdnValue) {
    $(ButtonValue)[0].style.color = "Black";
    $(HdnValue).val("");
}
function ColClick1(ButtonValue, HdnValue) {
    $(ButtonValue)[0].color = "#000";
    $(HdnValue).val("");
}


//聯動地址編輯
//例子 editAddress(obj, zip) or editAddress(obj)
function editAddress(obj, zip) {
    if (obj.children.length != 0)
        obj = obj.children[1];
    returnvalue = window.showModalDialog('../../TempAddress.aspx', window, 'dialogWidth:500px;dialogHeight:200px;help:no;status:no;scroll:auto;Resizable:yes;');
    if (returnvalue != null) {
        var part = returnvalue.split("@#$");
        if (obj) obj.value = part[0];
        if (zip) zip.value = part[1];
    }
}

function GetDetailWhereValue(FormID, cnKey,KeyField,TableName, objName, ctlName) { //可傳遞where條件，為第四個參數
    var where = '';
    if (arguments[6] != null)
        where = arguments[6];


    var strReturn = showDetailWindow(FormID, cnKey, 0, KeyField, where, TableName);
    SetCtrlValue(strReturn, objName, ctlName);
}

function showDetailWindow(FormID, cnKey, option, KeyField, strWhere, TableName) {
    var returnvalue;
    var Where;

    if (strWhere == null)
        Where = '';
    else
        Where = strWhere;
    var returnvalue = window.showModalDialog('../../WebUI/Label/TempVolumeSearchPage.aspx?FormID=' + FormID + '&cnKey=' + cnKey + '&option=' + option + '&KeyField=' + KeyField + '&Where=' + escape(Where) + '&TableName=' + TableName, window, 'dialogWidth:800px;dialogHeight:520px;help:no;status:no;scroll:auto;Resizable:yes;');

    if (returnvalue != "undefined" && returnvalue != null) {
        if (returnvalue != "") {//json數據，處理賦值時，必須用unescape()解碼。
            //
            return jQuery.parseJSON(returnvalue);
        }
        else
            return null;
    }
    else {
        return null;
    }
}
function getDetailMultiItems(FormID, cnKey, option, KeyField, strWhere, TableName, ButtonValue, HdnValue) {
    if ($(ButtonValue)[0].value == "取消指定") {
        $(HdnValue).val("");
        $(ButtonValue)[0].value = "指定";
        return;
    }
    var sValue = "";
    var Where;
    if (strWhere == null)
        Where = '';
    else
        Where = strWhere;
    var returnvalue = window.showModalDialog('../../WebUI/Label/TempVolumeSearchPage.aspx?FormID=' + FormID + '&cnKey=' + cnKey + '&option=' + option + '&KeyField=' + KeyField + '&Where=' + escape(Where) + '&TableName=' + TableName, window, 'dialogWidth:800px;dialogHeight:520px;help:no;status:no;scroll:auto;Resizable:yes;');
    if (returnvalue != "") {//json數據，處理賦值時，必須用unescape()解碼。
        var strReturn = jQuery.parseJSON(returnvalue);

        if (strReturn != null) {
            for (var i = 0; i < strReturn.length; i++) {
                if (sValue == "")
                    sValue += "'" + escape(strReturn[i][KeyField]) + "'";
                else
                    sValue += ",'" + escape(strReturn[i][KeyField]) + "'";
            }
        }
        //按鈕顏色變紅色
        if (sValue == "")
            $(ButtonValue)[0].value = "指定";
        else
            $(ButtonValue)[0].value = "取消指定";
        $(HdnValue).val(sValue);
    } 
}
