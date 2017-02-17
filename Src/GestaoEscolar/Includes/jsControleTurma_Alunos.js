function jsControleTurma_Alunos() {
    createDialog('#divBoletimCompleto', "90%", 0);

    $(".btnMensagemUnload").unbind("click").bind("click", function () {
        if (typeof MinutosCacheFechamento != "undefined")
            if (MinutosCacheFechamento > 0) {
                PreCarregarCacheFechamento();
            }
    });
}

function RetornaDados(item) {
    return {
        "tud_id": item.find('input[name$="hdnTudId"]').val(),
        "tur_id": item.find('input[name$="hdnTurId"]').val(),
        "tpc_id": item.find('input[name$="hdnTpcId"]').val(),
        "ava_id": item.find('input[name$="hdnAvaId"]').val(),
        "fav_id": item.find('input[name$="hdnFavId"]').val(),
        "tipoAvaliacao": item.find('input[name$="hdnTipoAvaliacao"]').val(),
        "esa_id": item.find('input[name$="hdnEsaId"]').val(),
        "tipoEscalaDisciplina": item.find('input[name$="hdnTipoEscala"]').val(),
        "tipoEscalaDocente": item.find('input[name$="hdnTipoEscalaDocente"]').val(),
        "notaMinimaAprovacao": item.find('input[name$="hdnNotaMinima"]').val(),
        "ordemParecerMinimo": item.find('input[name$="hdnParecerMinimo"]').val(),
        "tipoLancamento": item.find('input[name$="hdnTipoLancamento"]').val(),
        "fav_calculoQtdeAulasDadas": item.find('input[name$="hdnCalculoQtAulasDadas"]').val(),
        "tur_tipo": item.find('input[name$="hdnTurTipo"]').val(),
        "cal_id": item.find('input[name$="hdnCalId"]').val(),
        "tud_tipo": item.find('input[name$="hdnTudTipo"]').val(),
        "tpc_ordem": item.find('input[name$="hdnTpcOrdem"]').val(),
        "fav_variacao": item.find('input[name$="hdnVariacao"]').val(),
        "tipoDocente": item.find('input[name$="hdnTipoDocente"]').val(),
        "disciplinaEspecial": item.find('input[name$="hdnDisciplinaEspecial"]').val(),
        "permiteAlterarResultado": permiteAlterarResultado,
        "exibirNotaFinal": exibirNotaFinal,
        "ExibeCompensacao": ExibeCompensacao,
        "MinutosCacheFechamento": MinutosCacheFechamento,
        "fechamentoAutomatico": item.find('input[name$="hdnFechamentoAutomatico"]').val(),
        "processarFilaFechamentoTela": item.find('input[name$="hdnProcessarFilaFechamentoTela"]').val()
    }
}

function PreCarregarCacheFechamento() {
    $.ajax({
        type: "GET",
        url: "CacheEfetivacaoHandler.ashx",
        data: function () {
            var item = $("#divDadosFechamento");
            return RetornaDados(item);
        }(),
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json"/*,
        beforeSend: function () {
            $(".loader").parent().show();
        },
        complete: function () {
            setTimeout(function () {
                $(".loader").parent().hide();
            }, 1000);
        },
        error: function () {
            $(".loader").parent().hide();
        }*/
    });
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsControleTurma_Alunos);
arrFNCSys.push(jsControleTurma_Alunos);
