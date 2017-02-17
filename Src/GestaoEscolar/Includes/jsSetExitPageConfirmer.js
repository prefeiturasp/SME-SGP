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
                        return mensagemNavegadorSair;
                    }
                }
            }
        }
    }
}