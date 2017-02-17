function jsCadastroTurma() {
    createTabs("#divTabs", '', true);
    createDialog('#divTipoAtendimentoTurma', 350, 200);
    createDialog('#divTipoAtendimentoEspecial', 500, 200);
    createDialog('.divVigenciaDocentesDis', 700, 0);
    createDialog('.divHistoricoDocenciaCompartilhada', 700, 0);

    //Manipula o uso da tool tip na div de avaliaçao
    $('.divAvaliacao').tooltip(
    {
        position: "center right",
        effect: "fade",
        events:
        {
            input: "click,blur"
        }
    });
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsCadastroTurma);
arrFNCSys.push(jsCadastroTurma);

function setaCheckBoxPeriodo(periodo) {
    $(document).ready(function () {
        $(periodo + ' input[type="checkbox"]').click(function () {
            if ($(this).attr('checked') == true)
                $(periodo).find('input[type="checkbox"]:not("#' + $(this).attr('id') + '")').attr('checked', !$(this).attr('checked'));
        });
    });
}