function jsBuscaMinhaEscolaGestor() {
    createDialog('.divIndicadores', 800, 500);
    createDialog('#divDisciplinasCompartilhadas', 400, 0);
    createDialog('.divPendencias', 500, "auto");

    $($(".fdsTurmas").get().reverse()).each(function (index, element) {
        var fdsTurmas = $(element);

        fdsTurmas.find("#divTabs").tabs({
            selected: fdsTurmas.find("input[id$='txtSelectedTab']").val()
            , select: function (event, ui) {
                fdsTurmas.find("input[id$='txtSelectedTab']").val(ui.index);
            }
        });

        if (fdsTurmas.find('[id$="grvTurmasExtintas"]').length == 0) {
            fdsTurmas.find('#liTurmasEx').css('display', 'none');
        }
        else {
            fdsTurmas.find('#liTurmasEx').css('display', 'block');
        }

        if (fdsTurmas.find('[id$="grvProjetosRecParalela"]').length == 0) {
            fdsTurmas.find('#liProjetos').css('display', 'none');
        }
        else {
            fdsTurmas.find('#liProjetos').css('display', 'block');
        }
    });

    $('.txtPrevistas').blur(function () {
        var total = 0;
        $('.txtPrevistas').each(function () {
            total += Number($(this).val());
        });

        $('.lblPrevistas').text(total);
    });
}

function CarregarCacheEfetivacao(btn) {
    var item = $(btn).parent();

    $.ajax({
        type: "GET",
        url: "CacheEfetivacaoHandler.ashx",
        data: RetornaDados(item),
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json"/*,
        beforeSend: function () {
            $(".loader").parent().show();
        },
        complete: function()
        {
            setTimeout(function () {
                $(".loader").parent().hide();
            }, 1000);
        },
        error: function () {
            $(".loader").parent().hide();
        }*/
    });
}

function RetornaDados(item) {
    var dadosFechamento = item.find('input[name$="hdnDadosFechamento"]').val().split(";");
    return {
        "tud_id": dadosFechamento[0],
        "tur_id": dadosFechamento[1],
        "tpc_id": dadosFechamento[2],
        "ava_id": dadosFechamento[3],
        "fav_id": dadosFechamento[4],
        "tipoAvaliacao": dadosFechamento[5],
        "esa_id": dadosFechamento[6],
        "tipoEscalaDisciplina": dadosFechamento[7],
        "tipoEscalaDocente": dadosFechamento[8],
        "notaMinimaAprovacao": dadosFechamento[9],
        "ordemParecerMinimo": dadosFechamento[10],
        "tipoLancamento": dadosFechamento[11],
        "fav_calculoQtdeAulasDadas": dadosFechamento[12],
        "tur_tipo": dadosFechamento[13],
        "cal_id": dadosFechamento[14],
        "tud_tipo": dadosFechamento[15],
        "tpc_ordem": dadosFechamento[16],
        "fav_variacao": dadosFechamento[17],
        "tipoDocente": dadosFechamento[18],
        "disciplinaEspecial": dadosFechamento[19],
        "permiteAlterarResultado": permiteAlterarResultado,
        "exibirNotaFinal": exibirNotaFinal,
        "ExibeCompensacao": ExibeCompensacao,
        "MinutosCacheFechamento": MinutosCacheFechamento,
        "fechamentoAutomatico": dadosFechamento[20],
        "processarFilaFechamentoTela": $('input[name$="hdnProcessarFilaFechamentoTela"]').val()
    }
}

arrFNC.push(jsBuscaMinhaEscolaGestor);
arrFNCSys.push(jsBuscaMinhaEscolaGestor);