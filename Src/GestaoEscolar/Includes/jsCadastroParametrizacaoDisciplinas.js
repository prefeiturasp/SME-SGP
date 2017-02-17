$(document).ready(
    function () {

        function analisarCheckBox(titulo) {
            var chkItemLista = $("input[id*=chkItem" + titulo + "]");
            var flag = true;
            for (i = 0; i < chkItemLista.length; i++) {
                if (!chkItemLista[i].checked) {
                    flag = false;
                    break;
                }
            }
            $('input[id*=chk' + titulo + ']').attr("checked", flag);
        }

        // Analisa a coluna 'Não lançar frequência'
        analisarCheckBox('NaoLancarFrequencia');
        // Analisa a coluna 'Não lançar nota'
        analisarCheckBox('NaoLancarNota');
        // Analisa a coluna 'Não exibir frequência'
        analisarCheckBox('NaoExibirFrequencia');
        // Analisa a coluna 'Não exibir nota'
        analisarCheckBox('NaoExibirNota');
        // Analisa a coluna 'Não exibir no boletim'
        analisarCheckBox('NaoExibirBoletim');
        // Analisa a coluna 'Não lançar planejamento'
        analisarCheckBox('NaoLancarPlanejamento');
        // Validando o CheckBox 'Permitir lançar abono de falta'
        analisarCheckBox('PermitirLancarAbonoFalta');
    });