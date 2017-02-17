/**
* Stylesheet toggle variation on styleswitch stylesheet switcher.
* Built on jQuery.
* Under an CC Attribution, Share Alike License.
* By Kelvin Luck ( http://www.kelvinluck.com/ )
**/

(function($) {
    // Local vars for toggle
    var availableStylesheets = [];
    var activeStylesheetIndex = 0;

    // To loop through available stylesheets
    $.stylesheetToggle = function() {
        activeStylesheetIndex++;
        activeStylesheetIndex %= availableStylesheets.length;
        $.stylesheetSwitch(availableStylesheets[activeStylesheetIndex]);
    };

    // To switch to a specific named stylesheet
    $.stylesheetSwitch = function(styleName) {
        $('link[@rel*=style][title]').each(
				function(i) {
				    this.disabled = true;
				    if (this.getAttribute('title') == styleName) {
				        this.disabled = false;
				        activeStylesheetIndex = i;
				    }
				}
			);
        createCookie('style', styleName, 365);
    };

    // To initialise the stylesheet with it's 
    $.stylesheetInit = function() {
        $('link[rel*=style][title]').each(
				function(i) {
				    availableStylesheets.push(this.getAttribute('title'));
				}
			);
        var c = readCookie('style');
        if (c) {
            if (c == "altoContraste") {
                $(".styleSwitch")
                        .removeClass("lnkAltoContraste")
                        .addClass("lnkNormal")
                        .attr("rel", "css")
                        .attr("title", "Mudar esquema de cores Normal")
                        .html("N");
            }

            $.stylesheetSwitch(c);
        }
    };
}
)(jQuery);