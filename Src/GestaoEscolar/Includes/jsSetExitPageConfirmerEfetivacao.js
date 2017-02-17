function SetExitPageConfirmer() {
    if (exibeMensagemSair && exibeMensagemSairParametro) {
        if (exibeMensagemPadraoNavegadorSair) {
            ExitPageConfirmer();
        }
        else {
            var needToConfirmExit = true;
            //Cria janela de confirmação.
            $('#hd a, #hd input[type="submit"], .m a, .m input[type="submit"], .breadCrumb a, .breadCrumb input[type="submit"], #bd .btnMensagemUnload, .btnMensagemUnload')
                .die('click')
                .live('click', function () {
                    if (!($(this).is('select'))) {
                        //recebe do clientID do
                        btnAction = "#" + this.id;
                        caminho = this.href;
                        //Cria alerta de confirmação
                        if ($("#divConfirm").length > 0)
                            $("#divConfirm").remove();

                        $("<div id=\"divConfirm\" title=\"Confirmação\">" + mensagemNavegadorSair + "</div>").dialog({
                            bgiframe: true,
                            autoOpen: false,
                            resizable: false,
                            modal: true,
                            closeOnEscape: false,
                            open: function (event, ui) { $("#divConfirm").parent().find(".ui-dialog-titlebar-close").hide(); },
                            buttons: [{
                                text: msgSairTelaBotaoSim,
                                click: function () {
                                    $(this).dialog("close");
                                    execute = true;
                                    needToConfirmExit = false;

                                    if (caminho) {
                                        window.location.href = caminho;
                                    }
                                    else {
                                        $(btnAction).click();
                                    }

                                    if (opcao == 4) {
                                        PreCarregarCacheFechamento();
                                    }
                                }
                            },
                            {
                                text: msgSairTelaBotaoNao,
                                click: function () {
                                    needToConfirmExit = true;
                                    $(this).dialog("close");
                                }
                            }]
                        });
                        //Abre a mensagem de confirmação
                        if (!execute) {
                            $("#divConfirm").dialog("open");
                        }
                        //If execute is true, it means that it was set by the yes callback
                        //and so we should return true in order to not interfer with the form submission
                        var result = execute;
                        execute = false;
                        needToConfirmExit = false;
                        return result;
                    }
                    else {
                        needToConfirmExit = true;
                    }
                });

            if (exibeMensagemNavegadorComJQuery) {
                $('#bd a').unbind('click.confirmExit').bind('click.confirmExit', function () {
                    needToConfirmExit = false;
                });
                $('.btn').unbind('click.confirmExit').bind('click.confirmExit', function () {
                    needToConfirmExit = false;
                });

                window.onbeforeunload = function () {
                    if (needToConfirmExit) {
                        var divLoading = $("div[id$='divLoading']");
                        if (divLoading.length == 0 || divLoading.css("display") == "none") {
                            return mensagemNavegadorSair;
                        }
                    }
                }
            }
        }
    } else {
        $('#bd .btnMensagemUnload, .btnMensagemUnload')
                .die('click')
                .live('click', function () {
                    if (opcao == 4 && !($(this).is('select'))) {
                        $.ajax({
                            type: "GET",
                            url: "CacheEfetivacaoHandler.ashx",
                            data: function () {
                                var item = $("#divDadosFechamento");
                                return RetornaDados(item);
                            }(),
                            cache: false,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json"/*,
                            beforeSend: function () {
                                $(".loader").parent().show();
                            },
                            complete: function () {
                                setTimeout(function () {
                                    $(".loader").parent().hide();
                                }, 1000);
                            },
                            error: function () {
                                $(".loader").parent().hide();
                            }*/
                        });
                    }
                });
    }
}

function RetornaDados(item) {
    return {
        "tud_id": item.find('input[name$="hdnTudId"]').val(),
        "tur_id": item.find('input[name$="hdnTurId"]').val(),
        "tpc_id": item.find('input[name$="hdnTpcId"]').val(),
        "ava_id": item.find('input[name$="hdnAvaId"]').val(),
        "fav_id": item.find('input[name$="hdnFavId"]').val(),
        "tipoAvaliacao": item.find('input[name$="hdnTipoAvaliacao"]').val(),
        "esa_id": item.find('input[name$="hdnEsaId"]').val(),
        "tipoEscalaDisciplina": item.find('input[name$="hdnTipoEscala"]').val(),
        "tipoEscalaDocente": item.find('input[name$="hdnTipoEscalaDocente"]').val(),
        "notaMinimaAprovacao": item.find('input[name$="hdnNotaMinima"]').val(),
        "ordemParecerMinimo": item.find('input[name$="hdnParecerMinimo"]').val(),
        "tipoLancamento": item.find('input[name$="hdnTipoLancamento"]').val(),
        "fav_calculoQtdeAulasDadas": item.find('input[name$="hdnCalculoQtAulasDadas"]').val(),
        "tur_tipo": item.find('input[name$="hdnTurTipo"]').val(),
        "cal_id": item.find('input[name$="hdnCalId"]').val(),
        "tud_tipo": item.find('input[name$="hdnTudTipo"]').val(),
        "tpc_ordem": item.find('input[name$="hdnTpcOrdem"]').val(),
        "fav_variacao": item.find('input[name$="hdnVariacao"]').val(),
        "tipoDocente": item.find('input[name$="hdnTipoDocente"]').val(),
        "disciplinaEspecial": item.find('input[name$="hdnDisciplinaEspecial"]').val(),
        "permiteAlterarResultado": permiteAlterarResultado,
        "exibirNotaFinal": exibirNotaFinal,
        "ExibeCompensacao": ExibeCompensacao,
        "MinutosCacheFechamento": MinutosCacheFechamento,
        "fechamentoAutomatico": item.find('input[name$="hdnFechamentoAutomatico"]').val(),
        "processarFilaFechamentoTela": item.find('input[name$="hdnProcessarFilaFechamentoTela"]').val()
    }
}

function PreCarregarCacheFechamento() {
    $.ajax({
        type: "GET",
        url: "CacheEfetivacaoHandler.ashx",
        data: function () {
            var item = $("#divDadosFechamento");
            return RetornaDados(item);
        }(),
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json"/*,
        beforeSend: function () {
            $(".loader").parent().show();
        },
        complete: function () {
            setTimeout(function () {
                $(".loader").parent().hide();
            }, 1000);
        },
        error: function () {
            $(".loader").parent().hide();
        }*/
    });
}