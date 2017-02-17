function ExpandCollapse(divID, btn) {
    var div = $(btn).parents("tr").next(divID);

    if ($(div).css("display") == "none") {
        $(btn).removeClass('ui-icon ui-icon-circle-triangle-s');
        $(btn).addClass('ui-icon ui-icon-circle-triangle-n');

        $(div).css("display", "");
    }
    else {
        $(btn).removeClass('ui-icon ui-icon-circle-triangle-n');
        $(btn).addClass('ui-icon ui-icon-circle-triangle-s');

        $(div).css("display", "none");
    }
}
