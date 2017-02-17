function jsUCConfirmaOperacao() {
    createDialog('#divConfirmacao', 555, 0);
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsUCConfirmaOperacao);
arrFNCSys.push(jsUCConfirmaOperacao);