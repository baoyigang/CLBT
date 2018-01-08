$(document).ready(function () {
    //$(window).resize(content_resize);
    //content_resize();
});
var resize = false;

//Detail1_sData
var windowresize = window.onresize;
function _resize(e) {
    if (windowresize != null) windowresize(e);
    var o1, o2;
    o1 = $("#div_Detail1")[0];
    if (o1 != null) {
        if (o1.offsetWidth != 0) {
            o2 = $("#Detail1_sData");
            o2.css("width", o1.offsetWidth - o2[0].offsetLeft);
            // $("#Detail1_sHeaderInner").css("right", o2[0].scrollLeft);    
        }
    }
    o1 = $("#div_Detail2")[0];
    if (o1 != null) {
        if (o1.offsetWidth != 0) {
            o2 = $("#Detail2_sData");
            o2.css("width", o1.offsetWidth - o2[0].offsetLeft);
        }
    }

    o1 = $("#div_Detail3")[0];
    if (o1 != null) {
        if (o1.offsetWidth != 0) {
            o2 = $("#Detail3_sData");
            o2.css("width", o1.offsetWidth - o2[0].offsetLeft);
        }
    }

    //    o1 = $("#div_Detail4")[0];
    //    if (o1 != null) {
    //        if (o1.offsetWidth != 0) {
    //            o2 = $("#Detail4_sData");
    //            o2.css("width", o1.offsetWidth - o2[0].offsetLeft);
    //        } 
    //    }

    //    o1 = $("#div_Detail5")[0];
    //    if (o1 != null) {
    //        if (o1.offsetWidth != 0) {
    //            o2 = $("#Detail5_sData");
    //            o2.css("width", o1.offsetWidth - o2[0].offsetLeft);
    //        } 
    //    }
}
$(document).mouseup(
				function (a) {
				    if (a.target.className == "x-tab-strip-text ")
				        _resize(a);
				    //alert($(a.target).attr("id"));
				    //				    if (n[0].offsetLeft == 0) abc();
				    //				    if ($("#" + c.controlId + ":hidden").length != 0) return;
				    //				    if ($(a.target).attr("id") == (n.attr("id") + "date_calendardate_calendar1")) {
				    //				        return;
				    //				    }
				    //				    if (($(a.target).parentsUntil("#" + c.controlId).parent().length != 0 && $(a.target).parentsUntil("#" + c.controlId).parent()[0].id == c.controlId))
				    //				        return;
				    //				    $("#" + c.controlId).hide();
				}
					);

//div_Detail1
window.onresize = function (e) {
    _resize(e);
}

function content_resize() {

    //編輯頁面 div高度
    var div = $("#surdiv");
    var h = 300;
    if ($(window).height() <= 0) {
        h = document.body.clientHeight - 50;
    }
    else {
        h = $(window).height() - 50;
    }
    $("#surdiv").css("height", h);

    var grid = $("#GridView1");
    var h = 300;
    if ($(window).height() <= 0) {
        if (grid.offset() != null)
            h = document.body.clientHeight - grid.offset().top - 50;
        else
            h = document.body.clientHeight - 50;
    }
    else {
        if (grid.offset() != null)
            h = $(window).height() - grid.offset().top - 50;
        else
            h = document.body.clientHeight - 50;
    }
    $("#table-container").css("height", h);
    resize = true;
}
function content_resize2() {
    var grid = $("#GridView1");
    var h = 300;
    if ($(window).height() <= 0) {
        if (grid.offset() != null)
            h = document.body.clientHeight - grid.offset().top;
        else
            h = document.body.clientHeight;
    }
    else {
        if (grid.offset() != null)
            h = $(window).height() - grid.offset().top;
        else
            h = document.body.clientHeight;
    }
    $("#table-container").css("height", h);
}
function treeview_resize() {

    var h = 300;
    if ($(window).height() <= 0) {
        h = document.body.clientHeight - $("#toptable")[0].offsetHeight - 50;
    }
    else {
        h = document.body.clientHeight - $("#toptable")[0].offsetHeight - 50;
    }

    $("#div_tree").css("height", h);
//    var ht = h - 160;
//    $("#table-container").css("height", ht);
}
function treeview_resize2() {

    var h = 300;
    if ($(window).height() <= 0) {
        h = document.body.clientHeight - 60;
    }
    else {
        h = document.body.clientHeight - 60;
    }
    $("#TreeView1").css("height", h);
    $("#table-container").css("height", h);
}

function contentTwoGrid_resize() {
    //編輯頁面 div高度
    var div = $("#surdiv");
    var h = 300;
    if ($(window).height() <= 0) {
        h = document.body.clientHeight - 23;
    }
    else {
        h = $(window).height() - 23;
    }
    $("#surdiv").css("height", h);

    var grid = $("#gvMain");
    var h = 300;
    if ($(window).height() <= 0) {
        if (grid.offset() != null)
            h = document.body.clientHeight/2 - grid.offset().top - 35;
        else
            h = document.body.clientHeight/2 - 35;
    }
    else {
        if (grid.offset() != null)
            h = $(window).height()/2 - grid.offset().top - 35;
        else
            h = document.body.clientHeight/2 - 5;
    }
    $("#Main-container").css("height", h);
  
    $("#Sub-container").css("height", h);
    $("#Sub-container1").css("height", h);
    $("#Sub-container2").css("height", h);
    resize = true;
}

function contentThreeGrid_resize() {
    //編輯頁面 div高度
    var div = $("#surdiv");
    var h = 300;
    if ($(window).height() <= 0) {
        h = document.body.clientHeight - 23;
    }
    else {
        h = $(window).height() - 23;
    }
    $("#surdiv").css("height", h);

    var grid = $("#gvMain");
    var h = 300;
    if ($(window).height() <= 0) {
        if (grid.offset() != null)
            h = document.body.clientHeight / 2 - grid.offset().top - 35;
        else
            h = document.body.clientHeight / 2 - 35;
    }
    else {
        if (grid.offset() != null)
            h = $(window).height() / 2 - grid.offset().top - 35;
        else
            h = document.body.clientHeight / 2 - 5;
    }
    $("#Main-container").css("height", h);

    $("#Sub-container").css("height", h);

    resize = true;
}