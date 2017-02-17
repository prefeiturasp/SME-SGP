function jsCadastroColaborador() {

    createTabs("#divTabs", '.txtSelectedTab', true);

    createDialog('#divCargos', 750, 0);
    createDialog('#divFuncoes', 555, 0);
    createDialog('#divBuscaUA', 555, 0);
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsCadastroColaborador);
arrFNCSys.push(jsCadastroColaborador);
