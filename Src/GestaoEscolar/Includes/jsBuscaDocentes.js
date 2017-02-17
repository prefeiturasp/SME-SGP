function mostraFiltroEscolas() {
    try {
        var mostrar = !$(idchkTodosDocentes).attr('checked');

        $('#divFiltroEscolas').css('display', mostrar ? 'block' : 'none');
    }
    catch (e) {
    }
}

function jsBuscaDocentes() {
    mostraFiltroEscolas();
    
    try {
        $(idchkTodosDocentes).unbind('click').click(function() {
            mostraFiltroEscolas();
        });
    }
    catch (e) { }
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsBuscaDocentes);
arrFNCSys.push(jsBuscaDocentes);