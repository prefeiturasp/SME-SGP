function jsCadastroFrequenciaExterna() {
    $('td.td-faltas input[type="text"]').unbind('change').bind('change', function () {
        var qtdFaltas = $(this).val();
        var tr = $(this).parent('td').parent('tr');
        var index = tr.find('td.td-faltas').index($(this).parent('td'));
        var qtdAulasPrevistas = $(tr.find('td.td-aulasPrevistas').get(index)).html().trim();

        if (qtdAulasPrevistas != "-" && parseInt(qtdAulasPrevistas) < parseInt(qtdFaltas)) {
            $(this).parent('td').find('img[id$="imgAvisoAulasPrevistas"]').removeClass('hide');
        } else {
            $(this).parent('td').find('img[id$="imgAvisoAulasPrevistas"]').addClass('hide');
        }
    });
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsCadastroFrequenciaExterna);
arrFNCSys.push(jsCadastroFrequenciaExterna);