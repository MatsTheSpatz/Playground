//


var lib = (function () {

    var iconMap = new Array();
    iconMap['up'] = 'ui-icon-circle-arrow-n';
    iconMap['down'] = 'ui-icon-circle-arrow-s';

    // string constants
    var deleteButtonClassName = 'deleteButton';
    var deleteButtonClassSelector = '.' + deleteButtonClassName;

    // the table managed by this library.
    var $table;

    // private functions
    function subscribe($trElement) {
        $trElement.find(deleteButtonClassSelector).click(onDeleteClicked);
    }


    function onDeleteClicked(event) {
        var $target = $(event.target);
        var $row = $target.parent().parent();

        if (!$row || !$row.is('tr')) {
            throw ('unexpected nesting of elements. delete button is supposed to be in row.');
        }

        $row.remove();
    }

    function convertToUiElement($element) {
        var $buttons = $('button[data-button=true]', $element);
        $buttons.each(function () {
            $button = $(this);

            var iconType = $button.attr('data-button-icon');
            if (!iconType) {
                $button.button();
            }
            else {
                var iconName = iconMap[iconType];
                $button.button({ icons: { primary: iconName} });
            }
        });
    }

    return {
        b: 12,

        init: function ($tableElement, numberOfInitialRows) {

            if (!$tableElement) {
                throw ('initialization failure in init(): table element is mandatory argument.');
            }

            if (!$tableElement.is('table')) {
                throw ('initialization failure in init(): table element must be of type table.');
            }

            if (!$tableElement || $tableElement.length != 1) {
                throw ('initialization failure in init(): only one table element must be passed.');
            }

            $table = $tableElement;

            for (var i = 0; i < numberOfInitialRows; i++) {
                this.appendRow();
            }

        },

        appendRow: function () {

            var elements = ' \
                <td> \
                    <input type="text" maxlength="40" size="40" autofocus="autofocus" /> \
                </td> \
                <td> \
                    <table class="actionTable"> \
                        <tr> \
                            <td><button class="moveUp" data-button="true" data-button-icon="up">Move up</button></td> \
                            <td><button class="moveDown" data-button="true">Move up</button></td> \
                            <td><button class="delete" data-button="true">Delete</button></td> \
                            <td><button class="insert" data-button="true">Insert</button></td> \
                        </tr> \
                    </table> \
                </td> ';

            var $tr = $(document.createElement('tr'));
            $tr.html(elements);
            $table.append($tr);

            // convert to jQuery UI button
            convertToUiElement($tr);

            // hook up events.
            subscribe($tr);
        },

        d: 12
    };
})();