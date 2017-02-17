function jsCadastroEscola() {
    createDialog('.divCurriculoCurso', 850, 0);
    createDialog('#divBuscaColaborador', 555, 0);
    createDialog('#divAlterarEndereco', 480, 0);
    createDialog('#divCidades', 400, 0);
    createDialog('#divCidadesCadastro', 300, 0);
    createDialog('#divColaborador', 555, 550);
    createDialog('#divCadastroRecursoFisicoDidatico', 450, 0);
    createDialog('#divOrgaoSupervisao', 575, 0);
    createDialog('#divTipoEnsino', 450, 0);
    createDialog('#divContatos', 555, 0);
    createDialog('#divCadastroDiretor', 575, 0);
    createDialog('#divBuscaCidade', 555, 450);
    createDialog('#divCurriculoTurno', 400, 0);
    createTabs("#divTabs", '#txtSelectedTab', true);    

    $("input[id$='btnCancelarCurriculoCurso']").unbind("click").click(function (e) {
        $(document).ready(function () { $('.divCurriculoCurso').dialog('close'); });
    });

    $("input[id$='btnVoltar']").unbind("click").click(function (e) {
        $(document).ready(function () { $('#divBuscaColaborador').dialog('close'); });
    });

    $(function () {
        $(".tbCidadeEscola_incremental").unbind('autocomplete').autocomplete({
            source: function (request, response) {
                WSServicos.BuscaCidades(request.term, function (data) {
                    var json = eval(data);
                    response($.map(json, function (item) { return { label: item.cid_unf_pai_nome, value: item.cid_nome, cid_id: item.cid_id} }));
                });
            },
            minLength: 2,
            select: function (event, ui) {
                $(".tbCid_idEscola_incremental").attr('value', ui.item.cid_id);
                $(".tbCidadeEscola_incremental").attr('value', ui.item.cid_nome);
            },
            change: function (event, ui) {
                if (!ui.item) {
                    $(".tbCid_idEscola_incremental").attr('value', "00000000-0000-0000-0000-000000000000");
                    $(".tbCidadeEscola_incremental").attr('value', "");
                }
            }
        });

        // Atualiza o nome do grupo dos radio buttons de colaboradores,
        // para o agrupamento por papel de diretor.
        var papeisDiretores = $("table[id$='grvPapeis'] tr");
        if (papeisDiretores.length > 0) {
            for (var i = 0; i < papeisDiretores.length; i++) {
                var colaboradoresPapel = $(papeisDiretores[i]).find("input[id$='rbPrincipal']");
                if (colaboradoresPapel.length > 0) {
                    var colaboradorChecado = $(papeisDiretores[i]).find("input[id$='rbPrincipal']:checked");
                    for (var j = 0; j < colaboradoresPapel.length; j++) {
                        var colaborador = $(colaboradoresPapel[j]);
                        colaborador.attr("nomeOriginal", colaborador.attr("name"));
                        colaborador.attr("name", colaboradorChecado.attr("name"));
                        colaborador.click(function () {
                            var papelDiretor = $(this).parents("tr");
                            //set name for all to name of clicked 
                            papelDiretor.find("input[id$='rbPrincipal']").attr("name", $(this).attr("nomeOriginal"));
                        });
                    }
                }
            }
        }
    });
}

function ExpandCollapse(divID, btn) {
    var div = $(btn).parents("tr").next(divID);

    if ($(div).css("display") == "none") {
        $(btn).removeClass('ui-icon ui-icon-circle-triangle-s');
        $(btn).addClass('ui-icon ui-icon-circle-triangle-n');

        $(div).css("display", "");
    }
    else {
        $(btn).removeClass('ui-icon ui-icon-circle-triangle-n');
        $(btn).addClass('ui-icon ui-icon-circle-triangle-s');

        $(div).css("display", "none");
    }
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsCadastroEscola);
arrFNCSys.push(jsCadastroEscola);
