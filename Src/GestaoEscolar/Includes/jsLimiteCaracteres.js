(function ($) {
    $.fn.extend({
        LimiteCaracteres: function (limit) {
            var ele = $(this);

            $(this).keypress(function () {
                trataLimiteCaracteres();
            });

            $(this).keyup(function () {
                trataLimiteCaracteres();
            });

            function trataLimiteCaracteres() {
                if ($(ele).length > 0) {
                    var val = $(ele).val();
                    //val = val.replace(/[\n\r]/g,"");
                    var length = val.length;

                    if (length > limit)
                        val = val.substring(0, limit);

                    $(ele).val(val);
                }
            }

            trataLimiteCaracteres();
        }
    });
})(jQuery);