function checarTodos() {
    $("input:checkbox[name*='chkTodos']").change(function () {
        var checaTudo = $(this).attr("checked");
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
}

function limpaTudo() {
    $("input:checkbox[name*='chkTodos']").attr("checked", false);
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

arrFNC.push(changeCheckbox);
arrFNC.push(SelecionaPaginas);
arrFNC.push(changeCheckbox);
arrFNC.push(checarTodos);
arrFNCSys.push(changeCheckbox);
arrFNCSys.push(SelecionaPaginas);
arrFNCSys.push(changeCheckbox);
arrFNCSys.push(checarTodos);