//


var lib = (function () {

    var iconMap = new Array();
    iconMap['up'] = 'ui-icon-circle-arrow-n';
    iconMap['down'] = 'ui-icon-circle-arrow-s';
    iconMap['add'] = 'ui-icon-circle-arrow-s';
    iconMap['delete'] = 'ui-icon-circle-arrow-s';

    // the list managed by this library.
    var $list;
    var $addButton;

    // private functions
    function subscribe($button) {
        $button.click(onButtonClicked);
    }

    function onButtonClicked(event) {
        var $target = $(event.target);
        var buttonAction = $target.attr("data-button-action");

        if (buttonAction == 'add') {
            addRow();
        }
        else if (buttonAction == 'delete') {
            var $item = $target.parent('li');
            $item.remove();
        }
        else {
            alert("button action not found. Command = " + buttonAction);
        }
    }

    function convertToUiElement($element) {
        var $buttons = $('button[data-button=true]', $element);
        $buttons.each(function () {
            convertToButton($(this));
        });
    }

    function convertToButton($button) {
        var iconType = $button.attr('data-button-icon');
        if (!iconType) {
            $button.button();
        }
        else {
            var iconName = iconMap[iconType];
            $button.button({ icons: { primary: iconName} });
        }

        subscribe($button);
    }

    function addRow() {

        var elements = '<span class="ui-icon ui-icon-arrowthick-2-n-s"></span> \
                        <input type="text" maxlength="40" size="40" /> \
                        <button data-button="true" data-button-icon="delete" data-button-action="delete">Delete</button>';

        var $li = $(document.createElement('li'));
        $li.html(elements);
        $li.addClass('ui-state-default');
        $list.append($li);

        // convert new inner elements to jQuery UI button
        convertToUiElement($li);
    }

    return {

        init: function ($listElement, numberOfInitialRows) {

            if (!$listElement) {
                throw ('initialization failure in init(): list element is mandatory argument.');
            }

            if (!$listElement.is('ul')) {
                throw ('initialization failure in init(): list element must be of type <ul>.');
            }

            if (!$listElement || $listElement.length != 1) {
                throw ('initialization failure in init(): only one list element must be passed.');
            }

            $list = $listElement;
            $list.sortable();

            for (var i = 0; i < numberOfInitialRows; i++) {
                addRow();
            }
        },

        setAddButton: function ($button) {
            if (!$button) {
                throw ('invalid add button.');
            }

            $addButton = $button;
            convertToButton($addButton);
        }
    };
})();