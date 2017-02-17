// Remove os nós que o IE9 cria com texto vazio dentro das tabelas.
function RemoveNosTextoVazioTabelasIE9() {
    if (($.browser.msie) && ($.browser.version == 9)) {
        var x = $('#aspnetForm').find('table');
        // Remove os nós que o IE9 cria com texto vazio.
        RemoveNosTextoVazio(x);
    }
}

// Remove os nós que o IE9 cria com texto vazio (chamar somente para tables).
function RemoveNosTextoVazio(nodes) {
    for (var i = 0; i < nodes.length; i++) {
        if (nodes[i].hasChildNodes())
            RemoveNosTextoVazio(nodes[i].childNodes);

        // Nó de texto vazio.
        if ((nodes[i].nodeValue != undefined) && (nodes[i].tagName == undefined) && (nodes[i].nodeName == "#text") &&
            nodes[i].nodeValue.replace(/^\s*/, "").replace(/\s*$/, "") == "") {
            nodes[i].parentNode.removeChild(nodes[i]);
            i--;
        }
    }
}