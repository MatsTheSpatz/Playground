
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
        //        var $parentDiv = $target.parent('div');
        //        var $item = $('.ingredientSection', $parentDiv);
        //        $target.remove(); // delete button
        $item.remove(); // content
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
            var $sectionData = $(sectionData);

            $ingredients.append($sectionData);


            var $ingredientSection = $('.ingredientSection').last();
            var $ingredientList = $('.sortableIngredients', $ingredientSection).first();
            var $addRowButton = $('.addRow', $ingredientSection).first();

            // deal with focus-change on input-field
            var $sectionHeaderInput = $('.sectionHeaderInput', $ingredientSection);
            $sectionHeaderInput.focus(function () {
                $(this).removeClass('dileField').addClass('focusField');
            });
            $sectionHeaderInput.blur(function () {
                $(this).removeClass('focusField').addClass('dileField');
            });

            if (!$ingredientList.is('ul')) {
                throw ('initialization failure in init(): list element must be of type <ul>.');
            }

            // create new list for this section
            var s = new IngredientSection($ingredientList, $addRowButton, 4);

            // handle delete
            var $deleteSectionButton = $('.deleteSection', $sectionData).first();
            recipe.utilities.convertToJQueryUiButton($deleteSectionButton);
            recipe.utilities.subscribe($deleteSectionButton, deleteSection, this);
        }

    };
})();

