function jsCadastroFrequenciaExterna() {
    $('td.td-faltas input[type="text"]').unbind('change').bind('change', function () {
        var qtdFaltas = $(this).val();
        var qtdAulasPrevistas = $(this).parent('td').parent('tr').find('td.td-aulasPrevistas').html().trim();

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