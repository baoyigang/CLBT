
/*
<input id="txtNote" onpropertychange="javascript:setMaxLength(this,3);"  />
*/
function setMaxLength(object, length) {
    var result = true;
    var controlid = document.selection.createRange().parentElement().id;
    var controlValue = document.selection.createRange().text;
    if (controlid == object.id && controlValue != "") {
        result = true;
    }
    else if (object.value.length > length) {
        object.value = object.value.substring(0, length);
        result = false;
    }
    if (window.event) {
        window.event.returnValue = result;
        return result;
    }
}