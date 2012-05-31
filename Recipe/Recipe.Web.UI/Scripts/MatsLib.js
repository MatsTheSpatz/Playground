//

$(document).ready(function () {
    $table = $('table');
    lib.init($table, 2);
    lib.appendRow();
    alert('ok');
});

var OLDlib = (function () {

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
            var $tr = $(document.createElement('tr'));

            var td1 = '<td><input type="text" maxlength="22" size="22"/></td>';
            var td2 = '<td><textarea rows="2" cols="40" placeholder="test"></textarea></td>';
            var td3 = '<td><input class="' + deleteButtonClassName + '" type="button" value="-" /></td>';

            $tr.html(td1 + td2 + td3);
            $table.append($tr);
            subscribe($tr);
        },

        d:12
    };
})();