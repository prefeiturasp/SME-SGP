// funções para mostrar/esconder mensagens nos campos informados nos parâmetros.
 function infoTextarea() {
     $(".textareaInfo + textarea").each(function() {
         verificaTextarea($(this));
     }).unbind('keyup').keyup(function(event) {
         verificaTextarea($(this));
     }).mouseleave(function (event) {
         verificaTextarea($(this));
     });

    $(".textareaInfo").disableSelection().click(function() { $(this).next("textarea").focus(); });
 }

 function verificaTextarea(txta) {
     if ($(txta).val() != "") {
         $(txta).prev(".textareaInfo").hide();
     }
     else {
         $(txta).prev(".textareaInfo").addClass('wrap400px');
         $(txta).prev(".textareaInfo").css('max-width', $(txta).width());
         $(txta).prev(".textareaInfo").fadeIn("slow");
     }
     $(txta).css('overflow', 'auto');
}

 // Insere as funções na lista de funcões - será chamado no Init.js.
 arrFNC.push(infoTextarea);
 arrFNCSys.push(infoTextarea);