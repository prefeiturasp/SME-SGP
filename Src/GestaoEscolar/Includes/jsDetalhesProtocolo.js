function jsDetalhesProtocolo() {
    createDialog('#divDetalhesProtAula', 750, 0);
    createDialog('#divDetalhesProtJustificativa', 750, 0);
    createDialog('#divDetalhesProtPlanejamento', 750, 0);
    createDialog('#divDetalhesProtFoto', 750, 0);
    createDialog('#divDetalhesProtCompensacaoDeAula', 750, 0);
}
// utilizado no Diario de Classes/Protocolos
// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsDetalhesProtocolo);