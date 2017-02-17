function btnPrintClick() {
    $("#btnImprimir").unbind('click').click(function () {
        $("#divBoletim").printThis({ pageTitle: $("#divBoletim h1").text()});
    });
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(btnPrintClick);
arrFNCSys.push(btnPrintClick);