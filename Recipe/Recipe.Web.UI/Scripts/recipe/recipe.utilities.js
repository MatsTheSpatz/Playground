
// Namespace 'recipe'
if (typeof recipe == 'undefined') {
    recipe = {};
}

recipe.utilities = (function () {

    var iconMap = new Array();
    iconMap['up'] = 'ui-icon-circle-arrow-n';
    iconMap['down'] = 'ui-icon-circle-arrow-s';
    iconMap['add'] = 'ui-icon-plusthick';
    iconMap['delete'] = 'ui-icon-circle-close';
    iconMap['left'] = 'ui-icon-arrowthick-1-w';
    iconMap['right'] = 'ui-icon-arrowthick-1-e';

    function onButtonClicked(event, callback, instance) {
        var $target = $(event.target);
        var buttonAction = $target.attr("data-button-action");
        callback.call(instance, $target, buttonAction);
    }

    return {
        convertToJQueryUiButton: function ($button) {
            var iconType = $button.attr('data-button-icon');
            if (!iconType) {
                $button.button();
            } else {
                var iconName = iconMap[iconType];
                if ($button.html().length > 1) {
                    $button.button({ icons: { primary: iconName} });
                } else {
                    $button.button({ icons: { primary: iconName }, text: false });
                }
            }
        },

        subscribe: function ($button, callback, instance) {
            $button.click(function (event) {
                onButtonClicked(event, callback, instance);
            });
        },

        setCookie: function (name, value, days) {
            // expires
            var expires = "";
            if (days) {
                var date = new Date();
                date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
                expires = "; expires=" + date.toGMTString();
            }

            // put everything together
            document.cookie = name + "=" + value + expires + "; path=/";
        },

        getCookie: function (name) {
            var nameEQ = name + "=";

            // document.cookie gives back all cookies for this domain and path.
            var cookies = document.cookie.split(';');
            for (var i = 0; i < cookies.length; i++) {
                var cookie = cookies[i];
                while (cookie.charAt(0) == ' ') {
                    cookie = cookie.substring(1, cookie.length);
                }
                if (cookie.indexOf(nameEQ) == 0) {
                    return cookie.substring(nameEQ.length, cookie.length);
                }
            }
            return null;
        },

        eraseCookie: function (name) {
            setCookie(name, "", -1);  // expire yesterday.
        }
    };
})();
