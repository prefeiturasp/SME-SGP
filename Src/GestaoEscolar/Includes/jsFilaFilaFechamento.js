$(document).ready(
    function () {

        function clickTitulo(titulo) {

            $('input[id*=chk' + titulo + ']').click(
                function () {
                    $('input[id*=chkItem' + titulo + ']').attr("checked", this.checked);
                }
            );

            var chkItemLista = $('input[id*=chkItem' + titulo + ']');
            chkItemLista.click(
                function () {
                    var flag = true;
                    for (i = 0; i < chkItemLista.length; i++) {
                        if (!chkItemLista[i].checked) {
                            flag = false;
                            break;
                        }
                    }
                    $('input[id*=chk' + titulo + ']').attr("checked", flag);
                });
        }

        // Validando o CheckBox 'Gerar fila nota'
        clickTitulo('GerarFilaNota');
        // Validando o CheckBox 'Gerar fila frequência'
        clickTitulo('GerarFilaFrequencia');

    });