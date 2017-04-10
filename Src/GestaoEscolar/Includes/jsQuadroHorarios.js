function jsQuadroHorarios() {
    createDialog('.divAtribuirDisciplina', 600, 0);
}

function clickHorario(calEvent) {
    if (calEvent.id.split(';')[5] == 1)
        atribuirDisciplina(calEvent);
    else
        visualizaHorario(calEvent);
}

function atribuirDisciplina(calEvent) {
    $('span#ui-dialog-title-divAtribuirDisciplina').text('Atribuição de disciplina ao horário');
    $('#hdnHorario').val(calEvent.id);

    $('#txtDiaSemana').val($('#hdnDiasSemana').val().split(';')[calEvent.id.split(';')[3] - 1]);

    $('.txtHoraInicial').val(calEvent.start.format('HH:mm'));
    $('.txtHoraFinal').val(calEvent.end.format('HH:mm'));
    $('div.ddlTipoHorario select').val(calEvent.id.split(';')[2]);

    $('div.ddlTurmaDisciplina').show();
    $('#btnAtribuiDisciplina').show();
    $('div.ddlTurmaDisciplina select').val(calEvent.id.split(';')[4]);    

    $(document).ready(function () { $('.divAtribuirDisciplina').dialog('open'); $('.ddlTurmaDisciplina').first().focus(); return false; });
}

function visualizaHorario(calEvent) {
    $('span#ui-dialog-title-divAtribuirDisciplina').text('Visualizar horário');
    $('#txtDiaSemana').val($('#hdnDiasSemana').val().split(';')[calEvent.id.split(';')[3] - 1]);
    $('.txtHoraInicial').val(calEvent.start.format('HH:mm'));
    $('.txtHoraFinal').val(calEvent.end.format('HH:mm'));
    $('div.ddlTipoHorario select').val(calEvent.id.split(';')[2]);
    $('div.ddlTurmaDisciplina').hide();
    $('#btnAtribuiDisciplina').hide();
    
    $(document).ready(function () { $('.divAtribuirDisciplina').dialog('open'); return false; });
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsQuadroHorarios);
arrFNCSys.push(jsQuadroHorarios);