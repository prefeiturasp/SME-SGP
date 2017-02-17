function jsCadastroFrequenciaReuniaoResponsaveis() {

    //Congela a primeira linha e as duas primeiras colunas da table
    $(".tabelaLancamentoFrequencia").width($(".tabelaLancamentoFrequencia").parents("fieldset").width());
    $('#tblNotas').fixer({ fixedrows: 1, fixedcols: 2,parent: ".tabelaLancamentoFrequencia" });
    
    //checa todos as reuniões do aluno
    $(".checkAll input").click(function() {
        var checked_status = $(this).attr('checked');
        $(this).parents('tr').find('.chkFrequencia input').each(function() {
            $(this).attr('checked', checked_status);
        });
    });

    //descheca o "Todas" da respectiva linha quando houver pelo menos uma renião do aluno deschecada
    $(".chkFrequencia input").click(function() {
        var checkeall = true;
        $(this).parents('tr').find('.chkFrequencia input').each(function() {
            if (!($(this).attr('checked')))
                checkeall = false;
        });
        
        $(this).parents('tr').find('.checkAll input').each(function() {
            $(this).attr('checked', checkeall);
        });
    });

    //checa todas as reuniões de todos os alunos
    $(".chkTodas input").click(function() {
        var checked_status = $(this).attr('checked');
        $('div').find('.checkAll input').each(function() {
            $(this).attr('checked', checked_status)
            $(this).parents('tr').find('.chkFrequencia input').each(function() {
                $(this).attr('checked', checked_status)
            });
        });
    });

    //descheca o "Selecionar todos os registros" caso algum chkFrequencia seja deschecado
    $(".chkFrequencia input").click(function() {
        var checkeall = true;
        $('div').find('.chkFrequencia input').each(function() {
            if (!($(this).attr('checked')))
                checkeall = false;
        });

        $('div').find('.chkTodas input').each(function() {
            $(this).attr('checked', checkeall);
        });
    });

    //descheca o "Selecionar todos os registros" caso algum checkAll seja deschecado
    $(".checkAll input").click(function() {
        var checkeall = true;
        $('div').find('.checkAll input').each(function() {
            if (!($(this).attr('checked')))
                checkeall = false;
        });

        $('div').find('.chkTodas input').each(function() {
            $(this).attr('checked', checkeall);
        });
    });

    createDialog('#divCompComparecimento', 600, 0);
}



// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsCadastroFrequenciaReuniaoResponsaveis);
arrFNCSys.push(jsCadastroFrequenciaReuniaoResponsaveis);

arrFNC.push(RemoveNosTextoVazioTabelasIE9);
arrFNCSys.push(RemoveNosTextoVazioTabelasIE9);