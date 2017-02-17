var exibirMensagemConfirmacao;

// Função de correção de bug do IE 10, que existe no .Net 3.5 ou inferior.
// http://stackoverflow.com/questions/13299685/ie10-sending-image-button-click-coordinates-with-decimals-floating-point-values/15129393#15129393
$(function () {
    // Patch fractional .x, .y form parameters for IE10.
    if (typeof (Sys) !== 'undefined' && Sys.Browser.agent === Sys.Browser.InternetExplorer && (Sys.Browser.version === 10 || !!navigator.userAgent.match(/Trident.*rv[ :]*11\./))) {
        Sys.WebForms.PageRequestManager.getInstance()._onFormElementActive = function Sys$WebForms$PageRequestManager$_onFormElementActive(element, offsetX, offsetY) {
            if (element.disabled) {
                return;
            }
            this._activeElement = element;
            this._postBackSettings = this._getPostBackSettings(element, element.name);
            if (element.name) {
                var tagName = element.tagName.toUpperCase();
                if (tagName === 'INPUT') {
                    var type = element.type;
                    if (type === 'submit') {
                        this._additionalInput = encodeURIComponent(element.name) + '=' + encodeURIComponent(element.value);
                    }
                    else if (type === 'image') {
                        this._additionalInput = encodeURIComponent(element.name) + '.x=' + Math.floor(offsetX) + '&' + encodeURIComponent(element.name) + '.y=' + Math.floor(offsetY);
                    }
                }
                else if ((tagName === 'BUTTON') && (element.name.length !== 0) && (element.type === 'submit')) {
                    this._additionalInput = encodeURIComponent(element.name) + '=' + encodeURIComponent(element.value);
                }
            }
        };
    }

    // Adicionado tratamento customizado para o ValidationSummary, porque, em alguns temas,
    // a tela nao estava movendo para o topo e o usuario nao conseguia visualizar a mensagem da validacao.
    if (typeof (Page_ClientValidate) != "undefined") {
        ValidationSummaryOnSubmit = CustomValidationSummaryOnSubmit;
    }
});

// Cria um JQuey Dialog na div passada pelo id.
function createDialogCloseWithConfirmation(id, width, heigth) {

    try {
        // Primeiro dá UnBind, se estiver instanciado anteriormente, limpa memória.
        $(id).unbind('dialog');
    }
    catch (e) {
    }

    $(id).dialog({
        bgiframe: true,
        autoOpen: false,
        resizable: false,
        modal: true,
        closeOnEscape: true,
        open: function (type, data) {
            exibirMensagemConfirmacao = true;

            $(id).parent().prependTo($("#aspnetForm"));

            if (!(($.browser.msie) && ($.browser.version <= 7))) {
                $(id).dialog("option", "position", "center");
                $(window).scroll(function () {
                    $(id).dialog("option", "position", "center");
                });
            }
            if (($.browser.msie) && ($.browser.version == 9)) {
                if (controlDialog == '') {
                    controlDialog = id;
                    $('body').css('overflow', 'hidden');
                }
            }
        },
        beforeClose: function (type, data) {
            if (exibirMensagemConfirmacao == undefined) {
                exibirMensagemConfirmacao = false;
            }
            if (exibeMensagemSair && exibeMensagemSairParametro && (!exibeMensagemPadraoNavegadorSair) && exibirMensagemConfirmacao) {
                //recebe do clientID do botão
                btnAction = "#" + this.id;
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
                            $(btnAction).dialog("close");
                        }
                    },
                    {
                        text: msgSairTelaBotaoNao,
                        click: function () {
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
                return result;
            }
            else {
                return true;
            }
        },
        close: function (type, data) {
            $(window).unbind("scroll");

            if (($.browser.msie) && ($.browser.version == 9)) {
                if (controlDialog == id) {
                    controlDialog = '';
                    $('body').css('overflow', 'visible');
                }
            }
        },
        buttons: {}
    });


    if (width != null) {
        $(id).dialog('option', 'width', width);
    }

    // Caso a resolução seja 800x600 diminui o valor da altura máxima
    if ($(window).height() < 500) {
        $('.ui-dialog').css("max-height", '380px');
        $('.ui-dialog-content').css("max-height", '340px');
    }

    // Seta heigth - não obrigatório.
    if (heigth > 0)
        $(id).dialog("option", "height", heigth);

    $(id).find('.subir').unbind('click.Subir').bind('click.Subir', function () {
        $(id).scrollTop(0);
    });

    // Workaround bug #6644: Select in Dialog causes slowness on IE8
    //(http://bugs.jqueryui.com/ticket/6644)
    if (($.browser.msie) && ($.browser.version < 9))
        $(id + ' select').unbind('mousedown.dialogSelect').bind('mousedown.dialogSelect', function (e) { e.stopPropagation(); return false; });
}

//Função para mostrar um loading ao executar o IFrame do site do Relatório do Gestão.
function loadingProgressIframe() {
    var iframe = document.getElementById("ifUrlReport");
    var progress = $get("ctl00_Loader_upgProgress");
    if (iframe != null) {
        progress.style.display = "block";
        if (navigator.userAgent.indexOf("MSIE") > -1 && !window.opera) {
            iframe.onreadystatechange = function () {
                if (iframe.readyState == "complete") {
                    progress.style.display = "none";
                }
            };
        }
        else {
            iframe.onload = function () {
                progress.style.display = "none";
            };
        }
    }
}

function scrollToTop() {
    scrollToTopWindow();
}

function scrollToTopWindow(element) {
    if (element != undefined && $(element).length > 0)
    {
        // se o elemento estiver dentro de um dialog,
        // move para o topo do dialog.
        var dialogElement = $(element).parents('.ui-dialog-content');
        if ($(dialogElement).length > 0) {
            $(dialogElement).animate({ scrollTop: 0 }, 'fast');
            return;
        }
    }
    // move para o topo da tela.
    $('html, body, .off-canvas-wrap').animate({ scrollTop: 0 }, 'fast');
}

function CustomValidationSummaryOnSubmit(validationGroup) {
    if (typeof (Page_ValidationSummaries) == "undefined")
        return;
    var summary, sums, s;
    var headerSep, first, pre, post, end;
    for (sums = 0; sums < Page_ValidationSummaries.length; sums++) {
        summary = Page_ValidationSummaries[sums];
        if (!summary) continue;
        summary.style.display = "none";
        if (!Page_IsValid && IsValidationGroupMatch(summary, validationGroup)) {
            var i;
            if (summary.showsummary != "False") {
                summary.style.display = "";
                if (typeof (summary.displaymode) != "string") {
                    summary.displaymode = "BulletList";
                }
                switch (summary.displaymode) {
                    case "List":
                        headerSep = "<br>";
                        first = "";
                        pre = "";
                        post = "<br>";
                        end = "";
                        break;
                    case "BulletList":
                    default:
                        headerSep = "";
                        first = "<ul>";
                        pre = "<li>";
                        post = "</li>";
                        end = "</ul>";
                        break;
                    case "SingleParagraph":
                        headerSep = " ";
                        first = "";
                        pre = "";
                        post = " ";
                        end = "<br>";
                        break;
                }
                s = "";
                if (typeof (summary.headertext) == "string") {
                    s += summary.headertext + headerSep;
                }
                s += first;
                for (i = 0; i < Page_Validators.length; i++) {
                    if (!Page_Validators[i].isvalid && typeof (Page_Validators[i].errormessage) == "string") {
                        s += pre + Page_Validators[i].errormessage + post;
                    }
                }
                s += end;
                summary.innerHTML = s;

                // Alterada essa parte do codigo padrao, porque, em alguns temas,
                // a tela nao estava movendo para o topo e o usuario nao conseguia visualizar a mensagem da validacao. 
                //window.scrollTo(0, 0);
                scrollToTopWindow(summary);
                //
            }
            if (summary.showmessagebox == "True") {
                s = "";
                if (typeof (summary.headertext) == "string") {
                    s += summary.headertext + "\r\n";
                }
                var lastValIndex = Page_Validators.length - 1;
                for (i = 0; i <= lastValIndex; i++) {
                    if (!Page_Validators[i].isvalid && typeof (Page_Validators[i].errormessage) == "string") {
                        switch (summary.displaymode) {
                            case "List":
                                s += Page_Validators[i].errormessage;
                                if (i < lastValIndex) {
                                    s += "\r\n";
                                }
                                break;
                            case "BulletList":
                            default:
                                s += "- " + Page_Validators[i].errormessage;
                                if (i < lastValIndex) {
                                    s += "\r\n";
                                }
                                break;
                            case "SingleParagraph":
                                s += Page_Validators[i].errormessage + " ";
                                break;
                        }
                    }
                }
                alert(s);
            }
        }
    }
}

function scrollResponsivo() {

    $(".divScrollResponsivo").each(function () {
        var tt = $(this).find("table:first")
        if (tt.length > 0) {
            tt[0].style.display = "none";
            tt[0].offsetWidth;

            $(this).width(0).width($(this).parent().width());

            tt[0].style.display = "table";
        }
    });
}

function contrastColor(hexcolor) {
    hexcolor = hexcolor.replace("#", "");
    var r = parseInt(hexcolor.substr(0, 2), 16);
    var g = parseInt(hexcolor.substr(2, 2), 16);
    var b = parseInt(hexcolor.substr(4, 2), 16);
    var yiq = ((r * 299) + (g * 587) + (b * 114)) / 1000;
    return (yiq >= 128) ? '#000' : '#FFF';
}

//Rola a tela pra cima quando abrir a dialog
(function ($, prototype) {
    var open = prototype.open;
    prototype.open = function () {
        open.call(this);
        if (window.matchMedia("only screen and (max-width: 64em)").matches) {
            scrollToTop();
        }
    };
}(jQuery, jQuery.ui.dialog.prototype));

function UtilGestao() {

    setTimeout(function () { scrollResponsivo(); }, 100);

    $(window).resize(function () {
        scrollResponsivo();
    });

    //JS Menu responsivo
    $(".i a").unbind("focus focusout");

    if (!$(".m > .i:first").hasClass("menu-icon")) {
        $(".m").prepend("<li class='i menu-icon' style='display:none;'><a href='#' title=\"Menu\"><i></i></a></li>");
    }
    $(".menu-icon a").unbind("click").click(function (e) {
        e.preventDefault();
        $('body').scrollTop(0);
        $(this).parents(".m").toggleClass("active");
        $(this).parents("body").toggleClass("responsive-menu-active");
        $(".m .active").removeClass("active");
    });
    $(".m .i a").not(".m .i.menu-icon a").unbind("click").click(function (e) {
        var link = $(this);
        if ((window.matchMedia("only screen and (max-width: 1024px)").matches) && (link.next().hasClass("s"))) {
            e.preventDefault();
            link.parent().toggleClass("active");
        }
    })
}
arrFNC.push(UtilGestao); arrFNCSys.push(UtilGestao)