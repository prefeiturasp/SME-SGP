function jsMeusDados() {
    $(".senha input").attr('autocomplete', 'off');

    $("input[id$='btnSalvar']").unbind("click").bind("click", function () {
        // remove validacoes repetidas
        $("div[id$='ValidationSummary1']").html(GetUnique($("div[id$='ValidationSummary1']").html().split('<br>')).join('<br>'));
        if ($("div[id$='ValidationSummary1']").text() != '') {
            $("div[id$='ValidationSummary1']").css('display', 'block');
        }
    });

    $("input[id$='txtNovaSenha']").unbind("keyup").bind("keyup", function () {
        if (permiteAlterarEmail) {
            if ($(this).val() != "") {
                document.getElementById(idRfvConfNovaSenha).enabled = true;
                document.getElementById(idRevNovaSenhaFormato).enabled = true;
                document.getElementById(idRevNovaSenhaTamanho).enabled = true;
                document.getElementById(idCpvNovaSenha).enabled = true;
                document.getElementById(idCpvConfNovaSenha).enabled = true;
                document.getElementById(idCvNovaSenhaHistorico).enabled = true;
            } else {
                document.getElementById(idRfvConfNovaSenha).enabled = false;
                document.getElementById(idRevNovaSenhaFormato).enabled = false;
                document.getElementById(idRevNovaSenhaTamanho).enabled = false;
                document.getElementById(idCpvNovaSenha).enabled = false;
                document.getElementById(idCpvConfNovaSenha).enabled = false;
                document.getElementById(idCvNovaSenhaHistorico).enabled = false;
            }
        }
    });
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

function cvSenhaAtual_ClientValidate(sender, args) {
    $.ajax({
        type: "POST",
        url: "MeusDados.aspx/ValidarSenhaAtual",
        data: '{ "senhaAtual": "' + args.Value + '", "usu_id": "' + usu_id + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            args.IsValid = data.d;
        },
        error: function (data, success, error) {
            args.IsValid = false;
        }
    });

    return;
}

function cvEmailExistente_ClientValidate(sender, args) {
    $.ajax({
        type: "POST",
        url: "MeusDados.aspx/ValidarEmailExistente",
        data: '{ "email": "' + args.Value + '", "usu_id": "' + usu_id + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            args.IsValid = data.d;
        },
        error: function (data, success, error) {
            args.IsValid = false;
        }
    });

    return;
}

function cvNovaSenhaHistorico_ClientValidate(sender, args) {
    $.ajax({
        type: "POST",
        url: "MeusDados.aspx/ValidarHistoricoSenha",
        data: '{ "novaSenha": "' + args.Value + '", "usu_id": "' + usu_id + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            args.IsValid = data.d;
        },
        error: function (data, success, error) {
            args.IsValid = false;
        }
    });

    return;
}

arrFNC.push(jsMeusDados);
arrFNCSys.push(jsMeusDados);