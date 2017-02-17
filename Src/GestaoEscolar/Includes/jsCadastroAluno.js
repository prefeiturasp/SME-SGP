var idNotaDisciplina = 'td.colunaNotaDisciplina input[type="text"]';
var idFrequenciaDisciplina = 'td.colunaFrequenciaDisciplina input[type="text"]';


function jsCadastroAluno() {

    createTabs("#divTabs", "input[id$='txtSelectedTab']");
    
    $('#divTabs').tabs({ selected:parseInt($("input[id$='txtSelectedTab']").val())});

    createDialog('#divContatoResponsavel', 555, 0);
    createDialog('#divMatricula', 555, 0);
    createDialog('#divHistorico', 840, 0);
    createDialog('#divDisciplina', 555, 0);
    createDialog('#divObservacao', 555, 0);
    createDialog('#divBuscaEscolaOrigem', 555, 0);
    createDialog('#divEscolaOrigem', 555, 0);
    createDialog('#divDuplicidadeFonetica', 555, 0);
    createDialog('#divAlunoExistente', 555, 0);
    createDialog('#divBuscaResponsavel', 550, 0);
    createDialog('#divCadastroDisciplina', 555, 0);
    createDialog('#divCadastroObservacao', 560, 0);
    createDialog('#divCadastroEscolaOrigem', 555, 0);
    createDialog('.divNumeroMat', 555, 0);
    createDialog('#divCadastroAvaliacaoObservacao', 560, 0);
    createDialog('#divUploadAnexo', 555, 0);
    createDialog('#divProtocoloExcedente', 555, 0);
    createDialog('#divCartVacinacao', 840, 0);

    //nao mostra o X da janela de dialogo e o ESC
    $('div[aria-labelledby*=divNumeroMat] .ui-dialog-titlebar-close').hide();
    $('.divNumeroMat').dialog("option", "closeOnEscape", false);

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
                    response($.map(json, function (item) { return { label: item.cid_unf_pai_nome, value: item.cid_nome, cid_id: item.cid_id, cid_unf_id: item.unf_id} }));
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

    // Configurar notas das disciplina do histórico escolar.
    // Apenas se estiver habilitado a validações do histórico.
    // E o tipo de origem da escola é dentro da rede
    $(idNotaDisciplina).bind('blur', function () {

        var nota = $(this).attr('value');

        if (realizarValidacaoHistorico) {
            $(this).attr('MaxLength', 4);
            if (nota != '') {
                try {
                    nota = parseFloat(nota.replace(',', '.'));
                  //  nota = nota.toFixed(1);
                    $(this).attr('value', nota == 'NaN' ? "" : nota.replace('.', ','));

                }
                catch (e) {
                }
            }
        }

    });

    // Configurar os campos de frequência das disciplinas do histórico.
    // Apenas se estiver habilitado a validações do histórico.
    $(idFrequenciaDisciplina).bind('blur', function () {

        if (realizarValidacaoHistorico) {
            $(this).attr('MaxLength', 3);
            $(this).attr('obrigatorio', true);
        }
        else {
            $(this).attr('MaxLength', 7);
            $(this).attr('obrigatorio', false);
        }
    });


    $(idNotaDisciplina).trigger('blur');
    $(idFrequenciaDisciplina).trigger('blur');

    var idDivCadastroPessoa = $("div[id$='_updCadastroPessoa']");
    var idTxtDataNasc = idDivCadastroPessoa.find("input[id$='_txtDataNasc']");
    if (idTxtDataNasc.length > 0) {
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

function escondeComponente(Check1Id, Check2Id) {
    //$(componenteId).checked = true;
    var ckbCheck1 = document.getElementById(Check1Id.id);
    var ckbCheck2 = document.getElementById(Check2Id.id);

    if (ckbCheck1.checked) {
        ckbCheck2.checked = false;
    }
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

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsCadastroAluno);
arrFNC.push(jsDivDeficiencia);
arrFNCSys.push(jsCadastroAluno);
arrFNCSys.push(jsDivDeficiencia);
