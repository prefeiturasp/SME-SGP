function jsControleTurma_Listao() {
    if ($("#divHabilidadesRelacionadas").length > 0) {
        createDialog("#divHabilidadesRelacionadas", 600, 0);
    }
    if ($("#divRelatorio").length > 0) {
        createDialog("#divRelatorio", 555, 0);
    }
    if ($("#divCompensacao").length > 0) {
        createDialog("#divCompensacao", 600, 0);
    }
    if ($("#divAtividadeAvaliativa").length > 0) {
        createDialogCloseWithConfirmation("#divAtividadeAvaliativa", 760, 0);
    }

    createTabs("#divTabsListao", "input[id$='hdnListaoSelecionado']");
    RemoveNosTextoVazioTabelasIE9();

    if (idbtnCompensacaoAusencia != "") {
        $(idbtnCompensacaoAusencia).css('display',
            $('input[id$=hdnListaoSelecionado]').val() == "0"
            ? "inline-block" : "none");
    }

    $(".sortableFrequencia,.sortableFrequenciaTerritorio,.sortableAvaliacoes,.sortableAtividadeExtra").tablesorter();

    if (idDdlOrdenacaoFrequencia != "") {
        $(idDdlOrdenacaoFrequencia).unbind('change').change(function () {
            Ordena($(this), ".sortableFrequencia");
        });
    }

    if (idDdlOrdenacaoFrequenciaTerritorio != "") {
        $(idDdlOrdenacaoFrequenciaTerritorio).unbind('change').change(function () {
            Ordena($(this), ".sortableFrequenciaTerritorio");
        });
    }

    if (idDdlOrdenacaoAvaliacao != "") {
        $(idDdlOrdenacaoAvaliacao).unbind('change').change(function () {
            Ordena($(this), ".sortableAvaliacoes");
        });
    }

    if (idDdlOrdenacaoAtivExtra != "") {
        $(idDdlOrdenacaoAtivExtra).unbind('change').change(function () {
            Ordena($(this), ".sortableAtividadeExtra");
        });
    }
           

    $(".sortableFrequencia").bind("sortEnd", function () {
        var col = $('.sortableFrequencia').find('.tablesorter-headerSortDown');
        if (col.size() > 0) {
            $(idhdnOrdenacaoFrequencia).val(col.attr('data-column') + ",0");// Ascendente
        }
        var col = $('.sortableFrequencia').find('.tablesorter-headerSortUp');
        if (col.size() > 0) {
            $(idhdnOrdenacaoFrequencia).val(col.attr('data-column') + ",1");// Decrescente
        }

        $(this).find('.gridRow, .gridAlternatingRow').each(function () {
            var linha = $(this);
            var indicePar = (linha.index() % 2) == 0;

            if (indicePar && linha.hasClass('gridAlternatingRow')) {
                linha.removeClass('gridAlternatingRow').addClass('gridRow');
            }

            if (!indicePar && linha.hasClass('gridRow')) {
                linha.removeClass('gridRow').addClass('gridAlternatingRow');
            }
        });

    });

    $(".sortableFrequenciaTerritorio").bind("sortEnd", function () {
        var col = $('.sortableFrequenciaTerritorio').find('.tablesorter-headerSortDown');
        if (col.size() > 0) {
            $(idhdnOrdenacaoFrequenciaTerritorio).val(col.attr('data-column') + ",0");// Ascendente
        }
        var col = $('.sortableFrequenciaTerritorio').find('.tablesorter-headerSortUp');
        if (col.size() > 0) {
            $(idhdnOrdenacaoFrequenciaTerritorio).val(col.attr('data-column') + ",1");// Decrescente
        }

        $(this).find('.gridRow, .gridAlternatingRow').each(function () {
            var linha = $(this);
            var indicePar = (linha.index() % 2) == 0;

            if (indicePar && linha.hasClass('gridAlternatingRow')) {
                linha.removeClass('gridAlternatingRow').addClass('gridRow');
            }

            if (!indicePar && linha.hasClass('gridRow')) {
                linha.removeClass('gridRow').addClass('gridAlternatingRow');
            }
        });

    });

    $(".sortableAvaliacoes").bind("sortEnd", function () {
        var col = $('.sortableAvaliacoes').find('.tablesorter-headerSortDown');
        if (col.size() > 0) {
            $(idhdnOrdenacaoAvaliacao).val(col.attr('data-column') + ",0");// Ascendente
        }
        var col = $('.sortableAvaliacoes').find('.tablesorter-headerSortUp');
        if (col.size() > 0) {
            $(idhdnOrdenacaoAvaliacao).val(col.attr('data-column') + ",1");// Decrescente
        }

        $(this).find('.gridRow, .gridAlternatingRow').each(function () {
            var linha = $(this);
            var indicePar = (linha.index() % 2) == 0;

            if (indicePar && linha.hasClass('gridAlternatingRow')) {
                linha.removeClass('gridAlternatingRow').addClass('gridRow');
            }

            if (!indicePar && linha.hasClass('gridRow')) {
                linha.removeClass('gridRow').addClass('gridAlternatingRow');
            }
        });

    });

    $(".sortableAtividadeExtra").bind("sortEnd", function () {
        var col = $('.sortableAtividadeExtra').find('.tablesorter-headerSortDown');
        if (col.size() > 0) {
            $(idhdnOrdenacaoAtivExtra).val(col.attr('data-column') + ",0");// Ascendente
        }
        var col = $('.sortableAtividadeExtra').find('.tablesorter-headerSortUp');
        if (col.size() > 0) {
            $(idhdnOrdenacaoAtivExtra).val(col.attr('data-column') + ",1");// Decrescente
        }

        $(this).find('.gridRow, .gridAlternatingRow').each(function () {
            var linha = $(this);
            var indicePar = (linha.index() % 2) == 0;

            if (indicePar && linha.hasClass('gridAlternatingRow')) {
                linha.removeClass('gridAlternatingRow').addClass('gridRow');
            }

            if (!indicePar && linha.hasClass('gridRow')) {
                linha.removeClass('gridRow').addClass('gridAlternatingRow');
            }
        });

    });

    if (idhdnOrdenacaoFrequencia != "" && $(idhdnOrdenacaoFrequencia).length > 0) {
        if ($(idhdnOrdenacaoFrequencia).val() == "") {
            $(idDdlOrdenacaoFrequencia).trigger('change');
        }
        else {
            var col = parseInt($(idhdnOrdenacaoFrequencia).val().split(",")[0]);
            var ord = parseInt($(idhdnOrdenacaoFrequencia).val().split(",")[1]);
            var sorting = [[col, ord]];
            $('.sortableFrequencia').trigger("sorton", [sorting]);
        }
    }

    if (idhdnOrdenacaoFrequenciaTerritorio != "" && $(idhdnOrdenacaoFrequenciaTerritorio).length > 0) {
        if ($(idhdnOrdenacaoFrequenciaTerritorio).val() == "") {
            $(idDdlOrdenacaoFrequenciaTerritorio).trigger('change');
        }
        else {
            var col = parseInt($(idhdnOrdenacaoFrequenciaTerritorio).val().split(",")[0]);
            var ord = parseInt($(idhdnOrdenacaoFrequenciaTerritorio).val().split(",")[1]);
            var sorting = [[col, ord]];
            $('.sortableFrequenciaTerritorio').trigger("sorton", [sorting]);
        }
    }

    if (idhdnOrdenacaoAvaliacao != "" && $(idhdnOrdenacaoAvaliacao).length > 0) {
        if ($(idhdnOrdenacaoAvaliacao).val() == "") {
            $(idDdlOrdenacaoAvaliacao).trigger('change');
        }
        else {
            var col = parseInt($(idhdnOrdenacaoAvaliacao).val().split(",")[0]);
            var ord = parseInt($(idhdnOrdenacaoAvaliacao).val().split(",")[1]);
            var sorting = [[col, ord]];
            $('.sortableAvaliacoes').trigger("sorton", [sorting]);
        }
    }

    if (idhdnOrdenacaoAtivExtra != "" && $(idhdnOrdenacaoAtivExtra).length > 0) {
        if ($(idhdnOrdenacaoAtivExtra).val() == "") {
            $(idDdlOrdenacaoAtivExtra).trigger('change');
        }
        else {
            var col = parseInt($(idhdnOrdenacaoAtivExtra).val().split(",")[0]);
            var ord = parseInt($(idhdnOrdenacaoAtivExtra).val().split(",")[1]);
            var sorting = [[col, ord]];
            $('.sortableAtividadeExtra').trigger("sorton", [sorting]);
        }
    }

    if (($.browser.mozilla) && ($.browser.version == '9.0.1')) {
        var sheet2 = document.createElement('style')
        sheet2.innerHTML = ".divTreeLayout, x:-moz-any-link, x:default { " +
        "width: 362px!important; " +
		"height: 220px; " +
		"position: absolute; " +
		"background: #efefef; " +
		"right: 0px!important; " +
		"z-index:1; " +
        "margin: 10px; " +
        "top:-20px!important; }";
        document.body.appendChild(sheet2);
    }

    $('div.hitarea').unbind('click').click(function () {
        toggleArvore(this);
    });

    //$("#divTabsListao ul li a").click(function () {
    //    // esconde o botao de salvar caso seja a aba de plano de aula
    //    $("[id$='btnSalvar'],[id$='btnSalvarCima']").css('visibility', $("[id$='pnlPlanoAula']").parent().attr("aria-hidden") == 'true' ? 'visible' : 'hidden');
    //});

    //Adiciona página de confirmação caso o usuário tente sair da tela
    SetExitPageConfirmer();

    if ($('.fixedLeftColumnWrapper').length > 0) {
        FixedLeftColumn.CenterHeaderVertical = true;
        FixedLeftColumn.CenterRowVertical = true;
        FixedLeftColumn.UpdateLayout();
        $(window).resize(function () {
            FixedLeftColumn.UpdateWrapper();
        });
    }

    if ($("[id$='cblQualificadorAtividade']").length > 0) {
        $("[id$='cblQualificadorAtividade']").find("input[type='checkbox']").unbind('change').change(function () {
            var check = $(this);
            aplicarFiltroAvaliacao(check.parent().attr("valor"), check.is(":checked"));
        });
        filtrarAvaliacao();

        if ($("#tabela").length > 0) {
            $("#tabela th, #tabela td").css("visibility", "visible");
        }
    }

    $(document).ready(function () {
        var alteraFrequencia = function () {
            $('input[name$="hdnAlterouFrequencia"]').val("1");
        }

        var alteraNota = function () {
            $('input[name$="hdnAlterouNota"]').val("1");
        }

        var alteraPlano = function () {
            $('input[name$="hdnAlterouPlanoAula"]').val("1");
        }

        var alteraAtividadeExtra = function () {
            $('input[name$="hdnAlterouAtividadeExtra"]').val("1");
        }

        $('input[name*="UCLancamentoFrequencia"]').unbind('change').change(alteraFrequencia);
        $('input[name*="UCLancamentoFrequencia"]').unbind('click').click(alteraFrequencia);

        $('input[name*="rptAlunosAvaliacao"]').unbind('click').click(alteraNota);
        $('input[name*="rptAlunosAvaliacao"], select[name*="rptAlunosAvaliacao"]').unbind('change').change(alteraNota);
        $('input[name*="rptAlunosAvaliacao"]').unbind('keyup').keyup(alteraNota);
        $('input[name$="btnSalvarHabilidadesRelacionadas"], input[name$="btnSalvarRelatorio"]').unbind('click').click(alteraNota);

        $('input[name*="rptPlanoAula"]').unbind('click').click(alteraPlano);
        $('input[name*="rptPlanoAula"]').unbind('change').change(alteraPlano);
        $('input[name*="rptPlanoAula"]').unbind('keyup').keyup(alteraPlano);
        $('textarea[name*="rptPlanoAula"]').unbind('keyup').keyup(alteraPlano);

        $('input[name*="rptAlunoAtivExtra"]').unbind('click').click(alteraAtividadeExtra);
        $('input[name*="rptAlunoAtivExtra"], select[name*="rptAlunoAtivExtra"]').unbind('change').change(alteraAtividadeExtra);
        $('input[name*="rptAlunoAtivExtra"]').unbind('keyup').keyup(alteraAtividadeExtra);
        $('input[name$="btnAdicionarAtiExtra"]').unbind('click').click(alteraAtividadeExtra);
    });
}

function montaTreeview() {
    //Abre os nós com coisa checada.
    $('.divTreeviewScrollCOC .alcancadoAvaliacao input[type="checkbox"]:checked').parents('li').each(function () {
        $(this).parents('ul').css('display', 'block');
        $(this).children('div.hitarea').first().removeClass('expandable-hitarea').addClass('collapsable-hitarea');
    });

    $('ul.treeview li:last-child').addClass('last');
}

function copiaValores(idFonte, idDestino) {
    document.getElementById(idDestino).value = document.getElementById(idFonte).value;
}

//abre ou fecha o nó
function toggleArvore(div) {
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

function filtrarAvaliacao() {
    var checks = $("[id$='cblQualificadorAtividade']").find("input[type='checkbox']");
    for (var i = 0; i < checks.length; i++) {
        var check = $(checks[i]);
        aplicarFiltroAvaliacao(check.parent().attr("valor"), check.is(":checked"));
    }
}

function aplicarFiltroAvaliacao(valor, checado) {
    var cells = $("input[id$='hdnQatId'][value='" + valor + "']");
    for (var i = 0; i < cells.length; i++) {
        var cell = $(cells[i]);
        if (checado) {
            // mostro a avaliacao
            cell.parents("td").css("display", "table-cell");
            cell.parents("th").css("display", "table-cell");
        }
        else {
            // escondo a avaliacao
            cell.parents("td").css("display", "none");
            cell.parents("th").css("display", "none");
        }
	}
    FixedLeftColumn.UpdateWrapper();
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsControleTurma_Listao);
arrFNCSys.push(jsControleTurma_Listao);

arrFNC.push(montaTreeview);
arrFNCSys.push(montaTreeview);
