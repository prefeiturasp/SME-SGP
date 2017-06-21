function jsUCLancamentoRelatorioAtendimento() {
    $(document).ready(function () {
        createTabs("#divTabsRelatorio", "input[id$='txtSelectedTab']");
        $('#divTabsRelatorio').tabs({ selected: parseInt($("input[id$='txtSelectedTab']").val()) });
    });

    $("[type=radio]").each(function (i) {
        var name = $(this).attr("name");
        var splitted = name.split("$");
        $(this).attr("name", splitted[splitted.length - 1]);
    });

}

arrFNC.push(jsUCLancamentoRelatorioAtendimento);
arrFNCSys.push(jsUCLancamentoRelatorioAtendimento);