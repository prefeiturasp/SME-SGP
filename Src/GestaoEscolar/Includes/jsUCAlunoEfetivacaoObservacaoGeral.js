
var erroAPIEOL = false;

function jsUCAlunoEfetivacaoObservacaoGeral() {

    $(document).ready(function () {
        createTabs("#divTabs", "input[id$='txtSelectedTab']");
        $('#divTabs').tabs({
            selected: parseInt($("input[id$='txtSelectedTab']").val())
        });
    });
    createDialog('.divCadastroAnotacao', 600, 0);

    //createDialogCloseWithConfirmation('#divCadastroAnotacao', 555, 0);

    //$("textarea[name$='txtResumoDesempenho']").LimiteCaracteres(600);
    //$("textarea[name$='txtRecomendacaoAluno']").LimiteCaracteres(600);
    //$("textarea[name$='txtRecomendacaoResp']").LimiteCaracteres(700);

    $(".frequencia input").each(function () {
        var valorArredondado = AplicaVariacaoFrequencia(parseFloat($(this).val().replace(",", ".")));
        $(this).val(valorArredondado);
    });

    $()

    $('.ddlResultadoParecerConclusivo').unbind('change').bind('change', function () {
        var lbl = $('span[id$="lblMensagemResultadoInvalido"]');
        var lblErro = $('span[id$="lblMensagemResultadoErro"]');
        if ($('.ddlResultadoParecerConclusivo').children("option").filter(":selected").val() != "-1") {
            try
            {
                VerificarIntegridadeParecerEOL(function (retorno) {
                    if (retorno == "") {
                        lbl.addClass('hide');
                        lblErro.removeClass('hide');
                        $('select[$(".ddlResultadoParecerConclusivo")]').val("-1");
                        setTimeout('$(\'#divCadastroObservacaoGeral\').scrollTo(0,0);', 0);
                    }
                    else if (retorno == "true" || retorno == "True" || retorno == "TRUE") {
                        lbl.addClass('hide');
                        lblErro.addClass('hide');
                    }
                    else {
                        if (erroAPIEOL) {
                            lblErro.removeClass('hide');
                            lbl.addClass('hide');
                        }
                        else {
                            lbl.removeClass('hide');
                            lblErro.addClass('hide');
                        }
                        $('select[$(".ddlResultadoParecerConclusivo")]').val("-1");
                        setTimeout('$(\'#divCadastroObservacaoGeral\').scrollTo(0,0);', 0);
                    }
                });
            }
            catch (e)
            {
                lbl.addClass('hide');
                lblErro.removeClass('hide');
                $('select[$(".ddlResultadoParecerConclusivo")]').val("-1");
            }
        } else {
            lbl.addClass('hide');
            lblErro.addClass('hide');
        }
    });

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
}

function abrirTextoComMensagem(divTexto, btnExpandir, btnFechar) {
    $('#' + divTexto).css('display', 'inline-block');
    $('#' + btnExpandir).css('display', 'none');
    $('#' + btnFechar).css('display', 'inline-block');
}

function fecharTextoComMensagem(divTexto, btnExpandir, btnFechar) {
    $('#' + divTexto).css('display', 'none');
    $('#' + btnExpandir).css('display', 'inline-block');
    $('#' + btnFechar).css('display', 'none');
}

function IncluirTextoArea(classRepeater, texto) {
    var textarea = $('.' + classRepeater + ' .bimestreAtual');
    if (textarea.attr("disabled") == false) {
        if (textarea.val() != "") {
            textarea.val(textarea.val() + '\n');
        }

        textarea.val(textarea.val() + texto);
        textarea.keypress();
    }
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

function VerificarIntegridadeParecerEOL(onComplete) {
    var codigoEOLTurma = $('input[id$="hdnCodigoEOLTurma"]').val();
    var codigoEOLAluno = $('input[id$="hdnCodigoEOLAluno"]').val();
    var resultado = $('.ddlResultadoParecerConclusivo').children("option").filter(":selected").text();

    var retorno = "false";
    erroAPIEOL = false;

    $.ajax({
        type: "POST",
        url: pageName + "/VerificarIntegridadeParecerEOL",
        data: '{ "CodigoEOLTurma": "' + codigoEOLTurma + '", "CodigoEOLAluno": "' + codigoEOLAluno + '", "resultado": "' + resultado + '", "chamadaJavaScript": "true"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            retorno = data.d;
        },
        error: function (data, success, error) {
            erroAPIEOL = true;
        },
        complete: function (data) {
            onComplete(retorno);
        }
    });
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsUCAlunoEfetivacaoObservacaoGeral);
arrFNCSys.push(jsUCAlunoEfetivacaoObservacaoGeral);