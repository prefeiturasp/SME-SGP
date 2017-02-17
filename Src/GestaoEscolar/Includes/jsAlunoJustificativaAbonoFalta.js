function jsAlunoJustificativaAbonoFalta() {
    createDialog('#divCadastroJustificativaFalta', 555, 440);
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

arrFNC.push(jsAlunoJustificativaAbonoFalta);
arrFNCSys.push(jsAlunoJustificativaAbonoFalta);
