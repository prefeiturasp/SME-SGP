function jsBuscaDocumentosAluno() {
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

function limpaTudo() {
    //    $(".divSelecionaPaginas").css("display", "none");
    //    $(".divSelecionaPaginas").children('[id$="hdnSelecionaGrid"]').val("false");
    //    $(".divSelecionaPaginas").children('[id$="lkbSelecionaGrid"]').text("");
    //    $(".divSelecionaPaginas").children('[id$="lblSelecionaGrid"]').text("");
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

//function jsDivpesquisa() {
//    var divPesquisa, divRelatorio;

//    divPesquisa = $(".divPesquisa");
//    divRelatorio = $(".divRelatorio");
//    if (divPesquisa.height() <= divRelatorio.height()) {
//        divPesquisa.css('height', divRelatorio.height());
//    } else {
//        divRelatorio.css('height', divPesquisa.height());
//    }
//}

function ExtensaoDosFiltros(idEntidade) {
    if (!Page_ClientValidate(""))
        return false;
    var retorno;
    var opcao = $('input[id*="_rdbRelatorios"]:radio:checked').val();
    if (opcao == "237") {
        var data = { ent_id: idEntidade, rlt_id: opcao };
        $.ajax({
            async: false,
            type: "POST",
            url: window.location + "/ExtensaoDosFiltros",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result, status) {
                if (result.d == true) {
                    $("#divSelecionarPeriodo").dialog("open");
                    retorno = false;
                }
                else {
                    retorno = true;
                }
            }
        });
    }
    else if (opcao == "55") {
        if (TodosOsCursosSelecionadosSaoPeja())
            $("#divGrupamentoFrequencia").hide();
        else
            $("#divGrupamentoFrequencia").show();

        $("#divGrupamento").dialog("open");
        retorno = false;
    }
    else if (opcao == 200) {
        $("#divEncaminhamentoRemanejado").dialog("open");
        retorno = true;
    }
    else
        retorno = true;
    return retorno;
}

function TodosOsCursosSelecionadosSaoPeja() {
    if ($("input:checkbox[id$=chkTodos]:checked").parent().attr("todososcursospeja") == "1") {
        return true;
    } else {
        var numCurSelPeja = 0, numCur = 0;
        $("input:checkbox[id$=chkSelecionar]:checked").parent().each(function () {
            numCurSelPeja += parseInt($(this).attr('cursopeja'));
            numCur++;
        });

        if (numCurSelPeja == numCur)
            return true;
    }
    return false;
}


arrFNC.push(jsBuscaDocumentosAluno);
arrFNC.push(changeCheckbox);
arrFNC.push(SelecionaPaginas);
//arrFNC.push(jsDivpesquisa);
arrFNC.push(changeCheckbox);
arrFNC.push(checarTodos);
arrFNCSys.push(jsBuscaDocumentosAluno);
arrFNCSys.push(changeCheckbox);
arrFNCSys.push(SelecionaPaginas);
//arrFNCSys.push(jsDivpesquisa);
arrFNCSys.push(changeCheckbox);
arrFNCSys.push(checarTodos);
