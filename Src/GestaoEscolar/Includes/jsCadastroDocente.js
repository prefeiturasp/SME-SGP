function jsCadastroDocente() {

    createTabs("#divTabs", '.txtSelectedTab', true);
    createDialog('#divFormacoes', 555, 0);    
}
// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsCadastroDocente);


