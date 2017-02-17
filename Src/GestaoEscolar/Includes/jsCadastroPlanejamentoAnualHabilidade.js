function jsCadastroPlanejamentoAnualHabilidade() {

    createDialog('#divFiltrosRelatorio', 600, 0);
    createDialog('#divTextoGrande', 700, 400);
    createDialog('#divReplicarPlanejamento', 600, 0);
    createDialog('.divLancamentoAlcance', 700, 0);
    createDialog('.divSelecionaDisciplina', 725, 0);

    $(idtableAlunos).tablesorter({
        headers: {
            // assign the secound column (we start counting zero) 
            2: {
                // disable it by setting the property sorter to false 
                sorter: false
            }
        }
    });

    $(idDdlOrdenacaoAlunos).unbind('change').change(function () {
        Ordena($(this), idtableAlunos);
    });
    
    $(idDdlOrdenacaoAlunos).trigger('change');

    createTabs("#divTabs", "input[id='txtSelectedTab']");

    var selected_tab = 1;
    $(function () {
        var tabs = $("#divTabs").tabs({
            select: function (e, i) {
                selected_tab = i.index;
            }
        });
        selected_tab = $("[id$=selected_tab]").val() != "" ? parseInt($("[id$=selected_tab]").val()) : 0;
        tabs.tabs('select', selected_tab);
        $("form").submit(function () {
            $("[id$=selected_tab]").val(selected_tab);
        });
    });

    $('.divTreeviewScrollPlanBim .OrientacaoPlanejada input[type="checkbox"]').unbind('click').click(function () {
        chkOrientacaoTrabalhadASetEnabled(this);
        coloreOrientacaoPlanejamento(this, '.OrientacaoPlanejada', corPlanejado);
        coloreOrientacaoPlanejamento(this, '.OrientacaoTrabalhada', corTrabalhado);
    });

    $('.divTreeviewScrollPlanBim .OrientacaoTrabalhada input[type="checkbox"]').unbind('click').click(function () {
        imgmarcarAlcancadoSetEnabled(this);
        coloreOrientacaoPlanejamento(this, '.OrientacaoTrabalhada', corTrabalhado);
    });

    $('div.hitarea').unbind('click').click(function () {
        toggleArvorePlanejamento(this);
    });

    if (($.browser.mozilla) && ($.browser.version == '9.0.1')) {
        var sheet = document.createElement('style')
        sheet.innerHTML = ".divArvorePlanAnual, x:-moz-any-link, x:default { " +
        "width: 230px!important; " +
		"height: 200px; " +
		"position: absolute; " +
		"background: #efefef; " +
		"right: 0; " +
		"z-index:1; " +
        "margin: 10px; " +
        "top:-20px!important; }";
        document.body.appendChild(sheet);

        var sheet2 = document.createElement('style')
        sheet2.innerHTML = ".divTreeLayoutBim, x:-moz-any-link, x:default { " +
        "width: 362px!important; " +
		"height: 220px; " +
		"position: absolute; " +
		"background: #efefef; " +
		"right: 0px!important; " +
		"z-index:1; " +
        "margin: 10px; " +
        "top:-20px!important; }";
        document.body.appendChild(sheet2);

        //var element = document.getElementById('dvTreeLayoutPA');
        //element.style.margin = '10px';
        //element.style.top = '-10px!important';         
        //document.styleSheets[document.styleSheets.length-1].insertRule('.divTreeLayoutPA, x:-moz-any-link, x:default { margin: 10px; top:-10px!important; }',
        //document.styleSheets[document.styleSheets.length-1].cssRules.length)
    }

    //Adiciona página de confirmação caso o usuário tente sair da tela
    SetExitPageConfirmer();
}

function chkOrientacaoTrabalhadASetEnabled(checkbox) {
    var trabalhado = $(checkbox).parent('span').parent('span').parent('div').find('.OrientacaoTrabalhada input');
    var marcarAlcancado = $(checkbox).parent('span').parent('span').parent('div').find('.OrientacaoMarcarAlcancado');
    var orientacaoPlanejada = $(checkbox).attr('checked');

    if (!orientacaoPlanejada) {
        trabalhado.attr('disabled', true).removeAttr('checked');
        marcarAlcancado.attr('disabled', true);
    }
    else {
        trabalhado.removeAttr('disabled').parent('span.OrientacaoTrabalhada').removeAttr('disabled');
    }
}

function imgmarcarAlcancadoSetEnabled(checkbox) {
    var marcarAlcancado = $(checkbox).parent('span').parent('span').parent('div').find('.OrientacaoMarcarAlcancado');
    var orientacaoTrabalhada = $(checkbox).attr('checked');

    if (!orientacaoTrabalhada) {
        marcarAlcancado.attr('disabled', true);
    }
    else {
        marcarAlcancado.removeAttr('disabled');
    }
}

function montaTreeviewPlanejamento() {
    //Abre os nós com coisa checada.
    $('.divTreeviewScrollPlanBim .OrientacaoPlanejada input[type="checkbox"]:checked').parents('li').each(function () {
        $(this).parents('ul').css('display', 'block');
        $(this).children('div.hitarea').first().removeClass('expandable-hitarea').addClass('collapsable-hitarea');
    });

    $('.divTreeviewScrollPlanAnual .OrientacaoAlcancada input[type="checkbox"]:checked').parents('li').each(function () {
        $(this).parents('ul').css('display', 'block');
        $(this).children('div.hitarea').first().removeClass('expandable-hitarea').addClass('collapsable-hitarea');
    });

    // a classe "semMensagem" é para não gerar a mensagem das abas, caso a treeview esteja dentro de abas.
    $('.divTreeviewScrollPlanAnual ul, .divTreeviewScrollPlanBim ul').addClass("semMensagem");

    $('ul.treeview li:last-child').addClass('last');

    $(".expandable:not(:has(ul))").css({ display: "table", width:"100%"});

    $('.OrientacaoPlanejada input[type="checkbox"]:checked').each(function () {
        coloreOrientacaoPlanejamento(this, '.OrientacaoPlanejada', corPlanejado);
    });

    $('.OrientacaoTrabalhada input[type="checkbox"]:checked').each(function () {
        coloreOrientacaoPlanejamento(this, '.OrientacaoTrabalhada', corTrabalhado);
    });

    $('.OrientacaoPlanejada input[type="checkbox"]:not(:checked)').each(function () {
        chkOrientacaoTrabalhadASetEnabled(this);
    });

    $('.OrientacaoTrabalhada input[type="checkbox"]:not(:checked)').each(function () {
        imgmarcarAlcancadoSetEnabled(this);
    });

    $('.OrientacaoPlanejadaAula input[type="checkbox"]').each(function () {
        $(this).attr('disabled', true);
    });
}

function MostraCkbPeriodo(checked) {
    if (checked) {
        $('#divCkb').find('div').first().css({ display: 'block' });
        $(idckbTodasAulas).attr('checked', 'true');
    }
    else {
        $('#divCkb').find('div').first().css({ display: 'none' });
        $(idckbPeriodo).attr('checked', '');
        $(idckbTodasAulas).attr('checked', '');
        $('#divInicioFim').find('div').first().css({ display: 'none' });
        $(idTxtInicio).attr('value', '');
        $(idTxtFim).attr('value', '');
    }
}

function CheckTodasAulas(checked) {
    if (checked) {
        $('#divInicioFim').find('div').first().css({ display: 'none' });
        $(idTxtInicio).attr('value', '');
        $(idTxtFim).attr('value', '');
        $(idckbPeriodo).attr('checked', '');
    }
    else {
        $(idckbTodasAulas).attr('checked', 'true');
    }
}

function MostraTextBox(checked) {
    if (checked) {
        $('#divInicioFim').find('div').first().css({ display: 'block' });
        $(idckbTodasAulas).attr('checked', '');
    }
    else {
        $(idckbPeriodo).attr('checked', 'true');
    }
}

function coloreOrientacaoPlanejamento(checkbox, seletor, cor) {
    var chave = $(checkbox).parent('span').parent('span').parent('div').find('input[id$="hdnChave"]').val();
    var ids = chave.split(';');
    var tud_id = ids[0];
    var ocr_id = ids[1];
    var tpc_id = ids[2];
    var tpc_ordem = $('div#divTabs-' + tpc_id).find('input[id$="hdnTpcOrdem"]').val();

    $('div[id*="divTabs-"]')
    .filter(function () {
        return $(this).find('input[id$="hdnTpcOrdem"]').val() > tpc_ordem;
    })
    .find(seletor + ' input[type="checkbox"]')
    .filter(function () {
        var chave2 = $(this).parent('span').parent('span').parent('div').find('input[id$="hdnChave"]').val();
        return ocr_id == chave2.split(';')[1] && tud_id == chave2.split(';')[0];
    }).each(function () {
        var marcacaoAnterior = VerificaBimAnterior(this, seletor);

        //$(this).parent('span').parent('span').parent('div').css('color', marcacaoAnterior == 2 ? corTrabalhado : (marcacaoAnterior == 1 ? corPlanejado : ''));
        
        if (seletor == '.OrientacaoPlanejada') {
            $(this).parent('span').parent('span').css('background', marcacaoAnterior >= 1 ? cor : '').css("color", contrastColor(cor));
        } else {
            $(this).parent('span').parent('span').css('background', marcacaoAnterior == 2 ? cor : '').css("color", contrastColor(cor));
        }
    });

    //$('span.nivelAprendizado').css('color', 'black');
}

function VerificaBimAnterior(checkbox, seletor) {
    var chave = $(checkbox).parent('span').parent('span').parent('div').find('input[id$="hdnChave"]').val();
    var ids = chave.split(';');
    var tud_id = ids[0];
    var ocr_id = ids[1];
    var tpc_id = ids[2];
    var tpc_ordem = $('div#divTabs-' + tpc_id).find('input[id$="hdnTpcOrdem"]').val();

    var checados =
    $('div[id*="divTabs-"]')
    .filter(function () {
        return $(this).find('input[id$="hdnTpcOrdem"]').val() < tpc_ordem;
    })
    .find('input[type="checkbox"]:checked')
    .filter(function () {
        var chave2 = $(this).parent('span').parent('span').parent('div').find('input[id$="hdnChave"]').val();
        return ocr_id == chave2.split(';')[1] && tud_id == chave2.split(';')[0];
    });

    var qtdePlanejados = checados.filter(function () { return $(this).parent('span').hasClass('OrientacaoPlanejada'); }).size();
    var qtdeTrabalhados = checados.filter(function () { return $(this).parent('span').hasClass('OrientacaoTrabalhada'); }).size();

    // 0 - nada marcado
    // 1 - planejado
    // 2 - trabalhado
    return qtdeTrabalhados > 0 ? 2 : (qtdePlanejados > 0 ? 1 : 0);
}

//abre ou fecha o nó
function toggleArvorePlanejamento(div) {
    $(div).parent('li').find('ul').each(function () {
        var node = $(this);
        var display = node.css('display');
        if (display == 'none') {
            node.css('display', 'block');
            node.find('ul').css('display', 'block');
        } else if (display == 'block') {
            node.css('display', 'none');
            node.find('ul').css('display', 'none');
            node.find('div.hitarea').addClass('expandable-hitarea').removeClass('collapsable-hitarea');
        }
    });

    if ($(div).hasClass('collapsable-hitarea'))
        $(div).removeClass('collapsable-hitarea').addClass('expandable-hitarea');
    else if ($(div).hasClass('expandable-hitarea'))
        $(div).removeClass('expandable-hitarea').addClass('collapsable-hitarea');
}

function Ordena(controle, tabela) {
    var opcao = controle.find('option:selected').attr('value');
    var sorting;
    if (opcao == "0") {
        // Número de chamada.
        sorting = [[0, 0]];
    }
    else if (opcao == "1") {
        // Nome do aluno
        sorting = [[1, 0]];
    }
    $(tabela).trigger("sorton", [sorting]);
    return false;
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsCadastroPlanejamentoAnualHabilidade);
arrFNCSys.push(jsCadastroPlanejamentoAnualHabilidade);

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(montaTreeviewPlanejamento);
arrFNCSys.push(montaTreeviewPlanejamento);