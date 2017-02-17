function jsHistoricoEscolar() {
    //createTabs("#divTabs", '', true);
    createDialog('.divAddHistorico', 900, 0);
    createDialog('.divProjAtivComplementar', 750, 0);
    createDialog('#divBuscaEscolaOrigem', 555, 0);
    createDialog('#divEscolaOrigem', 555, 0);
    createDialog('#divCadastroEscolaOrigem', 555, 0);
    createDialog('#divCadastroCidade', 750, 0);

    $(function () {
        if ($(".tbMunicipioAluno_incremental").val() != '') {
            $(".ddlEstadoAluno").attr('disabled', 'disabled');
        } else {
            $(".ddlEstadoAluno").removeAttr('disabled');
        }

        $(".tbMunicipioAluno_incremental").unbind('autocomplete').autocomplete({
            source: function (request, response) {
                WSServicos.BuscaCidades(request.term, function (data) {
                    var json = eval(data);
                    response($.map(json, function (item) { return { label: item.cid_unf_pai_nome, value: item.cid_nome, cid_id: item.cid_id, cid_unf_id: item.unf_id } }));
                });
            },
            minLength: 2,
            select: function (event, ui) {
                $(".tbCid_idMunicipioAluno_incremental").attr('value', ui.item.cid_id);
                $(".tbMunicipioAluno_incremental").attr('value', ui.item.cid_nome);
                $(".ddlEstadoAluno").attr('value', ui.item.cid_unf_id);
                $(".ddlEstadoAluno").attr('disabled', 'disabled');
                $(".tbMunicipioAluno_incremental").blur();
            },
            change: function (event, ui) {
                if (!ui.item) {
                    $(".tbCid_idMunicipioAluno_incremental").attr('value', "00000000-0000-0000-0000-000000000000");
                    $(".tbMunicipioAluno_incremental").attr('value', "");
                    $(".ddlEstadoAluno").removeAttr('disabled');
                    $(".ddlEstadoAluno").attr('value', "-1");
                }
            }
        });
    })
}

//// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsHistoricoEscolar);
arrFNCSys.push(jsHistoricoEscolar);
