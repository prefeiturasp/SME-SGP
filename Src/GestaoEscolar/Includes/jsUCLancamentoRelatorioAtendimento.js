function jsUCLancamentoRelatorioAtendimento() {
    $(document).ready(function () {
        createTabs("#divTabsRelatorio", "input[id$='txtSelectedTab']");
        $('#divTabsRelatorio').tabs({ selected: parseInt($("input[id$='txtSelectedTab']").val()) });
    });

    $('input[type="radio"]').unbind('click').bind('click', function () {
        var id = $(this).attr('id');
        $(this).parent('span').parent('div.questionario-resposta').parent('div.questionario-conteudo').find('input[type="radio"]').each(function () {
            if ($(this).attr('id') == id) {
                $(this).attr('checked', 'checked');
            } else {
                $(this).removeAttr('checked');
                $(this).parent('span').siblings('input[type="text"].questionario-conteudo-resposta-texto-adicional').val('');
            }
        });
    });

     $('input[type="checkbox"]').unbind('click').bind('click', function () {
         if (!$(this).attr('checked')) {
             $(this).parent('span').siblings('input[type="text"].questionario-conteudo-resposta-texto-adicional').val('');
         }
    });


}

arrFNC.push(jsUCLancamentoRelatorioAtendimento);
arrFNCSys.push(jsUCLancamentoRelatorioAtendimento);