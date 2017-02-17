function checarTodos() {
    $("input:checkbox[name*='chkTodos']").change(function () {
        var checaTudo = $(this).attr("checked");
        // $(this).parents(".grid").find("td:first-child .checkbox input:checkbox").each(function() {
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
    $(".divSelecionaPaginas").css("display", "inline");
    $(".divSelecionaPaginas").children('[id$="hdnSelecionaGrid"]').val("false");
    $(".divSelecionaPaginas").children('[id$="lkbSelecionaGrid"]').text("Selecionar os " + $(".lblTotalRegistros").attr("valor") + " registros em todas as páginas.");
    $(".divSelecionaPaginas").children('[id$="lblSelecionaGrid"]').text("Todos os " + $(".grid td:first-child .checkbox input:checkbox").length + " registros desta página estão selecionados.");
}

function limpaTudo() {
    $(".divSelecionaPaginas").css("display", "none");
    $(".divSelecionaPaginas").children('[id$="hdnSelecionaGrid"]').val("false");
    $(".divSelecionaPaginas").children('[id$="lkbSelecionaGrid"]').text("");
    $(".divSelecionaPaginas").children('[id$="lblSelecionaGrid"]').text("");
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

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(changeCheckbox);
arrFNC.push(SelecionaPaginas);
arrFNC.push(changeCheckbox);
arrFNC.push(checarTodos);
arrFNC.push(limpaTudo);
arrFNCSys.push(changeCheckbox);
arrFNCSys.push(SelecionaPaginas);
arrFNCSys.push(changeCheckbox);
arrFNCSys.push(checarTodos);
arrFNCSys.push(limpaTudo);