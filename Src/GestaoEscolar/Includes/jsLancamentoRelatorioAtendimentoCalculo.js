function jsLancamentoRelatorioAtendimentoCalculo() {
    if ($('#resultadoCalculo').size() > 0)
    {
        $('.questionario-conteudo').find('input[type=checkbox],input[type=radio]').each(function () {
            $(this).bind('click', function () {
                CalculoSoma($(this).parents('fieldset').first());
            });
        });

        $('#divTabsRelatorio').find('fieldset').each(function () {
            CalculoSoma($(this));
        });
    }
}

function CalculoSoma(fieldset)
{
    var soma = 0;
    fieldset.find('.questionario-resposta').each(function () {
        $(this).find('input[type=checkbox]:checked,input[type=radio]:checked').each(function () {
            var peso = $(this).parents('.questionario-resposta').find('input[id$=hdnPeso]').first();
            if (peso != undefined && peso.val() != "")
            {
                soma += parseFloat(peso.val().replace(',','.'));
            }
        });
    });

    soma = soma.toString().replace('.',',');
    fieldset.find('#resultadoCalculo').attr('innerText', soma);
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsLancamentoRelatorioAtendimentoCalculo);
arrFNCSys.push(jsLancamentoRelatorioAtendimentoCalculo);
