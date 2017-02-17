$(function () {

    //Adiciona a clsse redactor no textarea

    $('.redactor').redactor({
        focus: true,
        buttons: ['html', '|', 'formatting', '|', 'bold', 'italic', 'deleted', '|',
        'unorderedlist', 'orderedlist', 'outdent', 'indent', '|',
        'image', 'table', 'link', '|', '|', 'alignment', '|', 'horizontalrule'],
        linebreaks: true,
        autoresize: false,
        tabSpaces: 4,
        validateRequest: false,
        enterCallback: function (e) { }
    });
});