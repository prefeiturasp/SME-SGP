function jsUCTransferencia() {

    // Nota numérica
    $(".notaNumericaTransferencia input").bind('blur', function () {
        var nota = $(this).attr('value');
        // Verifica se o campo nota não está vazio e se o parametro é verdadeiro
        if (nota != '') {

            try {
                // Tenta converter o valor do txt pra float.
                nota = parseFloat(nota.replace(',', '.'));

                if (!arredondamento) {
                    nota = nota.toFixed(numeroCasasDecimais);
                    $(this).attr('value', nota == 'NaN' ? "" : nota.replace('.', ','));
                }
            }
            catch (e) {
            }
        }

        if (arredondamento && $(this).val() != "") {
            $(this).val(ArredondarValor($(this).val()));
        }
    });

    $(".frequenciaTransferencia input").each(function () {
        if ($(this).val() != '' && $(this).val() != "NaN") {
            $(this).val(AplicaVariacaoFrequencia(parseFloat($(this).val().replace('.', ','))));
        }
    });


    $(".frequenciaTransferencia input").unbind('blur').bind('blur', function () {
        var valor = $(this).val();
        if (valor != '' && valor != "NaN") {
            $(this).val(AplicaVariacaoFrequencia(parseFloat(valor.replace('.', ','))));
        }
    });
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

    if (!isNaN(result)) {
        return result.toFixed(numeroCasas);
    }
    else {
        return "";
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

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsUCTransferencia);
arrFNCSys.push(jsUCTransferencia);