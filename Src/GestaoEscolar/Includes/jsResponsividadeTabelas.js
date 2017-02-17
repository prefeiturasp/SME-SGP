function jsResponsividadeTabelas() {
    $('table.grid-responsive-list').each(function () {
        var trHeader;
        var tabela;
        var inicio = 1;

        if ($(this).children('thead').size() > 0) {
            tabela = $(this).children('thead').first();
            trHeader = tabela.children('tr').eq(0);

            if (trHeader.size() > 0) {
                TrataTabela(inicio, tabela, trHeader);
            }
        }
        else {
            tabela = $(this);
            trHeader = tabela.children('tr').eq(0);
        }

        if ($(this).children('tbody').size() > 0) {
            tabela = $(this).children('tbody').first();

            if (trHeader.size() == 0) {
                // Se não encontrou a primeira tr na <table> nem no <thead>, buscar no <tbody>.
                trHeader = tabela.children('tr').eq(0);
                inicio = 1;
            }
            else {
                inicio = 0;
            }
        }
        else {
            tabela = $(this);
            inicio = 1;
        }

        TrataTabela(inicio, tabela, trHeader);
    });
}

function TrataTabela(inicio, tabela, trHeader) {
    if (trHeader.size() > 0) {
        for (var i = inicio; i < tabela.children('tr').size() ; i++) {
            var tr = tabela.children('tr').eq(i);

            for (var x = 0; x < tr.children('td').size() ; x++) {

                var td = tr.children('td').eq(x);
                var tdheader = trHeader.children('td,th').eq(x);

                if (tdheader != null && td != null
                    && tdheader.text() != null
                    && tdheader.text().trim() != "") {
                    td.attr('data-header', tdheader.text() + ": ");
                }
            }
        }
    }
}

// Insere as funções na lista de funcões - será chamado no Init.js.
arrFNC.push(jsResponsividadeTabelas);
arrFNCSys.push(jsResponsividadeTabelas);