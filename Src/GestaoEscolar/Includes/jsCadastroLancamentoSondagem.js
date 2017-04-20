function jsCadastroLancamentoSondagem() {
    $('input[id$="rbResposta"]').unbind('click').click(function () {
        var idResposta = $(this).attr('name');
        var nomeGrupo = idResposta.substring(idResposta.lastIndexOf('$'));
        $('input[name$="' + nomeGrupo + '"]').each(function () {
            if ($(this).attr('name') != idResposta)
            {
                $(this).removeAttr('checked');
            }
        });
    });

    if ($('#headerBotoes').children().length == 1) {
        $('#headerBotoes').css("display", "none");
    }

    if ($('#headerQuestoes').children().length == 1) {
        $('#headerQuestoes').css("display", "none");
    }

    if ($('#headerSubQuestoes').children().length == 1) {
        $('#headerSubQuestoes').css("display", "none");
    }
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsCadastroLancamentoSondagem);
arrFNCSys.push(jsCadastroLancamentoSondagem);