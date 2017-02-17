function jsCadastroAlunoMatricula() {

    createDialog('#divBuscaAluno', 755, 0);  
    createDialog('#divCadastroCidade', 555, 0);
    createDialog('#divBuscaResponsavel', 555, 0);
    createDialog('#divDuplicidadeFonetica', 555, 0);
    createDialog('#divAlunoExistente', 555, 0);
    createDialog('#divProtocoloExcedente', 555, 0);
    createDialog('.divNumeroMat', 555, 0);

    //nao mostra o X da janela de dialogo
    $('div[aria-labelledby*=divNumeroMat] .ui-dialog-titlebar-close').hide();
    $('.divNumeroMat').dialog("option", "closeOnEscape", false);

    $(function () {
        $(".tbNaturalidade_incremental").unbind('autocomplete').autocomplete({
            source: function (request, response) {
                WSServicos.BuscaCidades(request.term, function (data) {
                    var json = eval(data);
                    response($.map(json, function (item) { return { label: item.cid_unf_pai_nome, value: item.cid_nome, cid_id: item.cid_id} }));
                });
            },
            minLength: 2,
            select: function (event, ui) {
                $(".tbCid_idNaturalidade_incremental").attr('value', ui.item.cid_id);
                $(".tbNaturalidade_incremental").attr('value', ui.item.cid_nome);
            },
            change: function (event, ui) {
                if (!ui.item) {
                    $(".tbCid_idNaturalidade_incremental").attr('value', "00000000-0000-0000-0000-000000000000");
                    $(".tbNaturalidade_incremental").attr('value', "");
                }
            }
        });
    })

    var idFdsMatricula = $("fieldset[id$='_fdsMatricula']");
    var idTxtDataNasc = idFdsMatricula.find("input[id$='_txtDataNasc']");
    if (idTxtDataNasc.length > 0)
    {
        idTxtDataNasc.unbind('blur').bind('blur', function () {
            var txtDataNasc = $(this);
            var idGrvExcedente = "table[id$='_grvExcedente']";
            var lstImgDefasagemSerie = $(idGrvExcedente).find("[id$='imgDefasagemSerie']");
            var hdnDataAtual = $("input[id$='hdnDataAtual']");
            var esconderDefasagem = false;
            if (txtDataNasc.val().length == 10
                && hdnDataAtual.length > 0
                && hdnDataAtual.val().length == 10) {
                var strDataAtual = hdnDataAtual.val().split("/", 3);
                var strDataNasc = txtDataNasc.val().split("/", 3);
                var dataAtual = new Date(strDataAtual[2], strDataAtual[1] - 1, strDataAtual[0]);
                var dataNasc = new Date(strDataNasc[2], strDataNasc[1] - 1, strDataNasc[0]);
                // calculo a idade em meses, a partir da data de nascimento
                if (dataAtual > dataNasc) {
                    // compute difference in total months
                    var meses = 12 * (dataAtual.getFullYear() - dataNasc.getFullYear()) + (dataAtual.getMonth() - dataNasc.getMonth());
                    if (dataAtual.getDate() < dataNasc.getDate()) {
                        meses--;
                    }
                    var idade = meses;
                    // percorro o grid de excedentes para validar a defasagem de serie
                    for (var i = 0; i < lstImgDefasagemSerie.length; i++) {
                        var imgDefasagemSerie = $(lstImgDefasagemSerie[i]);
                        if (imgDefasagemSerie.length > 0) {
                            var td = imgDefasagemSerie.parents("td");
                            var idadeMaxima = parseInt(td.find("[id$='hdnIdadeMaxima']").val());
                            if (idadeMaxima > 0) {
                                var idadeMinima = parseInt(td.find("[id$='hdnIdadeMinima']").val());
                                if (idade > idadeMaxima || idade < idadeMinima) {
                                    // mostra o icone de alerta de defasagem
                                    imgDefasagemSerie.css("display", "inline-block");
                                }
                                else {
                                    imgDefasagemSerie.css("display", "none");
                                }
                            }
                            else {
                                imgDefasagemSerie.css("display", "none");
                            }
                        }
                    }
                }
                else {
                    esconderDefasagem = true;
                }
            }
            else {
                esconderDefasagem = true;
            }
            if (esconderDefasagem) {
                // percorro o grid de excedentes escondendo a defasagem de serie
                for (var i = 0; i < lstImgDefasagemSerie.length; i++) {
                    var imgDefasagemSerie = $(lstImgDefasagemSerie[i]);
                    if (imgDefasagemSerie.length > 0) {
                        imgDefasagemSerie.css("display", "none");
                    }
                }
            }
        });
        $(idTxtDataNasc).trigger('blur');
    }

    //Adiciona página de confirmação caso o usuário tente sair da tela
    SetExitPageConfirmer();
}

function jsDivDeficiencia() {
    var divRecursos, divEquipamentos, divPossui;
    var alturas = new Array();
    var alturaMaior = 0;

    divRecursos = $(".Recursos");
    divEquipamentos = $(".Equipamentos");
    divPossui = $(".Possui");

    alturas[0] = divRecursos.height();
    alturas[1] = divEquipamentos.height();
    alturas[2] = divPossui.height();

    for (var i = 0; i < 3; i++) {
        if (alturas[i] > alturaMaior) {
            alturaMaior = alturas[i];
        }
    }

    divRecursos.height(alturaMaior);
    divEquipamentos.height(alturaMaior);
    divPossui.height(alturaMaior);
}

arrFNC.push(jsCadastroAlunoMatricula);
arrFNC.push(jsDivDeficiencia);
arrFNCSys.push(jsCadastroAlunoMatricula);