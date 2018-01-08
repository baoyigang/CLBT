function JsDDL_changetitle(ddlId, textBoxId) {
    var drp = document.all(ddlId);
    var t = document.all(textBoxId);
    var j = 0;
    if (t.value == "") {
    }
    else {
        for (var i = 0; i < drp.length; i++) {
            if (t.value == drp.options(i).text) {
                drp.value = drp.options(i).value;
                break;
            }
            else {
                j = j + 1;
            }
        }
        if (j == drp.length) {
            var tOption = document.createElement("Option");
            tOption.text = t.value;
            tOption.value = t.value;
            drp.add(tOption);
        }
        drp.value = t.value;
    }
    t.parentNode.value = t.value;
}
//前臺附值手動刷新
function RefreshDDlVal(o) {
    if (o.value == null) return;
    o.children[1].value = o.value;
    JsDDL_changetitle(o.children[0].id, o.children[1].id);
}
function ChangePhoneAdress(obj1,obj2) {
    var ddl = obj1.id.split("_");
    var ddl2 = obj2.id.split("_");
    if (ddl[0] == "txtLinkPerson") {
        getWhereBaseData('CMD_CUSTOMERSUB', 'txtLinkPhone_' + ddl2[1] + ',txtLinkAddress_'+ddl2[1], 'PHONE,Address', "PERSON='" + $('#' + obj1.id).val() + "' and CUSTOMER_CODE='" + $('#txtCustomerID').val() + "'");
    }
}

jQuery.fn.extend({
    DDLDataSoure: function (r) {
        this[0].children[0].id
        //    var r = ["1測試", "2測試", "3測試", "4測試"];
        var drp = this[0].children[0];
        drp.innerHTML = "";
        var tOption = document.createElement("Option");
        tOption.text = "";
        tOption.value = "";
        drp.add(tOption);
        for (var i = 0; i < r.length; i++) {
            var tOption = document.createElement("Option");
            tOption.text = r[i];
            tOption.value = r[i];
            drp.add(tOption);
        }
    }
});
//js綁定DDLDataSoure
//$(document).ready(function () {
//    $("#txtDeliverCountry").DDLDataSoure(["1測試", "2測試", "3測試", "4測試"]);
//});