
// Namespace 'recipe'
if (typeof recipe == 'undefined') {
    recipe = {};
}

function IngredientList($listElement, $button, inititalRowCount) {

    // Private variables/methods (private variables only accessible to private methods!)
    var self = this;
    var $ingredientList = $listElement;
    var $addButton = $button;
    
    // privileged methods (so that prototype can acces them)
    // A privileged method is able to access the private variables and methods, 
    // and is itself accessible to the public methods and the outside.
    this.getIngredientList = function () {
        return $ingredientList;
    };

    this.getAddButton = function () {
        return $addButton;
    };

    this.init(inititalRowCount);
}

// Prototype for IngredientList
IngredientList.prototype = function () {

    // private helper functions
    var deleteRow = function ($target) {
        var $item = $target.parent('li');
        $item.remove();
    };

    var isLastRow = function ($row) {
        var nextRow = getNextRow($row);
        return (nextRow == undefined);
    };

    var getNextRow = function ($row) {
        var $nextRow = $row.next('.ingredient-item');
        var isLast = ($nextRow.length == 0);
        if (isLast) {
            return undefined;
        }
        return $nextRow;
    };

    var focusRow = function ($row) {
        $('input.rowInput', $row).focus();
    };

    return {
        init: function (rowCount) {
            // sortable
            this.getIngredientList().sortable({ connectWith: ['.sortableIngredients'] });

            // convert add button to JQueryUI-button
            var $addRowButton = this.getAddButton();
            recipe.utilities.convertToJQueryUiButton($addRowButton);
            recipe.utilities.subscribe($addRowButton, this.addAndFocusRow, this);

            // add rows
            for (var i = 0; i < rowCount; i++) {
                this.addRow();
            }
        },

        addAndFocusRow: function () {
            var $newRow = this.addRow();
            focusRow($newRow);
        },

        addRow: function () {

            var elements = '<input class="rowInput idleField" type="text" maxlength="40" /> \
                            <span class="dragHelper ui-icon ui-icon-arrowthick-2-n-s"></span> \
                            <button class="deleteRow" data-button-icon="delete" data-button-action="delete">A</button>';

            var $li = $(document.createElement('li'));
            $li.html(elements);
            $li.addClass('ui-state-default');
            $li.addClass('ingredient-item');

            var list = this.getIngredientList();
            list.append($li);

            // convert button to jQuery UI button
            var $deleteRowButton = $('.deleteRow', $li);
            recipe.utilities.convertToJQueryUiButton($deleteRowButton);
            recipe.utilities.subscribe($deleteRowButton, deleteRow, this);

            // deal with focus-change on input-field
            var $inputField = $('input.rowInput', $li);
            $inputField.focus(function () {
                $(this).removeClass('dileField').addClass('focusField');
            });
            $inputField.blur(function () {
                $(this).removeClass('focusField').addClass('dileField');
            });

            // deal with enter key on input-field
            var addRowClosure = (function (instance) {
                return function () {
                    instance.addRow();
                };
            })(this);

            $inputField.keypress(function (e) {
                if (e.keyCode == 13 || e.key == "Enter") { // keycode for chrome                    
                    var $currentRow = $(this).parent('.ingredient-item');
                    var $nextRow;
                    if (isLastRow($currentRow)) {
                        e.preventDefault();  // problem with ie: the event is used to trigger a 'delete row'-click.
                        e.stopPropagation();

                        $nextRow = addRowClosure();
                    } else {
                        $nextRow = getNextRow($currentRow);
                    }
                    focusRow($nextRow);
                }
            });
            return $li;
        },

        getCount: function () {
            var $ingredientList = this.getIngredientList();
            return $('.ingredient-item', $ingredientList).length;
        },
        
        getData: function() {
            var $ingredientList = this.getIngredientList();

            var ingredientItems = [];
            $('input.rowInput', $ingredientList).each(function() {
                var text = $(this).val();
                var trimmedText = $.trim(text);
                if (trimmedText.length > 0) {
                    ingredientItems.push(trimmedText);
                }
            });

            return ingredientItems;
        },

        constructor: IngredientList
    };
} ();
