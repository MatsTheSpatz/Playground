﻿//

$(document).ready(function () {
    var $list = $('ul').first();
    lib.init($list, 5);

    var $addButton = $('button[data-button-action="add"]');
    lib.setAddButton($addButton);
    //    lib.appendRow();

    //    $('select').each(function () {
    //        var $ul = $(document.createElement('ul'));

    //        $('option', $select).each(function () {
    //            var $li = $(document.createElement('li'));
    //            $li.text($(this).text());
    //            $ul.append($li);
    //        });

    //        $(this).replaceWith($ul);
    //    });

//	$( "select" ).combobox();



//    $('select').replaceWith(function () {
//        var $ul = $(document.createElement('ul'));
//        $ul.addClass('selectReplacement');

//        var i = 0;
//        $('option', $(this)).each(function () {
//            var $li = $(document.createElement('li'));
//            $li.text($(this).text());

//            if (i == 0) {
//                i = 1;
//                $li.addClass('selected');
//            }
//            $ul.append($li);
//        });

//        return $ul;
//    });
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




