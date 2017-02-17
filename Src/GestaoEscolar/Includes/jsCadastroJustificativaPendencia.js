function jsCadastroJustificativaPendencia() {
    //Adiciona página de confirmação caso o usuário tente sair da tela
    exibeMensagemNavegadorComJQuery = false;
    SetExitPageConfirmer();
    AjustarCssPeriodos();
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

function AjustarCssPeriodos()
{
    // Altera o css do check dos períodos do calendário, para não ficar cortado no IE.
    var periodos = $(".checkboxlist-columns .checkbox");
    var numPeriodos = periodos.length;
    if (numPeriodos > 0 && numPeriodos < 4) {
        var strNumPeriodos = numPeriodos.toString();
        var paiPeriodos = $(".checkboxlist-columns");
        paiPeriodos.css("-webkit-column-count", strNumPeriodos);
        paiPeriodos.css("-moz-column-count", strNumPeriodos);
        paiPeriodos.css("column-count", strNumPeriodos);
    }
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsCadastroJustificativaPendencia);