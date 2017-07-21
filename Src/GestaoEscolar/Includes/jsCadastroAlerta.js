function jsCadastroAlerta() {
    // Move a tela para o topo, quando tiver mensagem
    if ($("[id$='lblMessage']").length > 0 && $("[id$='lblMessage']").html() != "") {
        setTimeout('window.scrollTo(0,0);', 0);
    }
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsCadastroAlerta);
arrFNCSys.push(jsCadastroAlerta);