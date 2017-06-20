function jsUCLancamentoRelatorioAtendimento() {
    createTabs("#divTabsRelatorio", "input[id$='txtSelectedTab']");

    $('#divTabsRelatorio').tabs({ selected: parseInt($("input[id$='txtSelectedTab']").val()) });
}

arrFNC.push(jsUCLancamentoRelatorioAtendimento);
arrFNCSys.push(jsUCLancamentoRelatorioAtendimento);