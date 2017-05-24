$(document).ready(function () {
    //Pega todos os 'a' que tenham o simbolo '#' em seu atributo href
    $('a[href*="#area"]').click(function () {
        //Assegura que o nome do caminho da pagina corrente (location.pathname) é o mesmo do link clicado (this.pathname)
        if (location.pathname == this.pathname && location.host == this.host) {
            //Armazena o nome com o hash, 'achando' a div com esse id '#' e nome, e anima com scrollTop. O nro indica velocidade.
            var target = $(this.hash);
            target = target.size() && target || $("[name=" + this.hash.slice(1) + ']');
            $('html, body').animate({
                scrollTop: ($(target).offset().top) - 10
            }, 500);
            return false;
        };
    });

    $('#btn-top').click(function (e) {
        e.preventDefault();
        $('html, body').animate(
            { scrollTop: 0 }
        );
    });

    $('#btn-print').click(function (e) {
        e.preventDefault();
        window.print();
    });

    $('#btn-voltar').click(function (e) {
        e.preventDefault();
        window.top.location = urlRetorno;
    });    
});

function menuClick(a) {
    if (location.pathname == a.pathname && location.host == a.host) {
        //Armazena o nome com o hash, 'achando' a div com esse id '#' e nome, e anima com scrollTop. O nro indica velocidade.
        var target = $(a.hash);
        target = target.size() && target || $("[name=" + a.hash.slice(1) + ']');
        $('html, body').animate({
            scrollTop: ($(target).offset().top) - 10
        }, 500);
        return false;
    };
}

//Pega todas as seções com area e vai colocando uma classe no a(link) correspondente quando chega ao topo
$(window).scroll(function () {
    $('section[id^="area"]').each(function () {
        var scrollTop = ($(window).scrollTop()) + 200;
        var topDistance = $(this).offset().top;
        var idArea = $(this).attr('id');

        if (scrollTop > topDistance) {
            $('.nav-list a').removeClass('active');
            $('.nav-list a[href="#' + idArea + '"]').addClass('active');
        }
    });
});

$(document).on("keydown", function (e) {
    if ((e.which || e.keyCode) == 116) {
        e.preventDefault();
    }
});