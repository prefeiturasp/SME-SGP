function jsBuscaPlanejamento() {
    createDialog('#divPeriodo', 600, 0);
    createDialog('#divDisciplina', 600, 0);
    createDialog('.divSelecionaDisciplina', 725, 0);
    createDialog('#divSelecionaPeriodica', 725, 0);
    createDialog('#divFiltrosRelatorio', 500, 0);
    createDialog('#divAvaliacoes', 500, 0);
}

function MostraCkbPeriodo(checked) {
    if (checked) {
        $('#divCkb').find('div').first().css({ display: 'block' });
        $(idckbTodasAulas).attr('checked', 'true');
    }
    else {
        $('#divCkb').find('div').first().css({ display: 'none' });
        $(idckbPeriodo).attr('checked', '');
        $(idckbTodasAulas).attr('checked', '');
        $('#divInicioFim').find('div').first().css({ display: 'none' });
        $(idTxtInicio).attr('value', '');
        $(idTxtFim).attr('value', '');
    }
}
function MostraTextBox(checked) {
    if (checked) {
        $('#divInicioFim').find('div').first().css({ display: 'block' });
        $(idckbTodasAulas).attr('checked', '');
    }
    else {
        $(idckbPeriodo).attr('checked', 'true');
    }
}

function CheckTodasAulas(checked) {
    if (checked) {
        $('#divInicioFim').find('div').first().css({ display: 'none' });
        $(idTxtInicio).attr('value', '');
        $(idTxtFim).attr('value', '');
        $(idckbPeriodo).attr('checked', '');
    }
    else {
        $(idckbTodasAulas).attr('checked', 'true');
    }
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsBuscaPlanejamento);


