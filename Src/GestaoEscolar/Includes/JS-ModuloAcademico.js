function jsModuloAcademico() {
    createDialog('#divParametroAcademico', 800, 0);

    try {
        $('#divParametroAcademico').unbind('dialogopen');
    }
    catch (e) {
    }

    $('#divParametroAcademico').bind("dialogopen", function(event, ui) {
        try {
            $('.ui-dialog-titlebar-close').unbind('click');
        }
        catch (e) {
        }
        
        $('.ui-dialog-titlebar-close').click(function() {
            $("#" + $("input[id$='_btnCancelar']")[0].id).trigger('click');
            return false;
        });
    });

    $('.currency').unbind('blur').bind('blur', function () {
            var nota = parseFloat($(this).attr('value').replace(',', '.'));
            nota = nota.toFixed(2);
            $(this).attr('value', nota == 'NaN' ? "" : nota.replace('.', ','));
    });

    createDialog('#divParametroBuscaAluno', 555, 0);    
    createDialog('#divParecer', 400, 0);
    createDialog('#divTurnoHorario', 450, 300);
    createDialog('#divImportarTurnoHorario', 800, 600);
    createDialog('#divAvaliacao', 600, 300);
    createDialog('#divAvaliacaoRelacionada', 450, 0);
    createDialog('#divCalendario', 555, 200);
    createDialog('#divItemAvaliacao', 350, 250);
    createDialog('#divDataAnterior', 500, 0);
    createDialog('#divAulaEventoSemAtividade', 500, 0);
    createDialog('#divBuscaDocente', 755, 0);
}
// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsModuloAcademico);
arrFNCSys.push(jsModuloAcademico);
