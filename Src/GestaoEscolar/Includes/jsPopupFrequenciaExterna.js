function jsPopupFrequenciaExterna() {
    createDialog('.divFrequenciaExterna', 600, 0);

}

function AbrePopupFrequenciaExterna(aulas, faltas) {
    $('.divFrequenciaExterna').dialog('open');
    $('.divFrequenciaExterna').find('[id$=lblQtAulasExterna]').text(aulas);
    $('.divFrequenciaExterna').find('[id$=lblQtFaltasExterna]').text(faltas);
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsPopupFrequenciaExterna);
arrFNCSys.push(jsPopupFrequenciaExterna);