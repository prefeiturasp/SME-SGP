function jsBuscaAlunos() {
    createDialog('#divBoletimCompleto', "90%", 0);
    createDialog('#divObservacaoSituacao', 550);

    $('#divObservacaoSituacao').unbind('dialogopen').bind('dialogopen', function (type, data) {
        $(this).parent().prependTo($("#aspnetForm"));

        if (!(($.browser.msie) && ($.browser.version <= 7))) {
            $(this).dialog("option", "position", "center");
            $(window).scroll(function () {
                $(this).dialog("option", "position", "center");
            });
        }

        $(this).find('textarea').attr('value', '').focus();
    });
}

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

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsBuscaAlunos);
arrFNCSys.push(jsBuscaAlunos);
