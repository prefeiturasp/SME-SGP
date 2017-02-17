function jsCadastroOrientacaoCurricular() {
    createDialog('#divCopiarOrientacao', 600, 200);
    createDialog('#divMsgSalvarDados', 400, 120);
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsCadastroOrientacaoCurricular);
arrFNCSys.push(jsCadastroOrientacaoCurricular);