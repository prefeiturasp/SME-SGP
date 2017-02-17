function btnPrintClick() {
    $("#btnImprimir").unbind('click').click(function () {
        $("#divBoletinsAlunos").printThis({ pageTitle: $("#divBoletim h1:first").text(), printContainer: false });
    });
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(btnPrintClick);
arrFNCSys.push(btnPrintClick);