function jsFechamentoGestor() {
    createDialog('#divDadosTurma', 950, 0);

    // Mostra a mensagem apenas se o botao de salvar estiver visivel.
    var btnSalvar = $("input[id$='btnSalvar']");
    if (btnSalvar.length > 0 && btnSalvar.is(":visible")) {
        execute = false;
        SetExitPageConfirmer();
    }

    // Nota numérica
    $('td.td-notas input[type="text"]').unbind('blur').bind('blur', function () {
        NotaBlur($(this));
    });

    $(document).ready(function () {
        createTabs("#divTabs", "input[id$='txtSelectedTab']");
        $('#divTabs').tabs({
            selected: parseInt($("input[id$='txtSelectedTab']").val())
        });
    });

    ////$(document).ready(function () {
    //    $("a[id$='btnExpandir']").removeAttr('disabled');

    //    // mantem expandido apos o postback
    //    var idExpandido = "input[id$='hfExpandido'][value='1']";
    //    $(idExpandido).bind('blur', function () {
    //        // aberto
    //        ExpandCollapse('.trExpandir', $(this).prev("[id$='btnExpandir']"));
    //    });
    //    $(idExpandido).trigger('blur');

    //    var idExpandidoTodos = "input[id$='hfExpandidoTodos'][value='1']";
    //    $(idExpandidoTodos).bind('blur', function () {
    //        var btnExpandir = $(this).prev("[id$='btnExpandir']");
    //        var toolTip = $(btnExpandir).attr("title").toString();
    //        $(btnExpandir).removeClass('ui-icon ui-icon-circle-triangle-s');
    //        $(btnExpandir).addClass('ui-icon ui-icon-circle-triangle-n');
    //        if (toolTip.indexOf("Expandir") == 0) {
    //            $(btnExpandir).attr("title", toolTip.replace("Expandir", "Recolher"));
    //        }
    //    });
    //    $(idExpandidoTodos).trigger('blur');
    ////});
}

function NotaBlur(txtNota) {
    var nota = $(txtNota).val();
    var formatacaoNota = $("input[id$='hdnFormatacaoNota']").val().split(';');
    var numCasasDecimais = parseInt(formatacaoNota[0]);

    // Verifica se o campo nota não está vazio e se o parametro é verdadeiro
    if (nota != '') {
        if (formatacaoNota[1] == "true") {
            var variacaoEscalaAvaliacao = formatacaoNota[2];
            $(txtNota).val(ArredondarValor($(txtNota).val(), numCasasDecimais, variacaoEscalaAvaliacao));
        }
        else {
            try {
                // Tenta converter o valor do txt pra float.
                nota = parseFloat(nota.replace(',', '.'));
                nota = nota.toFixed(numCasasDecimais);
                $(txtNota).val(nota == 'NaN' ? "" : nota.replace('.', ','));
            }
            catch (e) {
            }
        }
    }
}

function ArredondarValor(valornota, numCasasDecimais, variacaoEscalaAvaliacao) {
    var total = valornota.toString().replace(",", ".");
    var aux = 0;
    var totalArredondado = 0;
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

    return (total == "NaN" ? "" : totalArredondado.toFixed(numCasasDecimais).toString().replace('.', ','));
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsFechamentoGestor);
arrFNCSys.push(jsFechamentoGestor);