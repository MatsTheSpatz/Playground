﻿
// Namespace 'recipe'
if (typeof recipe == 'undefined') {
    recipe = {};
}

recipe.utilities = (function () {

    var iconMap = new Array();
    iconMap['up'] = 'ui-icon-circle-arrow-n';
    iconMap['down'] = 'ui-icon-circle-arrow-s';
    iconMap['add'] = 'ui-icon-circle-arrow-s';
    iconMap['delete'] = 'ui-icon-circle-arrow-s';

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
                $button.button({ icons: { primary: iconName} });
            }
        },

        subscribe: function ($button, callback, instance) {
            $button.click(function (event) {
                onButtonClicked(event, callback, instance);
            });
        }
    };
})();
