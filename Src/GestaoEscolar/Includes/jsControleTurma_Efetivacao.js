function jsControleTurma_Efetivacao() {
    $(".divScrollEfetivacao").width($(".divScrollEfetivacao").parent().width());

    $(window).resize(function() {
        $(".divScrollEfetivacao").width(0).width($(".divScrollEfetivacao").parent().width());
    });

    $(".divScrollEfetivacao").scrollTo(0, $(".divScrollEfetivacao").parent().height());
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsControleTurma_Efetivacao);
arrFNCSys.push(jsControleTurma_Efetivacao);

