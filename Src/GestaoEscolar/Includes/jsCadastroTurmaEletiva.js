function jsCadastroTurmaEletiva() {

    createDialog('.divVigenciaDocentesDis', 700, 0);

    $("table[id$='chkPeriodosCalendario']").find('input[type="checkbox"]').unbind('click').click(function () { 
        var checked = $(this).attr('checked');
        var qt = $("table[id$='chkPeriodosCalendario']").find('input[type="checkbox"]:checked').size();
        
        if ((qt < 2) && (checked)) {
            $(this).parents('td').next('td').find('input[type="checkbox"]').attr('checked', 'true');
        }
    });

    try {
        if (idDdlSituacao != null) {
            $(idDdlSituacao).unbind('change').change(function() {
                mostraMensagemInativacao();
            });

            SetConfirmDialogButton(idBtnSalvar,
                '<b>Ao mudar a situação da turma para encerrada, todas as matrículas dos alunos serão inativadas. </b><br /><br />Deseja confirmar a operação?');

            mostraMensagemInativacao();
        }
    }
    catch (e) {
    }

    //Manipula o uso da tool tip na div de periodos do calendario
    $('.divPeriodosCalendario').tooltip(
    {
        position: "center right",
        effect: "fade",
        events:
        {
            input: "click,blur"
        }
    });
}

function mostraMensagemInativacao() {

    var valor = $(idDdlSituacao + ' option:selected').attr('value');

    // Variável que controla se a janela de confirmação será aberta.
    execute = (valor != 5);
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsCadastroTurmaEletiva);
arrFNCSys.push(jsCadastroTurmaEletiva);