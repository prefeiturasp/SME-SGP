function createTabs(idTab, idSelectedTab, mostraMsgNavegacao) {
    idTab = $(idTab);
    if (idTab.length > 0) {
        if (idSelectedTab == '') {
            idTab.unbind('tabs').tabs({
                select: function (event, ui) { $(idSelectedTab).val(ui.index); },
                show: function () { scrollResponsivo() }
            });
        }
        else {
            idTab.unbind('tabs').tabs({
                selected: $(idSelectedTab).val(),
                select: function (event, ui) { $(idSelectedTab).val(ui.index); },
                show: function () { scrollResponsivo() }
            });
        }
        idTab.find(".ui-tabs-nav.hide, .ui-tabs-panel.hide").removeClass("hide");
    }
}
