function jsUCCurriculo() {
    // Accordion dos eixos
    var lstHdnAberto = $("input[id$='hdnAberto']");
    for (var i = 0; i < lstHdnAberto.length; i++) {
        var hdnAberto = $(lstHdnAberto[i]);
        if (hdnAberto.val() == '1')
        {
            hdnAberto.parents(".accordion-body").addClass("accordion-opened");
        }
        else
        {
            hdnAberto.parents(".accordion-body").addClass("accordion-closed");
        }
    }
    $(".accordion-head").unbind('click').click(function () {
        var head = $(this);
        if (head.find("input[type='text']").length == 0) {
            var item = $(head.parents("tr").find('.accordion-body'));
            var hdnAberto = item.find("input[id$='hdnAberto']");
            if (item.hasClass("accordion-closed")) {
                item.removeClass("accordion-closed");
                item.addClass("accordion-opened");
                hdnAberto.val('1');
            }
            else if (item.hasClass("accordion-opened")) {
                item.removeClass("accordion-opened");
                item.addClass("accordion-closed");
                hdnAberto.val('0');
            }
        }
    });
    //
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsUCCurriculo);
arrFNCSys.push(jsUCCurriculo);