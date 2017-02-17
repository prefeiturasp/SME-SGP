function jsControleTurma_DiarioClasse() {
    createDialogCloseWithConfirmation("#divCadastroAula", 500, 0);
    createDialogCloseWithConfirmation("#divLancamentoFrequencia", 600, 0);
    createDialogCloseWithConfirmation("#divConfirmarLancamento", 500, 0);
    createDialogCloseWithConfirmation("#divAnotacoesAluno", 900, 0);
    createDialogCloseWithConfirmation("#divAnotacoesMaisdeUmAluno", 900, 0);
    createDialogCloseWithConfirmation("#divAtividadeAvaliativa", 760, 0);
    createDialogCloseWithConfirmation("#divPlanoAula", null, 0);
    createDialogCloseWithConfirmation('#divAvaliacao', 555, 225);
    createDialogCloseWithConfirmation("#divConfirmacaoExclusaoAulaDiretor", 550, 0);

    createTabs("#divTabsPlanejamento", "input[id$='txtSelectedTab']");

    $("#tabela").tablesorter();
    $("#tabelaAtividade").tablesorter();

    if ($('input[id*=hdnSituacaoTurmaDisciplina]').val() == '4') {
        $('.tabPlanejamento').hide();
    }

    $(idDdlOrdenacaoFrequencia).unbind('change').change(function () {
        Ordena($(this), "#tabela");
    });
    $(idDdlOrdenacaoAvaliacao).unbind('change').change(function () {
        Ordena($(this), "#tabelaAtividade");
    });

    $(idDdlOrdenacaoFrequencia).trigger('change');
    $(idDdlOrdenacaoAvaliacao).trigger('change');

    // check habilita/desabilita campos de edição para nota
    $("input:checkbox[id$='chkDesconsiderar']").click(function () {
        var valor = $(this).attr('checked');
        $(this).parent().parent().find("[id$='txtNota']").val("").attr("disabled", valor);
    });

    $("#tabela,#tabelaAtividade").bind("sortEnd", function () {
        $(this).find('.gridRow, .gridAlternatingRow').each(function () {
            var linha = $(this);
            var indicePar = (linha.index() % 2) == 0;

            if (indicePar && linha.hasClass('gridAlternatingRow')) {
                linha.removeClass('gridAlternatingRow').addClass('gridRow');
            }

            if (!indicePar && linha.hasClass('gridRow')) {
                linha.removeClass('gridRow').addClass('gridAlternatingRow');
            }
        });

    });

    $("#tabela,#tabelaAtividade").trigger('sortEnd');

    // check habilita/desabilita campos de edição para nota
    $("input[name$='chkParticipante']").unbind('click').click(function () {
        var valor = $(this).attr('checked') == false;
        $(this).parents("td").find("input[type=text]").attr('disabled', valor);
        $(this).parents("td").find("input[type=text]").attr('value', '');
        $(this).parents("td").find("input[type=image]").attr('disabled', valor);
        $(this).parents("td").find("select[id$='ddlPareceres'] option").removeAttr('selected').find(':first').attr('selected', 'selected');
        $(this).parents("td").find("select[id$='ddlPareceres']").attr('disabled', valor);
        if (!valor) {
            $(this).parents("td").find("input[type=text]").focus();
            $(this).parents("td").find("select[id$='ddlPareceres']").focus();
        }
    });

    $('.tbAtividadeAvaliativa .gridRow input[type="text"], .tbAtividadeAvaliativa .gridAlternatingRow input[type="text"]').unbind('blur').bind('blur', function () {
        var maiorNotaEscala = parseFloat(maiorValor);
        var vNota = parseFloat($(this).attr('value') == "" || $(this).attr('value') == 'NaN' ? '0' : $(this).attr('value').replace(',', '.'));
        vNota = vNota.toFixed(qtdCasasDecimais);

        var isNullNota = ($(this).val() == "");

        var chkDesconsiderar = $(this).parent().find("input:checkbox[id$='chkDesconsiderar']");

        if (!isNullNota) {
            $(this).attr('value', vNota == 'NaN' ? "" : vNota.replace('.', ','));
        } else if (chkDesconsiderar.size() > 0) {
            $(this).parent().find("input:checkbox[id$='chkDesconsiderar']").attr("checked", "true");
            $(this).val("").attr("disabled", "disabled");
        }

        if (destacarCampoNota && maiorNotaEscala != 'NaN' && vNota > maiorNotaEscala) {
            $(this).css("border-color", "red");
        }
        else {
            $(this).css("border-color", "");
        }

        if (arredondamento && $(this).val() != "") {
            $(this).val(ArredondarValor($(this).val()));
        }
    });

    $('.tbAtividadeAvaliativa .gridRow input[type="text"], .tbAtividadeAvaliativa .gridAlternatingRow input[type="text"]').trigger('blur');

    if (calcularMediaAutomatica || arredondamento) {
        $('.tbAtividadeAvaliativa .gridRow input[type="text"], .tbAtividadeAvaliativa .gridAlternatingRow input[type="text"]')
            .trigger('blur');
    }

    $(idChkRecursos).find('input[type="checkbox"]').last().unbind('click').click(function () {
        var valor = $(this).attr('checked');
        setaDisplayCss(idtxtOutroRecurso, valor);
        if (valor) {
            $(idtxtOutroRecurso).attr('value', '').focus();
        }
    });

    $('.divTreeviewScrollCOC .planejado input[type="checkbox"]').click(function () {
        var x = $(this);
        chktrabalhadoSetEnabled(this);
        //sem necessidade uma vez que o periodo anterior nao será visto
        //coloreOrientacao(this, '.planejado', corPlanejado);
        //coloreOrientacao(this, '.trabalhado', corTrabalhado);
        setTimeout(function () {
            x.attr('checked', !x.attr('checked'));
        }, 100);
    });

    $('.divTreeviewScrollCOC .trabalhado input[type="checkbox"]').click(function () {
        var checkbox = $(this);
        setTimeout(function () {
            checkbox.attr('checked', !checkbox.attr('checked'));
        }, 100);
    });

    if (($.browser.mozilla) && ($.browser.version == '9.0.1')) {
        var sheet2 = document.createElement('style')
        sheet2.innerHTML = ".divTreeLayout, x:-moz-any-link, x:default { " +
        "width: 362px!important; " +
		"height: 220px; " +
		"position: absolute; " +
		"background: #efefef; " +
		"right: 0px!important; " +
		"z-index:1; " +
        "margin: 10px; " +
        "top:-20px!important; }";
        document.body.appendChild(sheet2);
    }

    $('div.hitarea').unbind('click').click(function () {
        toggleArvore(this);
    });

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

function Ordena(controle, tabela) {
    var opcao = controle.find('option:selected').attr('value');
    var sorting;
    if (opcao == "0") {
        // Número de chamada.
        sorting = [[0, 0]];
    }
    else if (opcao == "1") {
        // Nome do aluno
        sorting = [[1, 0]];
    }
    $(tabela).trigger("sorton", [sorting]);
    return false;
}

function montaTreeview() {
    //Abre os nós com coisa checada.
    $('.divTreeviewScrollCOC .planejado input[type="checkbox"]:checked').parents('li').each(function () {
        $(this).parents('ul').css('display', 'block');
        $(this).children('div.hitarea').first().removeClass('expandable-hitarea').addClass('collapsable-hitarea');
    });

    //Abre os nós com coisa checada.
    $('.divTreeviewScrollCOC .alcancadoAvaliacao input[type="checkbox"]:checked').parents('li').each(function () {
        $(this).parents('ul').css('display', 'block');
        $(this).children('div.hitarea').first().removeClass('expandable-hitarea').addClass('collapsable-hitarea');
    });

    //Abre os nós com coisa checada.
    $('.divTreeviewScrollAula input[type="checkbox"]:checked').parents('li').each(function () {
        $(this).parents('ul').css('display', 'block');
        $(this).children('div.hitarea').first().removeClass('expandable-hitarea').addClass('collapsable-hitarea');
    });

    $('ul.treeview li:last-child').addClass('last');

    $('.planejado input[type="checkbox"]:checked').each(function () {
        coloreOrientacao(this, '.planejado', corPlanejado);
    });

    $('.trabalhado input[type="checkbox"]:checked').each(function () {
        coloreOrientacao(this, '.trabalhado', corTrabalhado);
    });

    $('.planejado input[type="checkbox"]:not(:checked)').each(function () {
        chktrabalhadoSetEnabled(this);
    });

    $('.OrientacaoPlanejadaAula input[type="checkbox"]').each(function () {
        $(this).attr('disabled', true);
    });

    $('.planejadoPeriodosAnteriores input[type="checkbox"]').each(function () {
        $(this).parent('span').parent('span').css('background', corPlanejado).css("color", contrastColor(corPlanejado));
        $(this).attr('disabled', true).attr('checked', true);
        chktrabalhadoSetEnabled(this);
    });

    $('.trabalhadoPeriodosAnteriores input[type="checkbox"]').each(function () {
        $(this).parent('span').parent('span').css('background', corTrabalhado).css("color", contrastColor(corTrabalhado));
        $(this).attr('disabled', true).attr('checked', true);
    });
}

//abre ou fecha o nó
function toggleArvore(div) {
    $(div).parent('li').find('ul').each(function () {
        var node = $(this);
        var display = node.css('display');
        if (display == 'none') {
            node.css('display', 'block');
            node.find('ul').css('display', 'block');
        } else if (display == 'block') {
            node.css('display', 'none');
            node.find('ul').css('display', 'none');
            node.find('div.hitarea').addClass('expandable-hitarea').removeClass('collapsable-hitarea');
        }
    });

    if ($(div).hasClass('collapsable-hitarea'))
        $(div).removeClass('collapsable-hitarea').addClass('expandable-hitarea');
    else if ($(div).hasClass('expandable-hitarea'))
        $(div).removeClass('expandable-hitarea').addClass('collapsable-hitarea');
}

function ArredondarValor(valornota) {
    var total = valornota.toString().replace(",", ".");
    var aux = 0;
    var totalArredondado = 0;
    var variacaoEscalaAvaliacao = 0;

    if (variacaoEscala != "" && variacaoEscala != "NaN") {
        variacaoEscalaAvaliacao = variacaoEscala;
    }

    // Arredondar -> se for '5.1' o sistema deve arredondar para '5.5', assim como '5.6' deve ser arredondada para '6.0', conforme a variação.

    if (total != "NaN") {
        aux = total - parseInt(total);
    }

    if (parseFloat(aux) == 0) {
        totalArredondado = total;
    }
    else if (parseFloat(aux) <= parseFloat(variacaoEscalaAvaliacao)) {
        totalArredondado = parseInt(total) + parseFloat(variacaoEscalaAvaliacao);
    }
    else if (parseFloat(aux) > parseFloat(variacaoEscalaAvaliacao)) {
        totalArredondado = parseInt(total) + (2 * parseFloat(variacaoEscalaAvaliacao));
    }
    totalArredondado = parseFloat(totalArredondado);

    return (total == "NaN" ? "" : totalArredondado.toFixed(qtdCasasDecimais).toString().replace('.', ','));
}

function chktrabalhadoSetEnabled(checkbox) {
    var trabalhado = $(checkbox).parent('span').parent('span').parent('div').find('.trabalhado input');
    var orientacaoPlanejada = $(checkbox).attr('checked');
    var alcancado = $('input[id$="imgMarcarAlcancado"]');

    if (!orientacaoPlanejada) {
        trabalhado.attr('disabled', true).removeAttr('checked');
    }
    else {
        trabalhado.removeAttr('disabled').parent('span.trabalhado').removeAttr('disabled');
        alcancado.removeAttr('disable').parent('div').removeAttr('disabled').parent('span').removeAttr('disabled').parent('div').removeAttr('disabled');
    }
}

function coloreOrientacao(checkbox, seletor, cor) {
    var chave = $(checkbox).parent('span').parent('span').parent('div').find('input[id$="hdnChave"]').val();
    var ids = chave.split(';');
    var tud_id = ids[0];
    var ocr_id = ids[1];
    var tpc_id = ids[2];
    var tpc_ordem = $('divTabs-1').find('input[id$="hdnTpcOrdem"]').val();

    $('#divTabsPlanejamento-1')
    .find(seletor + ' input[type="checkbox"]')
    .filter(function () {
        var chave2 = $(this).parent('span').parent('span').parent('div').find('input[id$="hdnChave"]').val();
        return ocr_id == chave2.split(';')[1] && tud_id == chave2.split(';')[0];
    }).each(function () {
        var marcacaoAnterior = VerificaMarcacaoAnterior(this, seletor);
        //$(this).parent('span').parent('span').parent('div').css('color', marcacaoAnterior == 2 ? corTrabalhado : (marcacaoAnterior == 1 ? corPlanejado : ''));
        $(this).parent('span').parent('span').find('.lblLegenda').css('color', marcacaoAnterior == 2 || marcacaoAnterior == 1 ? '#FFFFFF' : '');

        if (seletor == '.planejado') {
            $(this).parent('span').parent('span').css('background', marcacaoAnterior >= 1 ? cor : '').css("color", contrastColor(cor));
        } else {
            $(this).parent('span').parent('span').css('background', marcacaoAnterior == 2 ? cor : '').css("color", contrastColor(cor));
        }
    });
}

function VerificaMarcacaoAnterior(checkbox, seletor) {
    var chave = $(checkbox).parent('span').parent('span').parent('div').find('input[id$="hdnChave"]').val();
    var ids = chave.split(';');
    var tud_id = ids[0];
    var ocr_id = ids[1];
    var tpc_id = ids[2];
    var tpc_ordem = $('div#divTabs-' + tpc_id).find('input[id$="hdnTpcOrdem"]').val();

    var checados =
    $('#divTabsPlanejamento-Anterior')
    .find('input[type="checkbox"]:checked')
    .filter(function () {
        var chave2 = $(this).parent('span').parent('span').parent('div').find('input[id$="hdnChave"]').val();
        return ocr_id == chave2.split(';')[1] && tud_id == chave2.split(';')[0];
    });

    var qtdePlanejados = checados.filter(function () { return $(this).parent('span').hasClass('planejado'); }).size();
    var qtdeTrabalhados = checados.filter(function () { return $(this).parent('span').hasClass('trabalhado'); }).size();

    // 0 - nada marcado
    // 1 - planejado
    // 2 - trabalhado
    return qtdeTrabalhados > 0 ? 2 : (qtdePlanejados > 0 ? 1 : 0);
}

function LimitarCaracter(idCampo, idContador, TamMax) {
    if ((idCampo).value.length > TamMax) {
        (idCampo).value = (idCampo).value.substring(0, TamMax);
    }
    else {
        $('#' + idContador).css('color', 'black');
    }

    var Caracteres = (idCampo).value.length;
    $(idCampo).next($(idContador)).html(Caracteres + "/" + TamMax);
}

function AbrirPopUpLimpo(escondeQuantidade) {
    table = $('#divCadastroAula');
    qtdeAulas = $('#divQtdeAulas');

    $(idlblMessage3).html("");

    table.find('input[type="text"]').each(function () {
        $(this).val("");
    });

    table.find('input[type="checkbox"]').each(function () {
        $(this).attr('checked', false);
    });

    //escondeQuantidade.toLowerCase() == "true" ? qtdeAulas.css("display", "none") : qtdeAulas.css("display", "");

    $("#divCadastroAula").dialog("open");
    return false;
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsControleTurma_DiarioClasse);
arrFNCSys.push(jsControleTurma_DiarioClasse);

arrFNC.push(montaTreeview);
arrFNCSys.push(montaTreeview);
