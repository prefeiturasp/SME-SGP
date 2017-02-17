var idNotaNumerica = '.gridRow .colunaNota input[type="text"], .gridAlternatingRow .colunaNota input[type="text"], .gridRow .colunaNotaAdicional input[type="text"], .gridAlternatingRow .colunaNotaAdicional input[type="text"], .trExpandir .colunaNota input[type="text"]';
var idNotaPareceres = '.gridRow .colunaNota select, .gridAlternatingRow .colunaNota select, .trExpandir .colunaNota select';
var idCheckBoxFaltoso = '.gridRow .ColunaFaltoso input[type="checkbox"], .gridAlternatingRow .ColunaFaltoso input[type="checkbox"]';

function abrirPopupProgressao() {
    $(document).ready(function () { $('#divRedirecionaProgressao').dialog('open'); });
}

function jsSetaResultadoAutomatico() {
    try {
        if (criterioAvaliacao != undefined) {
            $(idNotaPareceres).each(function () {
                $(this).bind('change', function () {
                    SetaResultadoLinha(this, $(this).parents('tr'));
                });
            });

            $(idNotaNumerica).each(function () {
                $(this).bind('blur', function () {
                    SetaResultadoLinha(this, $(this).parents('tr'));
                });
            });

            $('.colunaQtdeAula input[type="text"], .colunaQtdeFalta input[type="text"]').bind('blur', function () {
                SetaResultadoLinha(this, $(this).parents('tr'));
            });

            $('.gridRow select[id$="ddlResultado"], .gridAlternatingRow select[id$="ddlResultado"]').each(function () {
                if ($(this).find('option:selected').attr('value') == "-1") {
                    SetaResultadoLinha(this, $(this).parents('tr'));
                }
            });
        }
    }
    catch (e)
    { }
}

function SetaResultadoLinha(item, row) {
    try {
        var ddlPareceres = row.find('td.colunaNota').find('select');
        var txtNota = row.find('td.colunaNota').find('input[type="text"]');

        var txtFrequencia = row.find('input[id$="txtFrequenciaAcumulada"]');
        var txtFrequenciaFinal = row.find('input[id$="txtFrequenciaFinal"]');
        var ddlResultado = row.find('select[id$="ddlResultado"]');

        var notaValida = false;
        var frequenciaValida = true;
        var resultado = 0;

        if (ddlPareceres.size() > 0) {
            // Busca o valor da ordem.
            var ordem = ddlPareceres.find('option:selected').attr('value').split(';')[1];

            if (ordem != '') {
                notaValida = (ordem >= parecerMinimoAprovacao);
            }
        }
        else if (parecerMinimoAprovacao >= 0) {
            notaValida = true;
        }

        if (txtNota.size() > 0) {
            // Tenta converter o valor do txt pra float.
            var nota = parseFloat(txtNota.attr('value').replace(',', '.'));

            notaValida = (nota >= notaMinimaAprovacao);
        }
        else if (notaMinimaAprovacao >= 0) {
            notaValida = true;
        }

        // Se não tem ordem nem nota mínima, é formato por relatórios - sempre aprova por nota.
        if (parecerMinimoAprovacao < 0 && notaMinimaAprovacao < 0)
            notaValida = true;

        if (txtFrequencia.size() > 0) {
            var frequencia = parseFloat(txtFrequencia.attr('value').replace(',', '.'));
            frequenciaValida = frequencia >= percentualMinimoFrequencia;
        }

        if (txtFrequenciaFinal.size() > 0) {
            var frequencia = parseFloat(txtFrequenciaFinal.attr('value').replace(',', '.'));
            frequenciaValida = frequencia >= percentualMinimoFrequencia;
        }

        // Criterios de avaliação: ConceitoGlobalFrequencia = 1,ConceitoGlobal = 2,NotaDisciplina = 3, Apenas frequência = 4, Todos Aprovados = 5
        // Resultados: Aprovado = 1,Reprovado = 2,ReprovadoFrequencia = 8,RecuperacaoFinal = 9

        if (criterioAvaliacao == 1) {
            if (!notaValida) {
                resultado = 2;
            }
            else if (frequenciaValida) {
                resultado = 1;
            }
            else {
                resultado = 8;
            }
        }
        else if (criterioAvaliacao == 4) {
            // Critério = 4  Valida apenas a frequência
            if (frequenciaValida) {
                resultado = 1;
            }
            else {
                resultado = 8;
            }
        }
        else if (criterioAvaliacao == 5) {
            // Critério = 5 Sempre aprovado
            resultado = 1
        }
        else {
            // Critério = 2-ConceitoGlobal ou 3-NotaDisicplina. Só valida a nota.
            resultado = notaValida ? 1 : 2;
        }

        if (possuiAvaliacaoRecuperacaoFinal && resultado != 1) {
            // Se tem avaliação de recuperação final, e o resultado é "não aprovado", ele vai pra recuperação.
            resultado = 9;
        }

        ddlResultado.find('option:selected').removeAttr('selected');
        ddlResultado.find('option[value="' + resultado + '"]').first().attr('selected', 'selected');
    }
    catch (e)
    { }
}

function jsCadastroEfetivacao() {
    createDialog('#divRelatorio', 555, 0);
    createDialog('#divConfirmaAtualizacao', 555, 0);
    createDialog('#divAvaliacao', 555, 225);
    createDialog('#divRegistroClasse', 380, 130);
    createDialog('#divRedirecionaProgressao', 555, 0);
    createDialogCloseWithConfirmation('#divCadastroObservacao', 1100, 0);
    createDialogCloseWithConfirmation('#divCadastroObservacaoGeral', 1100, 0);
    createDialog('.divJustificativa', 555, 0);
    createDialog('#divJustificativaParecerConclusivo', 555, 0);
    createDialog('.divJustificativaPosConselho', 555, 0);
    createDialog('#divBoletimCompleto', "90%", 0);

    // Cálculo da frequência
    function CalculaFrequencia(QtdeAulas, QtdeFaltas) {
        var result = 0;

        QtdeAulas = QtdeAulas.replace(',', '.');
        QtdeFaltas = QtdeFaltas.replace(',', '.');

        if (QtdeAulas > 0)
            result = (((parseFloat(QtdeAulas) - parseFloat(QtdeFaltas)) / parseFloat(QtdeAulas)) * 100);

        return AplicaVariacaoFrequencia(result);
    }

    function AplicaVariacaoFrequencia(result) {
        var variacao = parseFloat($("input[id$=hdnVariacaoFrequencia]").val().replace(",", "."));
        variacao = isNaN(variacao) || variacao <= 0 ? 0.01 : variacao;

        var NotaLimiteArredondamento = 0.5;
        var sVariacao = variacao.toString().split(".");
        var numeroCasas = 0;

        if (variacao > 0 && sVariacao.length == 1) //Numero positivo sem casas decimais
            numeroCasas = 0;
        else if (sVariacao.length == 1)
            numeroCasas = 2;
        else
            numeroCasas = sVariacao[1].length;

        if (result != parseInt(result)) {
            var parteInteira = (result - parseInt(result)) / variacao;
            var parteDecimal = parteInteira - parseInt(parteInteira);

            result = parseInt(result)
                        + (parseInt(parteInteira) * variacao)
                        + (NotaLimiteArredondamento <= parteDecimal ? variacao : 0);
        }

        return result.toFixed(numeroCasas);
    }

    $('.gridRow .colunaQtdeAula input[type="text"], .gridAlternatingRow .colunaQtdeAula input[type="text"]').unbind('blur').bind('blur', function () {
        try {

            var txtQtdeFalta = $(this).parents('.gridRow, .gridAlternatingRow').find('.colunaQtdeFalta input[type="text"]');
            var QtdeFalta = (txtQtdeFalta.attr('value') == '' ? '0' : txtQtdeFalta.attr('value'));
            var QtdeAula = (this.value == '' ? '0' : this.value);

            if (QtdeAula > 0) {
                var result = CalculaFrequencia(QtdeAula, QtdeFalta);
                $(this).parents('.gridRow, .gridAlternatingRow').find('.colunaFrequencia input[type="text"]').attr('value', result.replace('.', ','));
            }

            //if (QtdeAula > 0) {
            //    var txtQtdeCompensacao = $(this).parents('.gridRow, .gridAlternatingRow').find('.colunaQtdeCompensacao input[type="text"]');
            //    var QtdeCompensacao = (txtQtdeCompensacao.attr('value') == '' ? '0' : txtQtdeCompensacao.attr('value'));
            //    var qtdeFaltas = parseInt(QtdeFalta) - parseInt(QtdeCompensacao);
            //    var resultFinal = CalculaFrequencia(QtdeAula, qtdeFaltas.toString());
            //    $(this).parents('.gridRow, .gridAlternatingRow').find('.colunaFrequenciaAjustada input[type="text"]').attr('value', resultFinal.replace('.', ','));
            //}

            MostrarSalvar();
        }
        catch (ex) {
        }
    });

    $('.gridRow .colunaQtdeFalta input[type="text"], .gridAlternatingRow .colunaQtdeFalta input[type="text"]').unbind('blur').bind('blur', function () {
        try {

            var txtQtdeAula = $(this).parents('.gridRow, .gridAlternatingRow').find('.colunaQtdeAula input[type="text"]');
            var QtdeAula = (txtQtdeAula.attr('value') == '' ? '0' : txtQtdeAula.attr('value'));
            var QtdeFalta = (this.value == '' ? '0' : this.value);

            var txtFrequencia = $(this).parents('.gridRow, .gridAlternatingRow').find('.colunaFrequencia input[type="text"]');
            if (QtdeAula > 0) {
                var result = CalculaFrequencia(QtdeAula, QtdeFalta);
                txtFrequencia.attr('value', result.replace('.', ','));
            }

            //var txtFrequenciaFinal = $(this).parents('.gridRow, .gridAlternatingRow').find('.colunaFrequenciaAjustada input[type="text"]');
            //var txtQtdeCompensacao = $(this).parents('.gridRow, .gridAlternatingRow').find('.colunaQtdeCompensacao input[type="text"]');
            //var QtdeCompensacao = (txtQtdeCompensacao.attr('value') == '' ? '0' : txtQtdeCompensacao.attr('value'));

            //if (QtdeAula > 0) {
            //    var qtdeFaltas = parseInt(QtdeFalta) - parseInt(QtdeCompensacao);
            //    var resultFinal = CalculaFrequencia(QtdeAula, qtdeFaltas.toString());
            //    txtFrequenciaFinal.attr('value', resultFinal.replace('.', ','));
            //}

            MostrarSalvar();
        }
        catch (ex) {
        }
    });

    $('.gridRow .colunaQtdeCompensacao input[type="text"], .gridAlternatingRow .colunaQtdeCompensacao input[type="text"]').unbind('blur').bind('blur', function () {
        try {

            var txtQtdeAula = $(this).parents('.gridRow, .gridAlternatingRow').find('.colunaQtdeAula input[type="text"]');
            var txtQtdeFalta = $(this).parents('.gridRow, .gridAlternatingRow').find('.colunaQtdeFalta input[type="text"]');
            var QtdeAula = (txtQtdeAula.attr('value') == '' ? '0' : txtQtdeAula.attr('value'));
            var QtdeFalta = (txtQtdeFalta.attr('value') == '' ? '0' : txtQtdeFalta.attr('value'));

            var txtFrequencia = $(this).parents('.gridRow, .gridAlternatingRow').find('.colunaFrequencia input[type="text"]');
            if (QtdeAula > 0) {
                var result = CalculaFrequencia(QtdeAula, QtdeFalta);
                txtFrequencia.attr('value', result.replace('.', ','));
            }

            //var txtFrequenciaFinal = $(this).parents('.gridRow, .gridAlternatingRow').find('.colunaFrequenciaAjustada input[type="text"]');
            //var QtdeCompensacao = (this.value == '' ? '0' : this.value);

            //if (QtdeAula > 0) {
            //    var qtdeFaltas = parseInt(QtdeFalta) - parseInt(QtdeCompensacao);
            //    var resultFinal = CalculaFrequencia(QtdeAula, qtdeFaltas.toString());
            //    txtFrequenciaFinal.attr('value', resultFinal.replace('.', ','));
            //}

            MostrarSalvar();
        }
        catch (ex) {
        }
    });

    // Nota numérica
    $(idNotaNumerica).unbind('blur').bind('blur', function () {
        NotaBlur($(this));
        MostrarSalvar();
    });

    // Nota numérica
    $(idNotaNumerica).unbind('keyup').bind('keyup', function () {
        NotaKeyUp($(this));
        MostrarSalvar();
    });

    // Pareceres
    $(idNotaPareceres).unbind('change').bind('change', function () {
        ParecerChange($(this));
        MostrarSalvar();
    });

    $(idCheckBoxFaltoso).unbind('click').click(function () {
        VerificaCheckbox($(this), true);
    });

    $('.gridRow select[id$="ddlResultado"], .gridAlternatingRow select[id$="ddlResultado"]').unbind('change').bind('change', function () {
        MostrarSalvar();
    });

    $(".gridRow, .gridAlternatingRow, .trExpandir").each(function () {
        $(this).find('.ColunaFaltoso input[type="checkbox"]').each(function () {
            VerificaCheckbox($(this), false);
        });

        $(this).find('.colunaNota select').each(function () {
            ParecerChange($(this));
        });

        $(this).find('.colunaNota input[type="text"], .colunaNotaAdicional input[type="text"]').each(function () {
            NotaKeyUp($(this));
            NotaBlur($(this));
        });
    });

    $("#spTrocarTurma select").unbind('change').bind('change', function () {
        CarregarCacheEfetivacaoCombo($("#spTrocarTurma select option:selected").val());
    });

    var hdnVisibleBotaoSalvar = $("input[id$='hdnVisibleBotaoSalvar']");
    if (hdnVisibleBotaoSalvar.length > 0)
    {
        exibeMensagemSair = hdnVisibleBotaoSalvar.val() == "true";
    }
    SetExitPageConfirmer();

    $(document).ready(function () {
        $("a[id$='btnExpandir']").removeAttr('disabled');

        // mantem expandido apos o postback
        var idExpandido = "input[id$='hfExpandido'][value='1']";
        $(idExpandido).bind('blur', function () {
            // aberto
            ExpandCollapse('.trExpandir', $(this).prev("[id$='btnExpandir']"));
        });
        $(idExpandido).trigger('blur');

        var idExpandidoTodos = "input[id$='hfExpandidoTodos'][value='1']";
        $(idExpandidoTodos).bind('blur', function () {
            var btnExpandir = $(this).prev("[id$='btnExpandir']");
            var toolTip = $(btnExpandir).attr("title").toString();
            $(btnExpandir).removeClass('ui-icon ui-icon-circle-triangle-s');
            $(btnExpandir).addClass('ui-icon ui-icon-circle-triangle-n');
            if (toolTip.indexOf("Expandir") == 0) {
                $(btnExpandir).attr("title", toolTip.replace("Expandir", "Recolher"));
            }
        });
        $(idExpandidoTodos).trigger('blur');

        var idIsHead = "input[id$='hfIsHead']";
        $(idIsHead).bind('blur', function () {
            if ($(this).attr("value") == '1') {
                $(this).parents('.gridRow').css("display", "none");
            }
            else {
                $(this).parents('.gridRow').prev('.gridHeader').css("display", "none");
            }
        });
        $(idIsHead).trigger('blur');

        var ddlPareceresFinal = $("select[id$=ddlPareceresFinal]");
        ddlPareceresFinal.change(function () {
            $(this).attr('title', $(this).children("option:selected").attr('value'));
        });
        for (var i = 0; i < ddlPareceresFinal.length; i++) {
            var ddlParecerFinal = $(ddlPareceresFinal[i]);
            ddlParecerFinal.attr('title', ddlParecerFinal.children("option:selected").attr('value'));
        }

        // remove validacoes repetidas
        $("div[id$='vsEfetivacaoNotas']").html(GetUnique($("div[id$='vsEfetivacaoNotas']").html().split('<br>')).join('<br>'));
        if ($("div[id$='vsEfetivacaoNotas']").text() != '') {
            $("div[id$='vsEfetivacaoNotas']").css('display', 'block');
        }
    });
}

function NotaBlur(txtNota) {
    // Só muda o fundo se o checkbox de faltoso não estiver checado.
    if ($(txtNota).parents('.gridRow, .gridAlternatingRow').find('.ColunaFaltoso input[type="checkbox"]').size() == 0
        || !$(txtNota).parents('.gridRow, .gridAlternatingRow').find('.ColunaFaltoso input[type="checkbox"]').attr('checked')) {

        var nota = $(txtNota).attr('value');

        // Verifica se o campo nota não está vazio e se o parametro é verdadeiro
        if (nota != '') {

            try {
                // Tenta converter o valor do txt pra float.
                nota = parseFloat(nota.replace(',', '.'));

                if (!arredondamento) {
                    nota = nota.toFixed(numeroCasasDecimais);
                    $(txtNota).attr('value', nota == 'NaN' ? "" : nota.replace('.', ','));
                }

                if (nota < notaMinima && exibeCorMedia && parametro) {
                    if ($(txtNota).parents('.gridRow, .gridAlternatingRow').find('.colunaNota').size() > 0) {
                        $(txtNota).closest('td').css('background-color', '#FF3030');
                    }
                    return;
                }
            }
            catch (e) {
            }
        }

        if ($(txtNota).parents('.gridRow, .gridAlternatingRow').find('.colunaNota').size() > 0) {
            $(txtNota).closest('td').css('background-color', '');
        }
    }

    if (arredondamento && $(txtNota).val() != "") {
        $(txtNota).val(ArredondarValor($(txtNota).val()));
    }
}

function NotaKeyUp(txtNota) {
    var attr = $(txtNota).attr('disabled');
    // For some browsers, `attr` is undefined; for others,
    // `attr` is false.  Check for both.
    if (attr == undefined || attr == false) {
        // desabilita o botao de jutificativa pos-conselho, caso nao tenha digitado nenhuma nota
        if ($(txtNota).val() == '') {
            $(txtNota).siblings("input[id*='btnJustificativa']").attr("disabled", "disabled");
            $(txtNota).siblings("img[id*='imgJustificativa']").css("visibility", "hidden");
        }
        else {
            $(txtNota).siblings("input[id*='btnJustificativa']").removeAttr("disabled");
            $(txtNota).siblings("img[id*='imgJustificativa']").css("visibility", "visible");
        }
    }
}

function ParecerChange(ddlParecer) {
    // Só muda o fundo se o checkbox de faltoso não estiver checado.
    if (!$(ddlParecer).parents('.gridRow, .gridAlternatingRow').find('.ColunaFaltoso input[type="checkbox"]').attr('checked') && exibeCorMedia) {

        // Busca o valor da ordem.
        var ordem = $(ddlParecer).find('option:selected').attr('value').split(';')[1];

        try {
            // Verifica se o parametro é verdadeiro
            if (parametro) {
                if ((ordem != '') && (ordem != '-1')) {
                    if (ordem < parecerMinimo) {
                        if ($(ddlParecer).parents('.gridRow, .gridAlternatingRow').find('.colunaNota').size() > 0) {
                            $(ddlParecer).closest('td').css('background-color', '#FF3030');
                        }
                        return;
                    }
                }
            }
        }
        catch (e) {
        }

        if ($(ddlParecer).parents('.gridRow, .gridAlternatingRow').find('.colunaNota').size() > 0) {
            $(ddlParecer).closest('td').css('background-color', '');
        }
    }

    var attr = $(ddlParecer).attr('disabled');
    // For some browsers, `attr` is undefined; for others,
    // `attr` is false.  Check for both.
    if (attr == undefined || attr == false) {
        // desabilita o botao de jutificativa pos-conselho, caso nao tenha selecionado nenhum conceito
        if ($(ddlParecer).find('option:selected').index() == 0) {
            $(ddlParecer).siblings("input[id*='btnJustificativa']").attr("disabled", "disabled");
            $(ddlParecer).siblings("img[id*='imgJustificativa']").css("visibility", "hidden");
        }
        else {
            $(ddlParecer).siblings("input[id*='btnJustificativa']").removeAttr("disabled");
            $(ddlParecer).siblings("img[id*='imgJustificativa']").css("visibility", "visible");
        }
    }
}

function GetUnique(inputArray) {
    var outputArray = [];
    for (var i = 0; i < inputArray.length; i++) {
        if ((jQuery.inArray(inputArray[i].trim(), outputArray)) == -1) {
            outputArray.push(inputArray[i].trim());
        }
    }
    return outputArray;
}

function VerificaCheckbox(checkBox, click) {
    if (exibeCorMedia) {
        var cor = $(checkBox).attr('checked') ? '#FF3030' : "";
        if ($(checkBox).parents('.gridRow, .gridAlternatingRow').find('.colunaNota').size() > 0) {
            $(checkBox).parents('.gridRow, .gridAlternatingRow').find('.colunaNota')[0].bgColor = cor;
        }
    }

    // Notas da avaliação normal e adicional, nota numérica ou conceito.
    var idNotas_avaliacoes = '.colunaNota input[type="text"], .colunaNotaAdicional input[type="text"]';
    var idPareceres_avaliacoes = '.colunaNota select, .colunaNotaAdicional select';

    if (!$(checkBox).attr('checked')) {
        if (!periodoFechado) {
            $(checkBox).parents('.gridRow, .gridAlternatingRow').find(idNotas_avaliacoes + ',' + idPareceres_avaliacoes)
        .removeAttr('disabled');

            if (click) {
                $(checkBox).parents('.gridRow, .gridAlternatingRow').find(idNotas_avaliacoes + ',' + idPareceres_avaliacoes).focus();
            }
        }
    }
    else {
        // Limpa valor dos txts de nota.
        $(checkBox).parents('.gridRow, .gridAlternatingRow').find(idNotas_avaliacoes).attr('value', '');

        // Seleciona primeiro item no combo de pareceres.
        $(checkBox).parents('.gridRow, .gridAlternatingRow').find(idPareceres_avaliacoes).find('option')
            .removeAttr('selected')
            .first().attr('selected', 'selected');

        if (!periodoFechado) {
            // Desabilita checkbox e combo de pareceres de nota.
            $(checkBox).parents('.gridRow, .gridAlternatingRow').find(idNotas_avaliacoes + ',' + idPareceres_avaliacoes).attr('disabled', 'disabled');
        }
    }
}

function ExpandCollapse(divID, btn) {
    var div = $(btn).parents("tr").next(divID);
    var hfExpandido = $(btn).parents("tr").find("input[id$='hfExpandido']");
    if ($(div).css("display") == "none") {
        $(btn).removeClass('ui-icon ui-icon-circle-triangle-s');
        $(btn).addClass('ui-icon ui-icon-circle-triangle-n');

        $(div).css("display", "");
        hfExpandido.val("1"); //aberto
    }
    else {
        $(btn).removeClass('ui-icon ui-icon-circle-triangle-n');
        $(btn).addClass('ui-icon ui-icon-circle-triangle-s');

        $(div).css("display", "none");
        hfExpandido.val("0"); //fechado
    }
}

function ExpandCollapseAll(btn) {
    var hfExpandidoTodos = $(btn).parents("th").find("input[id$='hfExpandidoTodos']");
    var toolTip = $(btn).attr("title").toString();

    if (hfExpandidoTodos.val() == "1") {
        $(btn).removeClass('ui-icon ui-icon-circle-triangle-n');
        $(btn).addClass('ui-icon ui-icon-circle-triangle-s');

        // pego todos os botoes abertos e fecho
        $("input[id$='hfExpandido'][value='1']").siblings("a[id$='btnExpandir']").trigger('onclick');
        hfExpandidoTodos.val("0"); //fechado

        if (toolTip.indexOf("Recolher") == 0) {
            $(btn).attr("title", toolTip.replace("Recolher", "Expandir"));
        }
    }
    else {
        $(btn).removeClass('ui-icon ui-icon-circle-triangle-s');
        $(btn).addClass('ui-icon ui-icon-circle-triangle-n');

        // pego todos os botoes fechados e abro
        $("input[id$='hfExpandido'][value='0']").siblings("a[id$='btnExpandir']").trigger('onclick');
        hfExpandidoTodos.val("1"); //aberto

        if (toolTip.indexOf("Expandir") == 0) {
            $(btn).attr("title", toolTip.replace("Expandir", "Recolher"));
        }
    }
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

    return (total == "NaN" ? "" : totalArredondado.toFixed(numeroCasasDecimais).toString().replace('.', ','));
}

function MostrarSalvar() {
    var hdnVisibleBotaoSalvar = $("input[id$='hdnVisibleBotaoSalvar']");
    if (hdnVisibleBotaoSalvar.length > 0 && hdnVisibleBotaoSalvar.val() == "true") {
        $(".divSalvarFechamento").css("display", "block");
    }
}

function CarregarCacheEfetivacaoCombo(valor) {
    var dados = RetornaDadosCombo(valor);
    if (dados["fechamentoAutomatico"] == "true" && dados["processarFilaFechamentoTela"] == "true") {
        $.ajax({
            type: "GET",
            url: "CacheEfetivacaoHandler.ashx",
            data: dados,
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
}

function RetornaDadosCombo(valor) {
    var dados = valor.split(';');
    var tpc_id = "-1";
    var tipoAvaliacao = "1";
    var periodoSelecionado = $("input[id$='btnPeriodo'].periodo_selecionado")
    if (periodoSelecionado.length > 0)
    {
        tpc_id = periodoSelecionado.siblings("input[id$='hdnPeriodo']").val();
        tipoAvaliacao = periodoSelecionado.siblings("input[id$='hdnAvaliacaoTipo']").val();
    }
    return {
        "tur_id": dados[0],
        "tud_id": dados[1],
        //"cal_id": dados[2],
        //"tdt_posicao": dados[3],
        "tud_tipo": dados[4],
        //"tur_tipo": dados[5],
        //"tur_idNormal": dados[6],
        //"tud_idAluno": dados[7],
        "fechamentoAutomatico": dados[8],
        "tpc_id": tpc_id,
        "tipoAvaliacao": tipoAvaliacao,
        "processarFilaFechamentoTela": $('input[name$="hdnProcessarFilaFechamentoTela"]').val()
    }
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsCadastroEfetivacao);
arrFNCSys.push(jsCadastroEfetivacao);
arrFNC.push(jsSetaResultadoAutomatico);
arrFNCSys.push(jsSetaResultadoAutomatico);