function jsRelatorioAcoesRealizadas() {
    createDialog('#divPeriodoDeAvaliacao', 750, 0);
    createDialog('#divGrupamento', 700, 0);
    createDialog('#divSelecionarPeriodo', 600, 0);
    createDialog('#divBuscaAluno', 755, 0);
    createDialog('#divEncaminhamentoRemanejado', 755, 0);
    createDialog('#divDeclaracaoComparecimento', 600, 0);
}

function checarTodos() {
    $("input:checkbox[name*='chkTodos']").change(function () {
        var checaTudo = $(this).attr("checked");
        //        $(this).parents(".grid").find("td:first-child .checkbox input:checkbox").each(function() {
        $(".grid td:first-child .checkbox input:checkbox").each(function () {
            $(this).attr("checked", checaTudo);
            if ($(this).attr("checked")) {
                $(this).parents("tr").addClass("gridSelectedRow");
                $(".divSeleciona").fadeIn();
            } else {
                $(this).parents("tr").removeClass("gridSelectedRow");
                $(".divSeleciona").fadeOut();
            }
        });
        if (checaTudo == true) {
            selecionaTudo();
        } else {
            limpaTudo();
        }
    });
}
function changeCheckbox() {
    $(".grid td:first-child .checkbox input:checkbox").change(function () {
        if ($(this).attr("checked")) {
            $(this).parents("tr").addClass("gridSelectedRow");
            $(".divSeleciona").fadeIn();
        } else {
            $(this).parents("tr").removeClass("gridSelectedRow");
            $(".divSeleciona").fadeOut();
        }

        if ($(".grid td:first-child .checkbox input:checkbox:checked").length == $(".grid td:first-child .checkbox input:checkbox").length) {
            selecionaTudo();
        } else {
            $("input:checkbox[name*='chkTodos']").attr("checked", false);
            limpaTudo();
        }
    });
}
function selecionaTudo() {
    $("input:checkbox[name*='chkTodos']").attr("checked", true);
    //    $(".divSelecionaPaginas").css("display", "inline");
    //    $(".divSelecionaPaginas").children('[id$="hdnSelecionaGrid"]').val("false");
    //    $(".divSelecionaPaginas").children('[id$="lkbSelecionaGrid"]').text("Selecionar os " + $(".lblTotalRegistros").attr("valor") + " registros em todas as páginas.");
    //    $(".divSelecionaPaginas").children('[id$="lblSelecionaGrid"]').text("Todos os " + $(".grid td:first-child .checkbox input:checkbox").length + " registros desta página estão selecionados.");
}
function SelecionaPaginas() {
    $(".divSelecionaPaginas").children('[id$="lkbSelecionaGrid"]').click(function () {
        var texto = $(".divSelecionaPaginas").children('[id$="hdnSelecionaGrid"]').val();

        if (texto == "false") {
            $(".divSelecionaPaginas").children('[id$="hdnSelecionaGrid"]').val("true");
            $(this).text("Limpar seleção");
            $(".divSelecionaPaginas").children('[id$="lblSelecionaGrid"]').text("Os " + $(".lblTotalRegistros").attr("valor") + " registros, em todas as páginas, estão selecionados.");
        }
        else {
            $("input:checkbox[name*='chkTodos']").attr("checked", false).trigger("change");
        }
    });
}

arrFNC.push(jsRelatorioAcoesRealizadas);
arrFNC.push(changeCheckbox);
arrFNC.push(SelecionaPaginas);
//arrFNC.push(jsDivpesquisa);
arrFNC.push(changeCheckbox);
arrFNC.push(checarTodos);
arrFNCSys.push(jsRelatorioAcoesRealizadas);
arrFNCSys.push(changeCheckbox);
arrFNCSys.push(SelecionaPaginas);
//arrFNCSys.push(jsDivpesquisa);
arrFNCSys.push(changeCheckbox);
arrFNCSys.push(checarTodos);