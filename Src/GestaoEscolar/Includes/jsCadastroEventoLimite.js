function jsCadastroEventoLimite() {
    $("#accordion").accordion({
        autoHeight: false
        , collapsible: true
    });
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsCadastroEventoLimite);
arrFNCSys.push(jsCadastroEventoLimite);