function jsLancamentoAvaliacoesGeral() {
    $("input:checkbox[id$='chkFalta']").unbind("click").click(function (e) {
        var checkFalta = $(this);
        ConfiguraAusencia(checkFalta, checkFalta.is(":checked"), true);
    });

    // check habilita/desabilita campos de edição para nota
    $("input:checkbox[id$='chkDesconsiderar']").unbind('click').click(function () {
        var valor = $(this).attr('checked');
        $(this).parent().parent().find("[id$='txtNota']").val("").attr("disabled", valor);

        if (calcularMediaAutomatica) {
            CalculaNotaFinal("Media");
        }
    });

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

    $('.tbLancamentoAvaliacoes .gridRow input[type="text"], .tbLancamentoAvaliacoes .gridAlternatingRow input[type="text"]').unbind('blur').bind('blur', function () {

        if (arredondamento && $(this).val() != "") {
            $(this).val(ArredondarValor($(this).val()));
        }

        // Cálculo da média.
        //if (calcularMediaAutomatica) {
        var notas = 0;
        var qtNotas = 0;
        var maiorNotaEscala = parseFloat(maiorValor);

        if (calcularMediaAutomatica) {

            //Verifica se e a nota final para nao atualizar ela mesma
            if ($(this).attr('id').toLowerCase().indexOf("txtnotafinal") < 0) {
                $(this).parents('.tbLancamentoAvaliacoes .gridRow, .tbLancamentoAvaliacoes .gridAlternatingRow').find('input[type="text"]').each(function () {
                    // Verifica se não é avaliação marcada como "não consta na média".
                    if ($(this).parents('td').find('.media input').val() != 'True') {
                        // Verifica se não está marcado o checkbox "F" - falta.
                        if ($(this).parents('td').find('input[type="checkbox"][id$="chkFalta"]:checked').size() == 0) {
                            //Verifica se nao e o textbox da nota final
                            if ($(this).attr('id').toLowerCase().indexOf("txtnotafinal") < 0) {
                                var vNota = parseFloat($(this).attr('value') == "" || $(this).attr('value') == 'NaN' ? '0' : $(this).attr('value').replace(',', '.'));
                                notas += vNota;

                                if (destacarCampoNota && maiorNotaEscala != 'NaN' && vNota > maiorNotaEscala) {
                                    $(this).css("border-color", "red");
                                }
                                else {
                                    $(this).css("border-color", "");
                                }

                                // Soma quantidade de notas somente para as participantes.
                                if (($(this).parents("td").find("input[name$='chkParticipante']").length == 0) && ($(this).val() != "")
                                    || (($(this).parents("td").find("input[name$='chkParticipante']").length > 0)
                                    && ($(this).parents("td").find("input[name$='chkParticipante']").attr('checked') == true)
                                    && ($(this).attr('disabled') == false)))
                                    qtNotas++;
                            }
                        }
                    }
                });

                valorMedia = notas / qtNotas;
                valorSoma = notas;

                var result = valorMedia;

                result = result.toFixed(qtdCasasDecimais);

                var nota = parseFloat($(this).attr('value').replace(',', '.'));
                nota = nota.toFixed(qtdCasasDecimais);
                $(this).attr('value', nota == 'NaN' ? "" : nota.replace('.', ','));

                if (result == "NaN")
                    result = "";

                if (calcularMediaAutomatica) {
                    $(this).parents('.tbLancamentoAvaliacoes .gridRow, .tbLancamentoAvaliacoes .gridAlternatingRow').find('.spMedia').html(result.replace('.', ','));
                    $(this).parents('.tbLancamentoAvaliacoes .gridRow, .tbLancamentoAvaliacoes .gridAlternatingRow').find('input[type="text"]').each(function () {
                        if ($(this).attr('id').toLowerCase().indexOf("txtnotafinal") >= 0) {
                            $(this).val(result.replace('.', ','));
                        }
                    });
                }
            }
            else {
                var nota = parseFloat($(this).attr('value').replace(',', '.'));
                nota = nota.toFixed(qtdCasasDecimais);
                $(this).attr('value', nota == 'NaN' ? "" : nota.replace('.', ','));

                if (destacarCampoNota && maiorNotaEscala != 'NaN' && parseFloat($(this).attr('value').replace(',', '.')) > maiorNotaEscala) {
                    $(this).css("border-color", "red");
                }
                else {
                    $(this).css("border-color", "");
                }
            }
        }

        // Seta a cor vermelho no txt caso a nota seja maior que a máxima.
        var maiorNotaEscala = parseFloat(maiorValor);
        var vNota = parseFloat($(this).attr('value') == "" || $(this).attr('value') == 'NaN' ? '0' : $(this).attr('value').replace(',', '.'));
        var isNullNota = ($(this).val() == "");
        HabilitaNotaRecuperacao(this, vNota, isNullNota, 1);

        vNota = vNota.toFixed(qtdCasasDecimais);
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
    });

    $('.tbLancamentoAvaliacoes .gridRow select[id$="ddlPareceres"], .tbLancamentoAvaliacoes .gridAlternatingRow select[id$="ddlPareceres"]').unbind('change').bind('change', function () {
        var vNota = $(this)[0].selectedIndex;
        var isNullNota = (vNota <= 0);
        HabilitaNotaRecuperacao(this, vNota - 1, isNullNota, 2);
    });

    if (calcularMediaAutomatica || arredondamento) {
        $('.tbLancamentoAvaliacoes .gridRow input[type="text"], .tbLancamentoAvaliacoes .gridAlternatingRow input[type="text"]')
            .trigger('blur');
    }

    var i;
    var checksFalta = $(".tbLancamentoAvaliacoes").find("[id$='chkFalta']:checked");
    for (i = 0; i < checksFalta.length; i++) {
        ConfiguraAusencia($(checksFalta[i]), true, false);
    }

    var txtsNota = $(".tbLancamentoAvaliacoes").find("input:text[id$='txtNota']");
    for (i = 0; i < txtsNota.length; i++) {
        var txtNota = $(txtsNota[i]);
        var vNota = parseFloat(txtNota.attr('value') == "" || txtNota.attr('value') == 'NaN' ? '0' : txtNota.attr('value').replace(',', '.'));
        var isNullNota = (txtNota.val() == "");
        HabilitaNotaRecuperacao(txtNota, vNota, isNullNota, 1);
    }

    var combosPareceres = $(".tbLancamentoAvaliacoes").find("select[id$='ddlPareceres']");
    for (i = 0; i < combosPareceres.length; i++) {
        var comboPareceres = $(combosPareceres[i]);
        var vNota = comboPareceres[0].selectedIndex;
        var isNullNota = (vNota <= 0);
        HabilitaNotaRecuperacao(comboPareceres, vNota - 1, isNullNota, 2);
    }
}

function ConfiguraAusencia(chkFalta, checado, causaBlurTxt) {
    if (!chkFalta.attr("disabled")) {
        var checkParent = chkFalta.parents('td');
        var txtNota = checkParent.find("input:text[id$='txtNota']");
        var ddlPareceres = checkParent.find("select[id$='ddlPareceres']");
        var btnRelatorio = checkParent.find("input[id$='btnRelatorio']");
        var btnHabilidade = checkParent.find("[id$='btnHabilidade']");
        if (checado) {
            if (txtNota.length > 0 && txtNota.is(":visible")) {
                txtNota.val("").attr("disabled", "disabled");
            }
            else if (ddlPareceres.length > 0 && ddlPareceres.is(":visible")) {
                ddlPareceres.removeAttr('selected').first().attr('selected', 'selected');
                ddlPareceres.attr("disabled", "disabled");
            }
            else if (btnRelatorio.length > 0 && btnRelatorio.is(":visible")) {
                btnRelatorio.attr("disabled", "disabled");
            }
            if (btnHabilidade.length > 0 && btnHabilidade.is(":visible")) {
                btnHabilidade.attr("disabled", "disabled");
            }
        }
        else {
            if (txtNota.length > 0 && txtNota.is(":visible")) {
                txtNota.removeAttr("disabled");
            }
            else if (ddlPareceres.length > 0 && ddlPareceres.is(":visible")) {
                ddlPareceres.removeAttr("disabled");
            }
            else if (btnRelatorio.length > 0 && btnRelatorio.is(":visible")) {
                btnRelatorio.removeAttr("disabled");
            }
            if (btnHabilidade.length > 0 && btnHabilidade.is(":visible")) {
                btnHabilidade.removeAttr("disabled");
            }
        }

        if (causaBlurTxt) {
            if (txtNota.length > 0 && txtNota.is(":visible")) {
                txtNota.trigger('blur');
            }
            else if (ddlPareceres.length > 0 && ddlPareceres.is(":visible")) {
                var vNota = ddlPareceres[0].selectedIndex;
                var isNullNota = (vNota <= 0);
                HabilitaNotaRecuperacao(ddlPareceres, vNota - 1, isNullNota, 2);
            }
        }
    }
}

function CalculaNotaFinal(tipoCalculo) {

    // Calcula o valor da média final de acordo com o tipo de cálculo.
    $('.tbLancamentoAvaliacoes .gridRow, .tbLancamentoAvaliacoes .gridAlternatingRow').each(function () {
        var soma = 0;
        var qtNotas = 0;
        var txtNotaFinal = $(this).find('input[type="text"][id$="txtNotaFinal"]');

        $(this).find('input[type="text"]').each(function () {
            if ($(this).attr('id') != txtNotaFinal.attr('id') && !$(this).hasClass("Desconsiderar")) {
                // Soma o valor da linha.
                var notalinha = $(this).attr('value').replace(',', '.');
                soma += parseFloat((notalinha == 'NaN' || notalinha == '' ? 0 : notalinha));

                // Soma quantidade de notas somente para as participantes.
                if (($(this).parents("td").find("input[name$='chkParticipante']").length == 0) && ($(this).val() != "")
                    || (($(this).parents("td").find("input[name$='chkParticipante']").length > 0)
                    && ($(this).parents("td").find("input[name$='chkParticipante']").attr('checked') == true)
                    && ($(this).attr('disabled') == false)))                    
                    qtNotas++;
            }
        });

        if (qtNotas > 0) {
            var total = 0;
            if (tipoCalculo == "Media") {
                var calculo = soma / qtNotas;
                total = calculo.toFixed(qtdCasasDecimais);
            }
            else if (tipoCalculo == "Soma") {
                total = soma.toFixed(qtdCasasDecimais);
            }
            txtNotaFinal.val(ArredondarValor(total));

        } else {
            txtNotaFinal.val('');
        }

    });
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

function HabilitaNotaRecuperacao(idCampoNota, vNota, isNullNota, tipoNota) {
    if ($("[id$='hdnMinimoAprovacaoDocente']").length > 0) {
        var valorMinimoAprovacaoDocente = $("[id$='hdnMinimoAprovacaoDocente']").val();
        var parentTd = $(idCampoNota).parents("td");
        var idHdnTntIdPai = parentTd.find("[id$='hdnTntIdPai']");
        var idNota = parentTd.find("[id$='hdnTntId']");
        if (idHdnTntIdPai.length > 0 && idNota.length > 0 && idHdnTntIdPai.val() == "-1" && idNota.val() != "-1") {
            var idChkFalta = parentTd.find("[id$='chkFalta']");
            // Se for nota numerica, converto o valor para float
            var valorIdPai = $(idNota).val();
            var linhaAluno = $(idCampoNota).parents("tr");
            var relacionadas = $(linhaAluno).find("[id$='hdnTntIdPai'][value='" + valorIdPai + "']");
            for (var i = 0; i < relacionadas.length; i++) {
                var relacionadaTd = $(relacionadas[i]).parents("td");
                var campoNotaRecuperacao = tipoNota == 1 ? relacionadaTd.find("[id$='txtNota']") : relacionadaTd.find("[id$='ddlPareceres']");
                var idChkFaltaRecuperacao = relacionadaTd.find("[id$='chkFalta']");
                var idBtnHabilidade = relacionadaTd.find("[id$='btnHabilidade']");
                if (campoNotaRecuperacao.length > 0) {
                    var valorPermiteRecuperacaoQualquerNota = $("[id$='hdnPermiteRecuperacaoQualquerNota']").val();
                    var condicao = (idChkFalta.length == 0 || !idChkFalta.is(":checked"));
                    if (condicao && valorPermiteRecuperacaoQualquerNota == 'True') {
                        campoNotaRecuperacao.removeAttr("disabled");
                        if (idChkFaltaRecuperacao.length > 0)
                            idChkFaltaRecuperacao.removeAttr("disabled");
                        if (idBtnHabilidade.length > 0)
                            idBtnHabilidade.removeAttr("disabled");
                    }
                    else {
                        if (valorMinimoAprovacaoDocente != "") {
                            var minimoAprovacaoDocente = tipoNota == 1 ? parseFloat(valorMinimoAprovacaoDocente) : valorMinimoAprovacaoDocente;
                            // desabilito a nota de recuperacao se o aluno nao estava ausente
                            // e a nota na avaliacao normal foi acima da media.
                            var condicao1 = (idChkFalta.length == 0 || !idChkFalta.is(":checked"))
                                            && (isNullNota
                                                || (tipoNota == 1 && vNota >= minimoAprovacaoDocente)
                                                // para o parecer eh invertido, porque os conceitos estao ordenados do maior para o menor
                                                // e estamos validando pelo indice. Portanto, os conceitos de indice menor valem mais.
                                                || (tipoNota == 2 && vNota <= minimoAprovacaoDocente));

                            if (condicao1
                                // ou desabilito a nota de recuperacao se o aluno estava ausente na recuperacao.
                                || (

                                    idChkFaltaRecuperacao.length > 0
                                    && idChkFaltaRecuperacao.is(":checked")
                                )) {
                                campoNotaRecuperacao.val("").attr("disabled", "disabled");
                                // a nota na avaliacao normal foi acima da media, 
                                // desabilito tambem as outras opcoes do aluno para a avaliacao de recuperacao 
                                if (condicao1) {
                                    if (idChkFaltaRecuperacao.length > 0)
                                        idChkFaltaRecuperacao.attr("checked", false).attr("disabled", "disabled");
                                    if (idBtnHabilidade.length > 0)
                                        idBtnHabilidade.attr("disabled", "disabled");
                                }
                            }
                            else {
                                campoNotaRecuperacao.removeAttr("disabled");
                                if (idChkFaltaRecuperacao.length > 0)
                                    idChkFaltaRecuperacao.removeAttr("disabled");
                                if (idBtnHabilidade.length > 0)
                                    idBtnHabilidade.removeAttr("disabled");
                            }
                        }
                    }
                }
            }
        }
    }
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsLancamentoAvaliacoesGeral);
arrFNCSys.push(jsLancamentoAvaliacoesGeral);