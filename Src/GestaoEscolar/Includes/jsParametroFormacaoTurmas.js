function jsParametroFormacaoTurma() {
    var txtQtdDeficiente = "input[id$='txtQtdDeficiente']";
    var idCursoSelecionado = "input[id$='hdnParametroSelecionado']";

    $("#accordion").accordion({
        active: $(idCursoSelecionado).val() == '-1' ? false : parseInt($(idCursoSelecionado).val())
        , autoHeight: false
        , collapsible: true
        , change: function (event, ui) {
            var index = $(this).find("h3").index(ui.newHeader);
            $(idCursoSelecionado).val(index);
        }
    });

    $(txtQtdDeficiente).trigger('blur');
}

// Configura a visibilidade dos campos relacionados ao tipo de controle de capacidade com aluno deficiente.
// Se o tipo do controle for individual, cria os campos dinamicamente e carrega os dados, se existirem.
function HabilitaCapacidade(
    ComboTipoCapacidadeId
    , lblCapacidadeId
    , txtCapacidadeId
    , lblQtdDeficienteId
    , txtQtdDeficienteId
    , lblCapacidadeComDeficienteId
    , txtCapacidadeComDeficienteId
    , divCapacidadesId
    , rfvCapacidadeId
    , rfvQtdDeficienteId
    , rfvCapacidadeComDeficienteId
    , hdnIdCapacId
    , hdnCapacidadesId
    , TipoControleCapacidadeSemControle
    , TipoControleCapacidadeCapacidadeNormal
    , TipoControleCapacidadeCapacidadeNormalIndividual
    , PeriodoId
    , habilitarCampos) {
    var ComboTipoCapacidade = document.getElementById(ComboTipoCapacidadeId);
    
    var rfvCapacidade = document.getElementById(rfvCapacidadeId);
    var rfvQtdDeficiente = document.getElementById(rfvQtdDeficienteId);
    var rfvCapacidadeComDeficiente = document.getElementById(rfvCapacidadeComDeficienteId);

    // Desabilita validators para não serem ativados ao limpar os campos.
    if (rfvCapacidade != null) {
        ValidatorEnable(rfvCapacidade, false);
    }
    if (rfvQtdDeficiente != null) {
        ValidatorEnable(rfvQtdDeficiente, false);
    }
    if (rfvCapacidadeComDeficiente != null) {
        ValidatorEnable(rfvCapacidadeComDeficiente, false);
    }

    // Limpa os campos que não são obrigatórios para todos os tipos.
    var txtCapacidade = document.getElementById(txtCapacidadeId);
    if (txtCapacidade != null) {
        txtCapacidade.value = "";
    }
    var txtQtdDeficiente = document.getElementById(txtQtdDeficienteId);
    if (txtQtdDeficiente != null) {
        txtQtdDeficiente.value = "";
    }
    var txtCapacidadeComDeficiente = document.getElementById(txtCapacidadeComDeficienteId);
    if (txtCapacidadeComDeficiente != null) {
        txtCapacidadeComDeficiente.value = "";
    }

    switch (ComboTipoCapacidade.value) {
        case TipoControleCapacidadeSemControle:
            document.getElementById(lblCapacidadeId).style.display = "none";
            document.getElementById(lblQtdDeficienteId).style.display = "none";
            document.getElementById(lblCapacidadeComDeficienteId).style.display = "none";
            txtCapacidade.style.display = "none";
            txtQtdDeficiente.style.display = "none";
            txtCapacidadeComDeficiente.style.display = "none";
            document.getElementById(divCapacidadesId).style.display = "none";
            if (rfvCapacidade != null) {
                ValidatorEnable(rfvCapacidade, false);
            }
            if (rfvQtdDeficiente != null) {
                ValidatorEnable(rfvQtdDeficiente, false);
            }
            if (rfvCapacidadeComDeficiente != null) {
                ValidatorEnable(rfvCapacidadeComDeficiente, false);
            }
            break;
        case TipoControleCapacidadeCapacidadeNormal:
            document.getElementById(lblCapacidadeId).style.display = "";
            document.getElementById(lblQtdDeficienteId).style.display = "";
            document.getElementById(lblCapacidadeComDeficienteId).style.display = "";
            txtCapacidade.style.display = "";
            txtQtdDeficiente.style.display = "";
            txtCapacidadeComDeficiente.style.display = "";
            document.getElementById(divCapacidadesId).style.display = "none";
            if (rfvCapacidade != null) {
                ValidatorEnable(rfvCapacidade, true);
            }
            if (rfvQtdDeficiente != null) {
                ValidatorEnable(rfvQtdDeficiente, true);
            }
            if (rfvCapacidadeComDeficiente != null) {
                ValidatorEnable(rfvCapacidadeComDeficiente, true);
            }
            break;
        case TipoControleCapacidadeCapacidadeNormalIndividual:
            document.getElementById(lblCapacidadeId).style.display = "";
            document.getElementById(lblQtdDeficienteId).style.display = "";
            document.getElementById(lblCapacidadeComDeficienteId).style.display = "none";
            txtCapacidade.style.display = "";
            txtQtdDeficiente.style.display = "";
            txtCapacidadeComDeficiente.style.display = "none";
            document.getElementById(divCapacidadesId).style.display = "";
            if (rfvCapacidade != null) {
                ValidatorEnable(rfvCapacidade, true);
            }
            if (rfvQtdDeficiente != null) {
                ValidatorEnable(rfvQtdDeficiente, true);
            }
            if (rfvCapacidadeComDeficiente != null) {
                ValidatorEnable(rfvCapacidadeComDeficiente, false);
            }

            ConfiguraCamposIncluidos(divCapacidadesId, txtQtdDeficienteId, hdnIdCapacId, hdnCapacidadesId, PeriodoId, habilitarCampos);

            break;
    }
}

// Cria os campos relacionados ao tipo de controle individual e carrega os dados, se existirem.
function ConfiguraCamposIncluidos(divCapacidadesId, txtQtdDeficienteId, hdnIdCapacId, hdnCapacidadesId, PeriodoId, habilitarCampos) {
    var divCapacidades = document.getElementById(divCapacidadesId);
    var txtQtdDeficiente = document.getElementById(txtQtdDeficienteId);

    // Apaga todos os campos temporários criados, antes de gerar novos.
    while (divCapacidades.firstChild) {
        divCapacidades.removeChild(divCapacidades.firstChild);
    }

    for (var indice = 1; indice <= txtQtdDeficiente.value; indice++) {
        var linha = document.createElement("span");
        linha.innerHTML = "Capacidade máx. quando houver " + indice + " aluno(s) incluído(s) <span style='color:Red;'>*</span>";
        divCapacidades.appendChild(linha);

        linha = document.createElement("br");
        divCapacidades.appendChild(linha);

        // Criar textBox temporária.
        linha = document.createElement("input");
        linha.setAttribute("type", "hidden");
        linha.setAttribute("id", "txtIdTemporaria" + indice + "_" + PeriodoId);
        divCapacidades.appendChild(linha);

        // Criar textBox temporária.
        linha = document.createElement("input");
        linha.setAttribute("type", "textbox");
        linha.setAttribute("id", "txtCapacidadeTemporaria" + indice + "_" + PeriodoId);
        linha.setAttribute("maxLength", "4");
        linha.setAttribute("onchange", "RetornarCapacidades('" + txtQtdDeficiente.value + "','" + hdnIdCapacId + "','" + hdnCapacidadesId + "','" + PeriodoId + "')");
        linha.setAttribute("onKeyPress", "return MascaraNumeros()");
        if (Boolean.parse(habilitarCampos) == false) {
            linha.setAttribute("disabled", "false");
        }
        divCapacidades.appendChild(linha);

        linha = document.createElement("br");
        divCapacidades.appendChild(linha);
    }

    if (document.getElementById(hdnCapacidadesId).value.length > 0) {
        CarregarCapacidades(hdnIdCapacId, hdnCapacidadesId, PeriodoId);
    }
}

function MascaraNumeros() { 
    if (event.keyCode < 48 || event.keyCode > 57) { 
        return false; 
    } 
} 

// Carrega os dados referentes a capacidade com alunos deficientes.
function CarregarCapacidades(hdnIdCapacId, hdnCapacidadesId, PeriodoId) {
    var hdnIdCapac = document.getElementById(hdnIdCapacId);
    var hdnCapacidades = document.getElementById(hdnCapacidadesId);

    if (hdnCapacidades.value.length > 0) {
        var ids = hdnIdCapac.value.substring(0, hdnIdCapac.value.length - 1).split(";");
        var valores = hdnCapacidades.value.substring(0, hdnCapacidades.value.length - 1).split(";");
        for (var indice = 1; indice <= valores.length; indice++) {
            var campoCriadoId = document.getElementById("txtIdTemporaria" + indice + "_" + PeriodoId);
            if (campoCriadoId != null) {
                campoCriadoId.value += ids[indice - 1];
            }

            var campoCriado = document.getElementById("txtCapacidadeTemporaria" + indice + "_" + PeriodoId);
            if (campoCriado != null) {
                campoCriado.value += valores[indice - 1];
            }
        }
    }
}

// Atualiza o campo campo hidden com os novos valores de capacidade.
// É disparado no evento "blur" de cada campo criado dinamicamente.
function RetornarCapacidades(QtdDeficiente, hdnIdCapacId, hdnCapacidadesId, PeriodoId) {
    var ids = "";
    var valores = "";
    var campoCriado;

    // Busca por todos os campos criados dinamicamente e concatena seus valores.
    for (var indice = 1; indice <= QtdDeficiente; indice++) {
        campoCriado = document.getElementById("txtIdTemporaria" + indice + "_" + PeriodoId);
        ids += campoCriado.value + ";";

        campoCriado = document.getElementById("txtCapacidadeTemporaria" + indice + "_" + PeriodoId);
        valores += campoCriado.value + ";";
    }

    // Passa todos os valores para um campo criado estático, para que possa ser acessado.
    var hdnIdCapac = document.getElementById(hdnIdCapacId);
    hdnIdCapac.value = ids;

    var hdnCapacidades = document.getElementById(hdnCapacidadesId);
    hdnCapacidades.value = valores;
}

// Configura a visibilidade dos campos relacionados ao tipo de deficiência.
function HabilitaTipoDeficiencia(ComboTiposDeficienciaAlunoIncluidosId, listaTiposDeficiencia, TiposDeficienciaAlunoIncluidos) {
    var ComboTiposDeficienciaAlunoIncluidos = document.getElementById(ComboTiposDeficienciaAlunoIncluidosId);

    switch (ComboTiposDeficienciaAlunoIncluidos.value) {
        case TiposDeficienciaAlunoIncluidos:
            document.getElementById(listaTiposDeficiencia).style.display = "inline-block";
            break;
        default:
            document.getElementById(listaTiposDeficiencia).style.display = "none";
            break;
    }
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsParametroFormacaoTurma);
arrFNCSys.push(jsParametroFormacaoTurma);