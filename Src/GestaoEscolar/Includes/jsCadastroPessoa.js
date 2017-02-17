function jsCadastroPessoa() {
    createTabs("#divTabs", '', true);
    createDialog('#divBuscaPessoa', 555, 0);
    createDialog('#divBuscaCidade', 555, 450);
    createDialog('#divCidade', 750, 500);
    createDialog('#divCadastroCidade', 750, 0);
        
    $(function() {
        $(".tbNaturalidade_incremental").unbind('autocomplete').autocomplete({
            source: function(request, response) {
                WSServicos.BuscaCidades(request.term, function(data) {
                    var json = eval(data);
                    response($.map(json, function(item) { return { label: item.cid_unf_pai_nome, value: item.cid_nome, cid_id: item.cid_id} }));
                });
            },
            minLength: 2,
            select: function(event, ui) {
                $(".tbCid_idNaturalidade_incremental").attr('value', ui.item.cid_id);
                $(".tbNaturalidade_incremental").attr('value', ui.item.cid_nome);
            },
            change: function(event, ui) {
                if (!ui.item) {
                    $(".tbCid_idNaturalidade_incremental").attr('value', "00000000-0000-0000-0000-000000000000");
                    $(".tbNaturalidade_incremental").attr('value', "");
                }
            }
        });
    })
}


//// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsCadastroPessoa);
arrFNCSys.push(jsCadastroPessoa);
