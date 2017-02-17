function jsUCInformacoesComplementares() {
    createDialog('.divCadastroObservacao', 700, 0);
    createDialog('.divCadastroCertificado', 700, 0);

    //$("input[name$='btnConfirmaObservacoesPadroes']").unbind('click').click(function (e) {
    //    e.preventDefault();
    //    var label = $(this).next(),
    //        textarea = $("textarea[name$='txtObservacao']");

    //    //if (textarea.val() != "") {
    //    //    textarea.val(textarea.val() + '\n');
    //    //}

    //    //textarea.val(textarea.val() + label.text());
    //    textarea.val(label.text());
    //    textarea.keypress();
    //});
}

arrFNC.push(jsUCInformacoesComplementares);
arrFNCSys.push(jsUCInformacoesComplementares);