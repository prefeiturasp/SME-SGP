function jsCadastroRelatorioRP() {
    // Move a tela para o topo, quando tiver mensagem
    if ($("[id$='lblMensagem']").length > 0 && $("[id$='lblMensagem']").html() != "") {
        setTimeout('window.scrollTo(0,0);', 0);
    }
    $(document).ready(function () {
        if ($("[id$='btnSalvar']").length > 0) {
            SetExitPageConfirmer();
        }
    });
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsCadastroRelatorioRP);
arrFNCSys.push(jsCadastroRelatorioRP);