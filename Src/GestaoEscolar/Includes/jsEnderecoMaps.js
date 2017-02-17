var map;
var latitude;
var longitude;
var marker;
var idTxtLatitude;
var idTxtLongitude;
//var idLkbExpandir;
var idMap_canvas

function jsPopUp() {
    createDialog('#divLocalizacaoGeografica', 620, 0);
}
// Carrega o script necessário para a geração dos mapas.
function Inicializar(idMap) {
    idMap_canvas = idMap;
    //idLkbExpandir = idExpandir;
    window.onload = CarregarScript;

    $(idMap).hide();
    //$(idExpandir).hide();
}

// Carrega o Google Maps JavaScript API.
function CarregarScript() {
    var script = document.createElement("script");
    script.type = "text/javascript";
    script.src = "http://maps.googleapis.com/maps/api/js?sensor=false&callback=CarregarMapaInicial";
    document.body.appendChild(script);
}

// Carrega uma única vez o mapa inicial.
function CarregarMapaInicial() {
    geocoder = new google.maps.Geocoder();

    //latitude = $(idTxtLatitude).val();
    //longitude = $(idTxtLongitude).val();

    if (idMap_canvas != null) {
        var myOptions = {
            zoom: 16,
            center: new google.maps.LatLng(latitude, longitude),
            mapTypeId: google.maps.MapTypeId.ROADMAP
        }
        map = new google.maps.Map(idMap_canvas, myOptions);
    }

    MarcaPonto(new google.maps.LatLng(latitude, longitude));
}

// Carrega as configurações necessárias para a geração dos mapas.
function CarregarConfiguracao() {
    ConfigurarMapaTelaCheia();
}

// Gera o mapa de acordo com as coordenadas.
function GerarMapaPorCoordenadas(idLatitude, idLongitude, idMap) {
    idTxtLatitude = idLatitude;
    idTxtLongitude = idLongitude;
    //idLkbExpandir = idExpandir;
    idMap_canvas = idMap;

    latitude = 0;
    longitude = 0;

    if (($(idLatitude).val() != '') && ($(idLongitude).val() != '')) {
        $(document).ready(function () { $('#divLocalizacaoGeografica').dialog('open'); });
        Inicializar(idMap);//, idExpandir);

        $(idMap).show();
        //$(idExpandir).show();

        if (map == null) {
            CarregarMapaInicial();
        }

        EnquadraMapa();

        var coordenadas = $(idLatitude).val() + ", " + $(idLongitude).val();
        geocoder.geocode({ 'address': coordenadas }, Resultados);

        $(idMap).focus()
    }
}

// Gera o mapa de acordo com o endereço.
function GerarMapaPorEndereco(idLatitude, idLongitude, idCep, idLogradouro, idNumero, idCidade, idMap) {
    idTxtLatitude = idLatitude;
    idTxtLongitude = idLongitude;
    var cep = $(idCep).val();
    var logradouro = $(idLogradouro).val();
    var numero = $(idNumero).val();
    var cidade = $(idCidade).val();
    //idLkbExpandir = idExpandir;
    idMap_canvas = idMap;

    latitude = 0;
    longitude = 0;

    if ((cep != '') && (logradouro != '') && (numero != '') && (cidade != '')) {
        $(document).ready(function () { $('#divLocalizacaoGeografica').dialog('open'); });
        Inicializar(idMap);//, idExpandir);

        $(idMap).show();
        //$(idExpandir).show();

        if (map == null) {
            CarregarMapaInicial();
        }

        EnquadraMapa();

        var endereco = cep + " " + logradouro + " " + numero + " " + cidade;
        geocoder.geocode({ 'address': endereco, 'partialmatch': true }, Resultados);
    }

    $(idMap).focus()
}

// Exibe o mapa de acordo com o resultado.
function Resultados(results, status) {
    if (status == 'OK' && results.length > 0) {
        map.fitBounds(results[0].geometry.viewport);
        map.setZoom(16);

        MarcaPonto(results[0].geometry.location);
        AtualizaCoordenadas(results[0].geometry.location);
    } else {
        alert("Endereço não encontrado.");
    }
}

// Marca um ponto no mapa
function MarcaPonto(location) {
    if (map != null) {
        map.setCenter(location);
        if (marker == null) {
            marker = new google.maps.Marker({
                map: map,
                position: location,
                draggable: true
            });
        }
        else {
            marker.setPosition(location);
        }

        google.maps.event.addDomListener(marker, 'dragend', function () {
            AtualizaCoordenadas(marker.getPosition());
        });
    }
}

// Atualiza os campos das coordenadas.
function AtualizaCoordenadas(mapa) {
    $(idTxtLatitude).val(mapa.lat().toString());
    $(idTxtLongitude).val(mapa.lng().toString());
}

// Carrega as configurações necessárias para a opção mapa em tela cheia.
function ConfigurarMapaTelaCheia() {
    // Cria a div que servirá de fundo para a janela do mapa.
    elemento = document.createElement('div');
    elemento.setAttribute('id', "fundo");
    elemento.style.position = 'absolute';
    elemento.style.left = '0';
    elemento.style.top = '0';
    elemento.style.zIndex = '9000';
    elemento.style.backgroundColor = '#000';
    elemento.style.display = 'none';
    document.forms[0].appendChild(elemento);

    // Adiciona o evento de esconder mapa caso o fundo seja clicado.
    $('#fundo').unbind("click").click(function (e) {
        e.preventDefault();
        EsconderMapaTelaCheia();
    });

    // Adiciona o evento de esconder mapa caso a tecla esc seja pressionada.
    $(document).keyup(function (e) {
        if (e.keyCode == 27) {
            e.preventDefault();
            EsconderMapaTelaCheia();
        }
    });
}

// Mostra o mapa em formato tela cheia.
function MostrarMapaTelaCheia(idMap) {
    idMap_canvas = idMap;
    $('#fundo').height($(document).height()).width('100%').fadeTo("slow", 0.8);
    $(idMap).height($(window).height() - 100).width($(window).width() - 100).css({ "position": "fixed", "top": "50px", "left": "50px", "z-index": "9999" });

    EnquadraMapa();
}

// Esconde o mapa em formato tela cheia.
function EsconderMapaTelaCheia() {
    $('#fundo').hide();
    $(idMap_canvas).width(340).height(150).css({ "position": "relative", "float": "left", "top": "0", "left": "0" });

    EnquadraMapa();
}

// Centraliza e enquadra o mapa de acordo com o tamanho do container.
function EnquadraMapa() {
    google.maps.event.trigger(map, 'resize');
    map.setCenter(new google.maps.LatLng(latitude, longitude));
    map.setZoom(16);
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsPopUp);
arrFNC.push(CarregarConfiguracao);
arrFNCSys.push(jsPopUp);
arrFNCSys.push(CarregarConfiguracao);