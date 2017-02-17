function Funcao() {
    createDialog('#divAvaliacoes', 600, 0);
    createDialog('#divDisciplinas', 600, 0);
}

function TopoDaPagina() {
    scrollTo(0, 0);
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(Funcao);