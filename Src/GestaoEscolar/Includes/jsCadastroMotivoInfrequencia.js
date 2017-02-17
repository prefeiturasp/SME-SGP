function jsCadastroMotivoInfrequencia() {
    CriarSanfona();
}

// Cria o efeito sanfona com um determinado item selecionado.
function CriarSanfona() {

    var idAreaSelecionada = "input[id$='hdnAreaSelecionada']";

    $("#accordion").accordion({
        active: $(idAreaSelecionada).val() == '-1' ? false : parseInt($(idAreaSelecionada).val())
       // , autoHeight: true
        , collapsible: true
        , change: function (event, ui) {
            var index = $(this).find("h3").index(ui.newHeader);
            $(idAreaSelecionada).val(index);
        }
    });
}
// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsCadastroMotivoInfrequencia);
arrFNCSys.push(jsCadastroMotivoInfrequencia);