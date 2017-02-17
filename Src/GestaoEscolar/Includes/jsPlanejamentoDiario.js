function jsPlanejamentoDiario() {
    createDialog('#divModalMensagem', 500, 0);
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsPlanejamentoDiario);
arrFNCSys.push(jsPlanejamentoDiario);