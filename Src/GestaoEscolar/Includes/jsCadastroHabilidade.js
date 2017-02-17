var fecharJanelaRelacionarHabilidades = true;
var validarSelectMatriz = true;

function jsCadastroHabilidade() {
    createDialog('#divOrientacoes', 600, 0);
    createDialog('#divReplicar', 600, 0);
    createDialog('#divRelacionarHabilidades', 600, 0);
    $("#divConfirmRelacionamento").dialog(
        {
            autoOpen: false
            , resizable: false
            , modal: true
            , buttons: {
                "Sim": function () { $(this).dialog("close"); if (fecharJanelaRelacionarHabilidades) { $('#divRelacionarHabilidades').unbind("dialogbeforeclose"); $("#divRelacionarHabilidades").dialog("close"); } else { validarSelectMatriz = false; } },
                "Não": function () { $(this).dialog("close"); }
            }
        }
    );

    $('div.hitarea').unbind('click').click(function () {
        toggleArvore(this);
    });

    $('#divRelacionarHabilidades').unbind("dialogbeforeclose").bind("dialogbeforeclose", function() { return validaAlteracaoRelacionamento(true); });
    $("[id$='ddlMatrizCurricular']").unbind("mousedown").bind("mousedown", function() { if (validarSelectMatriz) { return validaAlteracaoRelacionamento(false); } });
    $("[id$='ddlMatrizCurricular']").unbind("change").bind("change", function () { validarSelectMatriz = true; });

    $(document).ready(function () {
        $("input[id$='chkRelacionada']:checked").parents('ul').show();
    });
}

//abre ou fecha o nó
function toggleArvore(div) {
    var node = $(div).parents('li').find('ul').first();
    var display = node.css('display');
    if (display == 'none') {
        node.css('display', 'block');
        $(div).removeClass('expandable-hitarea').addClass('collapsable-hitarea');
    } else if (display == 'block') {
        node.css('display', 'none');
        $(div).addClass('expandable-hitarea').removeClass('collapsable-hitarea');
        node.find('ul').css('display', 'none');
        node.find('div.hitarea').addClass('expandable-hitarea').removeClass('collapsable-hitarea');
    }
}

function validaAlteracaoRelacionamento(fechar)
{
    fecharJanelaRelacionarHabilidades = fechar;
    if (($("input[id$='chkRelacionada']:checked").parents("div[id$='divHabilidade']").find("input[id$='hdnRelacionada'][value='0']")).length > 0
        || ($("input[id$='chkRelacionada']:not(:checked)").parents("div[id$='divHabilidade']").find("input[id$='hdnRelacionada'][value='1']")).length > 0) {
        $("#divConfirmRelacionamento").dialog("open");
        return false;
    }
    return true;
}

arrFNC.push(jsCadastroHabilidade);
arrFNCSys.push(jsCadastroHabilidade);