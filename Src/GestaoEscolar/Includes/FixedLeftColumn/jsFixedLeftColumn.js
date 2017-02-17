var FixedLeftColumn = {
    CenterHeaderVertical: false
    , CenterRowVertical: false
    , UpdateLayout: function () {
        // Atualiza a largura total da tabela inicial
        this.UpdateWidth();
        var fixedColumnLeft = 0;
        var arrayLeft = [];
        var i;

        // Atualiza a posicao do cabecalho das colunas fixas
        var fixColumnsTh = $("th.fixedLeftColumn");
        for (i = 0; i < fixColumnsTh.length; i++) {
            var fixColumnTh = $(fixColumnsTh[i]);
            fixColumnTh.css("left", fixedColumnLeft);
            arrayLeft[i] = fixedColumnLeft;
            fixedColumnLeft += fixColumnTh.innerWidth();
        }

        var rowsBody = $(".fixedLeftColumnWrapper").find("tbody").find("tr");
        for (i = 0; i < rowsBody.length; i++) {
            // Atualiza a posicao das colunas fixas
            var fixColumnsTd = $(rowsBody[i]).children("td.fixedLeftColumn");
            for (var j = 0; j < fixColumnsTd.length; j++) {
                $(fixColumnsTd[j]).css("left", arrayLeft[j]);
            }
        }
        
        var rows = $(".fixedLeftColumnWrapper").find("tr");
        for (i = 0; i < rows.length; i++) {
            var row = $(rows[i]);
            var j;

            // Encontro a altura maxima de linha
            var maxRowHeight = 0;
            var notFixColumns = row.first('not(.fixedLeftColumn)');
            if (notFixColumns.length > 0) {
                maxRowHeight = $(notFixColumns[0]).outerHeight();
            }
            var fixColumns = row.children(".fixedLeftColumn");
            for (j = 0; j < fixColumns.length; j++) {
                var fixColumnHeight = $(fixColumns[j]).outerHeight();
                if (fixColumnHeight > maxRowHeight) {
                    maxRowHeight = fixColumnHeight;
                }
            }

            // Atualiza a altura das linhas da tabela
            row.height(maxRowHeight);
            for (j = 0; j < fixColumns.length; j++) {
                $(fixColumns[j]).height(maxRowHeight);
            }
        }

        if (this.CenterHeaderVertical == true) {
            // Centraliza verticalmente o texto do cabecalho fixo
            var fixedTextsH = $("th.fixedLeftColumn").find("div:first-child");
            for (i = 0; i < fixedTextsH.length; i++) {
                var fixedTextH = $(fixedTextsH[i]);
                fixedTextH.height(fixedTextH.parent().height()).css("display", "table-cell").css("vertical-align", "middle");
            }
        }
        if (this.CenterRowVertical == true) {
            // Centraliza verticalmente o texto da linha fixa
            var fixedTextsR = $("td.fixedLeftColumn").find("span:first-child");
            for (i = 0; i < fixedTextsR.length; i++) {
                var fixedTextR = $(fixedTextsR[i]);
                fixedTextR.height(fixedTextR.parent().height()).css("display", "table-cell").css("vertical-align", "middle");
            }
        }
        this.UpdateWrapper();
    }
    , UpdateWidth: function () {
        // Atualiza a largura total da tabela inicial
        var tableWidth = 0;
        var heads = $(".fixedLeftColumnWrapper").find("th");
        for (var i = 2; i < heads.length; i++) {
            tableWidth += $(heads[i]).outerWidth();
        }
        var dif = tableWidth / ($(window).width() * 0.95);
        if (dif > 0) {
            $('.fixedLeftColumnWrapper').find("table").width(Math.ceil(tableWidth * dif));
		}
    }
    , UpdateWrapper: function () {
        $('.fixedLeftColumnWrapper').width($(".fieldset-FixedLeftColumn").width());

        //// Atualiza a largura total da tabela
        
        var heads = $(".fixedLeftColumnWrapper").find("th");
        var totalWidth = 0;
        for (var i = 2; i < heads.length; i++) {
            var head = $(heads[i]);
            if (head.css("display") != "none") {
                totalWidth += 240;
            }
        }
        $('.fixedLeftColumnWrapper').find("table").width(totalWidth);
    }
};