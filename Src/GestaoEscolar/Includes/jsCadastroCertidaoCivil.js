function jsCadastroCertidaoCivil() {
    $(function() {
        $(".tbCidadeCertidao_incremental").unbind('autocomplete').autocomplete({
            source: function(request, response) {
                WSServicos.BuscaCidades(request.term, function(data) {
                    var json = eval(data);
                    response($.map(json, function(item) { return { label: item.cid_unf_pai_nome, value: item.cid_nome, cid_id: item.cid_id} }));
                });
            },
            minLength: 2,
            select: function(event, ui) {
                $(this).parent().find(".tbCid_idCertidao_incremental").attr('value', ui.item.cid_id);
                $(this).parent().find(".tbCidadeCertidao_incremental").attr('value', ui.item.cid_nome);
            },
            change: function(event, ui) {
                if (!ui.item) {
                    $(this).parent().find(".tbCid_idCertidao_incremental").attr('value', "00000000-0000-0000-0000-000000000000");
                    $(this).parent().find(".tbCidadeCertidao_incremental").attr('value', "");
                }
            }
        });
    })
}
// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsCadastroCertidaoCivil);
arrFNCSys.push(jsCadastroCertidaoCivil);