//Métodos
function SetaValidator(idValidator, enable) {
    // Habilitar/desabilitar validator na tela.
    var rfvValidator = document.getElementById(idValidator.replace('#', ''));
    ValidatorEnable(rfvValidator, enable);
}

function configuraValidacao(idTxt1, idTxt2, idTxt3, idTxt4, idTxt5, id1, id2, id3, id4, id5) {
    // Habilita/desabilita validator de acordo com o preenchimento de outros campos obrigatórios.
    if (($('#' + idTxt1).attr('value').length == 0)
        && ($('#' + idTxt2).attr('value').length == 0)
        && ($('#' + idTxt3).attr('value').length == 0)
        && ($('#' + idTxt4).attr('value').length == 0)
        && ($('#' + idTxt5).attr('value').length == 0)
    ) {
        SetaValidator('#' + id1, false);
        SetaValidator('#' + id2, false);
        SetaValidator('#' + id3, false);
        SetaValidator('#' + id4, false);
        SetaValidator('#' + id5, false);
    }
    else {
        SetaValidator('#' + id1, true);
        SetaValidator('#' + id2, true);
        SetaValidator('#' + id3, true);
        SetaValidator('#' + id4, true);
        SetaValidator('#' + id5, true);
    }
}

function jsUCCadastroEndereco() {

    $.mask.rules.g = /[0-9.-]/;

    // mascara do e-mail
    $(".maskCoordenadas").setMask({ mask: 'g', type: 'repeat', fixedChars: null, selectCharsOnFocus: false, autoTab: false });

    $(function() {
        $(".tbLogradouro_incremental").unbind('autocomplete').autocomplete({
            source: function(request, response) {
                WSServicos.BuscaEnderecosLogradouro($('#' + $(this)[0].element[0].id).parent().find(".tbCep_incremental").val(), request.term, function(data) {
                    var json = eval(data);
                    response($.map(json, function(item) { return { label: item.end_endereco, value: item.end_logradouro, end_id: item.end_id, end_cep: item.end_cep, end_logradouro: item.end_logradouro, end_distrito: item.end_distrito, end_zona: item.end_zona, end_bairro: item.end_bairro, cid_id: item.cid_id, cid_nome: item.cid_nome} }));
                });
            },
            minLength: 2,
            select: function(event, ui) {
                $(this).parent().find(".tbEnd_id_incremental").attr('value', ui.item.end_id);
                $(this).parent().find(".tbCep_incremental").attr('value', ui.item.end_cep);
                $(this).parent().find(".tbLogradouro_incremental").attr('value', ui.item.end_logradouro);
                $(this).parent().find(".tbDistrito_incremental").attr('value', ui.item.end_distrito);
                $(this).parent().find(".tbZona_incremental").attr('value', ui.item.end_zona == 0 ? "-1" : ui.item.end_zona);
                $(this).parent().find(".tbBairro_incremental").attr('value', ui.item.end_bairro);
                $(this).parent().find(".tbCid_id_incremental").attr('value', ui.item.cid_id);
                $(this).parent().find(".tbCidade_incremental").attr('value', ui.item.cid_nome);

                $(this).parent().find(".tbDistrito_incremental").attr('readOnly1', "readOnly");
                $(this).parent().find(".tbZona_incremental").attr('disabled', "true");
                $(this).parent().find(".tbBairro_incremental").attr('readOnly1', "readOnly");
                $(this).parent().find(".tbCidade_incremental").attr('readOnly1', "readOnly");

                $(this).parent().find(".tbDistrito_incremental")
                .unbind('keydown', down).keydown(down);

                $(this).parent().find(".tbBairro_incremental")
                .unbind('keydown', down).keydown(down);

                $(this).parent().find(".tbCidade_incremental")
                .unbind('keydown', down).keydown(down);

            },
            change: function(event, ui) {
                if (!ui.item) {
                    $(this).parent().find(".tbEnd_id_incremental").attr('value', "00000000-0000-0000-0000-000000000000");
                    $(this).parent().find(".tbLogradouro_incremental").attr('value', this.value);
                    $(this).parent().find(".tbDistrito_incremental").attr('value', "");
                    $(this).parent().find(".tbZona_incremental").attr('value', "-1");
                    $(this).parent().find(".tbBairro_incremental").attr('value', "");
                    $(this).parent().find(".tbCid_id_incremental").attr('value', "00000000-0000-0000-0000-000000000000");
                    $(this).parent().find(".tbCidade_incremental").attr('value', "");

                    $(this).parent().find(".tbDistrito_incremental").removeAttr('readOnly1');
                    $(this).parent().find(".tbZona_incremental").removeAttr('disabled');
                    $(this).parent().find(".tbBairro_incremental").removeAttr('readOnly1');
                    $(this).parent().find(".tbCidade_incremental").removeAttr('readOnly1');

                    $(this).parent().find(".tbDistrito_incremental").unbind('keydown', down);
                    $(this).parent().find(".tbBairro_incremental").unbind('keydown', down);
                    $(this).parent().find(".tbCidade_incremental").unbind('keydown', down);
                }
            }
        });
    });

    createAutocompleteCidade();

    $('input[readOnly1="readOnly"]')
    .unbind('keydown', down).keydown(down);

    $("input[id$='chkPrincipal']").click(function () {
        var checado = $(this).attr("checked");
        $("input[id$='chkPrincipal']").attr("checked", false);
        if (checado == true)
            $(this).attr("checked", true);
    });
}

function down(event) {
    if (event.keyCode != 9)
        event.preventDefault();
}

function createAutocompleteCidade() {
    $(function() {
        $(".tbCidade_incremental").unbind('autocomplete').autocomplete({
            source: function(request, response) {
                WSServicos.BuscaCidades(request.term, function(data) {
                    var json = eval(data);
                    response($.map(json, function(item) { return { label: item.cid_unf_pai_nome, value: item.cid_nome, cid_id: item.cid_id} }));
                });
            },
            minLength: 2,
            select: function(event, ui) {
                $(this).parent().find(".tbCid_id_incremental").attr('value', ui.item.cid_id);
                $(this).parent().find(".tbCidade_incremental").attr('value', ui.item.cid_nome);
            },
            change: function(event, ui) {
                if (!ui.item) {
                    $(this).parent().find(".tbCid_id_incremental").attr('value', "00000000-0000-0000-0000-000000000000");
                    $(this).parent().find(".tbCidade_incremental").attr('value', "");
                }
            }
        });
    });
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsUCCadastroEndereco);
arrFNCSys.push(jsUCCadastroEndereco);