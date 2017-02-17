function mostraFiltroEscolas() {
    try {
        var mostrar = !$(idchkTodosColaboradores).attr('checked');

        $('#divFiltroEscolas').css('display', mostrar ? 'block' : 'none');
    }
    catch (e) {
    }
}

function jsBuscaColaboradores() {
    mostraFiltroEscolas();

    try {
        $(idchkTodosColaboradores).unbind('click').click(function () {
            mostraFiltroEscolas();
        });
    }
    catch (e) { }
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsBuscaColaboradores);
arrFNCSys.push(jsBuscaColaboradores);