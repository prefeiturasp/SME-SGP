function btnPrintClick() {
    $("#btnImprimir").unbind('click').click(function() {
        $("#divDeclaracao").printThis({ pageTitle: "Impressão" });
    });
}

 // Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(btnPrintClick);
arrFNCSys.push(btnPrintClick);



