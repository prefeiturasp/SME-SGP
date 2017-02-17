function JS_Login() {

    $('#divEsqueciSenha').unbind('dialog').dialog({
        bgiframe: true,
        autoOpen: false,
        resizable: true,
        modal: true,
        width: 550,
        closeOnEscape: false,
        open: function(type, data) {
            $('#divEsqueciSenha').parent().appendTo($("form:first"));
        },
        close: {},
        buttons: {}
    });
    $(window).unbind('scroll').scroll(function() {
        $("#divEsqueciSenha").dialog("option", "position", "center");
    });
    $('#divAlterarSenha').unbind('dialog').dialog({
        bgiframe: true,
        autoOpen: false,
        resizable: true,
        modal: true,
        width: 375,
        closeOnEscape: false,
        open: function (type, data) {
            $('#divAlterarSenha').parent().appendTo($("form:first"));
            $('#divAlterarSenha').keypress(function (e) { clickButton(e, "#" + $("input[id$='_btnSalvar']")[0].id); });
        },
        close: function (type, data) {
            $('#login').keypress(function (e) { clickButton(e, "#" + $("input[id$='_btnEntrar']")[0].id); });
        },
        buttons: {}
    });
    $(window).unbind('scroll').scroll(function () {
        $("#divAlterarSenha").dialog("option", "position", "center");
    });
    $('#divSelectGrupo').unbind('dialog').dialog({
        bgiframe: true,
        autoOpen: false,
        resizable: true,
        modal: true,
        width: 450,
        open: function (type, data) {
            $('#divSelectGrupo').parent().appendTo($("form:first"));
        },
        buttons: {}
    });
    $(window).unbind('scroll').scroll(function () {
        $("#divSelectGrupo").dialog("option", "position", "center");
    });

    $('#login').unbind('keypress').keypress(function (e) { clickButton(e, "#" + $("input[id$='_btnEntrar']")[0].id); });

}

//Setão botão padrão por javascript
function clickButton(e, buttonid) {
    if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
        $(buttonid).click();
        return false;
    } else {
        return true;
    }
}

function cvSenhaAtual_ClientValidate(sender, args) {
    $.ajax({
        type: "POST",
        url: "Login.aspx/ValidarSenhaAtual",
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

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(JS_Login);