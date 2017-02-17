function jsUCAlunoEfetivacaoObservacao() {
    createTabs("#divTabs", '');

    $("textarea[name$='txtResumoDesempenho']").LimiteCaracteres(600);
    $("textarea[name$='txtRecomendacaoAluno']").LimiteCaracteres(600);
    $("textarea[name$='txtRecomendacaoResp']").LimiteCaracteres(700);

    $("input[name$='btnConfirmaDesempenho']").unbind('click').click(function (e) {
        e.preventDefault();
        var label = $(this).next(),
            textarea = $("textarea[name$='txtResumoDesempenho']");

        if (textarea.val() != "") {
            textarea.val(textarea.val() + '\n');
        }

        textarea.val(textarea.val() + label.text());
        textarea.keypress();
    });

    $("input[name$='btnConfirmaRecomendacao']").unbind('click').click(function (e) {
        e.preventDefault();
        var label = $(this).next(),
            textarea = $("textarea[name$='txtRecomendacaoAluno']");

        if (textarea.val() != "") {
            textarea.val(textarea.val() + '\n');
        }

        textarea.val(textarea.val() + label.text());
        textarea.keypress();
    });

    $("input[name$='btnConfirmaRecomendacaoResp']").unbind('click').click(function (e) {
        e.preventDefault();
        var label = $(this).next(),
            textarea = $("textarea[name$='txtRecomendacaoResp']");

        if (textarea.val() != "") {
            textarea.val(textarea.val() + '\n');
        }

        textarea.val(textarea.val() + label.text());
        textarea.keypress();
    });


    if ($("#divTabs select[name$='ddlResultado']").val() == "-1") {
        $("input[name$='imgJustificativaParecerConclusivo']").attr("disabled", "disabled");
    }
    else {
        $("input[name$='imgJustificativaParecerConclusivo']").removeAttr("disabled");
    }

    $("#divTabs select[name$='ddlResultado']").unbind("change").change(function (e) {
        e.preventDefault();
        var valorParecerConclusivo = $(this).val();

        if (valorParecerConclusivo == "-1") {
            $("input[name$='imgJustificativaParecerConclusivo']").attr("disabled", "disabled");
        }
        else {
            $("input[name$='imgJustificativaParecerConclusivo']").removeAttr("disabled");
        }
    });

    $(".frequencia input").each(function () {
        var valorArredondado = AplicaVariacaoFrequencia(parseFloat($(this).val().replace(",", ".")));
        $(this).val(valorArredondado);
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

//// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsUCAlunoEfetivacaoObservacao);
arrFNCSys.push(jsUCAlunoEfetivacaoObservacao);