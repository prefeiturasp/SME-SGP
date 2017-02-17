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

        // Validando o CheckBox 'Não lançar frequência'
        clickTitulo('NaoLancarFrequencia');
        // Validando o CheckBox 'Não lancar nota'
        clickTitulo('NaoLancarNota');
        // Validando o CheckBox 'Não exibir frequência'
        clickTitulo('NaoExibirFrequencia');
        // Validando o CheckBox 'Não exibir nota'
        clickTitulo('NaoExibirNota');
        // Validando o CheckBox 'Não exibir no boletim'
        clickTitulo('NaoExibirBoletim');
        // Validando o CheckBox 'Não lançar planejamento'
        clickTitulo('NaoLancarPlanejamento');
        // Validando o CheckBox 'Permitir lançar abono de falta'
        clickTitulo('PermitirLancarAbonoFalta');

    });