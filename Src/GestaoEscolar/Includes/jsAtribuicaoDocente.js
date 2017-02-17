function jsAtribuicaoDocente() {
    createDialog('#divBuscaDocente', 755, 0);

    var rows = $("table[id$='dgvTurma']").find("tr");
    for (var i = 0; i < rows.length; i++) {
        controlaVisibilidadePosicoes($(rows[i]));
        habilitarVigenciaSubstituto($(rows[i]));
    }

    $("input[id$='chkSubstituto']").unbind("change").change(function () {
        controlaVisibilidadePosicoes($(this).parents("tr"));
        habilitarVigenciaSubstituto($(this).parents("tr"));
    });

    $("input[id$='chkTitular']").unbind("change").change(function () {
        controlaVisibilidadePosicoes($(this).parents("tr"));
    });

    $("input[id$='chkSegundoTitular']").unbind("change").change(function () {
        controlaVisibilidadePosicoes($(this).parents("tr"));
    });
}

function habilitarVigenciaSubstituto(row) {
    var inicioSubstituto = row.find("[id$='txtVigenciaInicio']");
    if (inicioSubstituto.length > 0) {
        var chkSubstituto = row.find("[id$='chkSubstituto']");
        var fimSubstituto = row.find("[id$='txtVigenciaFim']");
        var calInicio = inicioSubstituto.next();
        var calFim = fimSubstituto.next();
        if (chkSubstituto.is(':checked')) {
            // habilita as datas de vigencia
            inicioSubstituto.removeAttr("disabled");
            fimSubstituto.removeAttr("disabled");
            calInicio.css("display", "block");
            calFim.css("display", "block");
            if (inicioSubstituto.val() == "") {
                var fullDate = new Date();
                var twoDigitMonth = (fullDate.getMonth() + 1) + ""; if (twoDigitMonth.length == 1) twoDigitMonth = "0" + twoDigitMonth;
                var twoDigitDate = fullDate.getDate() + ""; if (twoDigitDate.length == 1) twoDigitDate = "0" + twoDigitDate;
                var currentDate = twoDigitDate + "/" + twoDigitMonth + "/" + fullDate.getFullYear();
                inicioSubstituto.val(currentDate);
            }
        }
        else {
            // limpa e desabilita as datas de vigencia
            inicioSubstituto.val("").attr("disabled", "disabled");
            fimSubstituto.val("").attr("disabled", "disabled");
            calInicio.css("display", "none");
            calFim.css("display", "none");
        }
    }
}

function controlaVisibilidadePosicoes(row) {
    var chkTitular = row.find("[id$='chkTitular']");
    var chkSegundoTitular = row.find("[id$='chkSegundoTitular']");
    var chkSubstituto = row.find("[id$='_chkSubstituto']");

    if (chkTitular.is(':checked')) {
        chkSegundoTitular.attr("disabled", "disabled");
        chkSegundoTitular.attr('checked', false);
        chkSubstituto.attr("disabled", "disabled");
        chkSubstituto.attr('checked', false);
    } else if (chkSegundoTitular.is(':checked')) {
        chkTitular.attr("disabled", "disabled");
        chkTitular.attr('checked', false);
        chkSubstituto.attr("disabled", "disabled");
        chkSubstituto.attr('checked', false);
    } else if (chkSubstituto.is(':checked')) {
        chkTitular.attr("disabled", "disabled");
        chkTitular.attr('checked', false);
        chkSegundoTitular.attr("disabled", "disabled");
        chkSegundoTitular.attr('checked', false);
    } else if (!chkTitular.is(':checked') && !chkSegundoTitular.is(':checked') && !chkSubstituto.is(':checked')) {
        chkTitular.removeAttr("disabled");
        chkSegundoTitular.removeAttr("disabled");
        chkSubstituto.removeAttr("disabled");
    }
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsAtribuicaoDocente);
arrFNCSys.push(jsAtribuicaoDocente);