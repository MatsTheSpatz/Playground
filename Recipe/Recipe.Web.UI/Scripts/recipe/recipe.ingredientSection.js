
// Namespace 'recipe'
if (typeof recipe == 'undefined') {
    recipe = {};
}

function IngredientSection($listElement, $button, inititalRowCount) {

    // By convention, we make a private self variable. 
    // This is used to make the object available to the private methods.
    // This is a workaround for an error in the ECMAScript Language Specification which causes this
    // to be set incorrectly for inner functions.
    // source: http://www.crockford.com/javascript/private.html

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

// public methods for the IngredientSection
IngredientSection.prototype = function () {

    // private helper functions
    var deleteRow = function($target) {
        var $item = $target.parent('li');
        $item.remove();
    };

    return {
        init: function (rowCount) {
            // sortable
            this.getIngredientList().sortable({ connectWith: ['.sortableIngredients'] });

            // convert add button to JQueryUI-button
            var $addButton = this.getAddButton();
            recipe.utilities.convertToJQueryUiButton($addButton);
            recipe.utilities.subscribe($addButton, this.addRow, this);

            // add rows
            for (var i = 0; i < rowCount; i++) {
                this.addRow();
            }
        },

        addRow: function () {

            var elements = '<span class="ui-icon ui-icon-arrowthick-2-n-s"></span> \
                        <input class="rowInput" type="text" maxlength="40" /> \
                        <button class="deleteRow" data-button="true" data-button-icon="delete" data-button-action="delete">A</button>';

            var $li = $(document.createElement('li'));
            $li.html(elements);
            $li.addClass('ui-state-default');
            $li.addClass('ingredient-item');

            var list = this.getIngredientList();
            list.append($li);

            // convert new inner elements to jQuery UI button
            var $buttons = $('button[data-button=true]', $li);
            $buttons.each(function () {
                recipe.utilities.convertToJQueryUiButton($(this));
                recipe.utilities.subscribe($(this), deleteRow, this);
            });
        },

        constructor: IngredientSection
    };
}();
