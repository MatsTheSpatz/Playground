
// Namespace 'recipe'
if (typeof recipe == 'undefined') {
    recipe = {};
}

recipe.ingredients = (function () {

    var $ingredients;
    var $add;
    var sectionData; // lazy-loaded html data of a section

    var deleteSection = function ($target) {
        var $item = $target.parent('.ingredientSection');
        $item.remove();
    };

    return {
        init: function ($divIngredients, $buttonAdd) {

            $ingredients = $divIngredients;
            $add = $buttonAdd;

            // add button
            recipe.utilities.convertToJQueryUiButton($add);
            recipe.utilities.subscribe($add, this.addSection);

            // add first section
            var addSectionClosure = (function (instance) {
                return function (data) {
                    sectionData = data;
                    instance.addSection();
                };
            })(this);

            $.get('home/SomeData', addSectionClosure);
        },

        addSection: function () {
            if (!sectionData || sectionData.length == 0) {
                throw new Error('Must call init before addSection.');
            }

            // add a new section in Html
            $ingredients.append(sectionData);

            var $ingredientSection = $('.ingredientSection').last();
            var $ingredientList = $('.sortableIngredients', $ingredientSection).first();
            var $addRowButton = $('.crudButton', $ingredientSection).last();

            if (!$ingredientList.is('ul')) {
                throw ('initialization failure in init(): list element must be of type <ul>.');
            }

            var s = new IngredientSection($ingredientList, $addRowButton, 4);

            // handle delete
            var $deleteSectionButton = $('.crudButton', $ingredientSection).first();
            recipe.utilities.convertToJQueryUiButton($deleteSectionButton);
            recipe.utilities.subscribe($deleteSectionButton, deleteSection, this);
        }
    };
})();

