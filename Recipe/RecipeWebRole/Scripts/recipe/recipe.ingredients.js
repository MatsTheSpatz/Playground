
// Namespace 'recipe'
if (typeof recipe == 'undefined') {
    recipe = {};
}

recipe.ingredients = (function () {

    var $ingredientsDiv;
    var $addSectionButton;
    var sectionInfos = [];

    var deleteSectionClicked = function ($target) {

        for (var i = 0; i < sectionInfos.length; i++) {
            if (sectionInfos[i].deleteButton.get(0) == $target.get(0)) {
                // remove from DOM
                var section = sectionInfos[i].section;
                section.remove();

                // remove from internal array
                sectionInfos.splice(i, 1);
            }
        }
    };

    return {
        init: function () {
            $ingredientsDiv = $('#ingredientSections');
            $addSectionButton = $('#addIngredientSection');

            // convert add-button to jquery UI button
            recipe.utilities.convertToJQueryUiButton($addSectionButton);
            recipe.utilities.subscribe($addSectionButton, this.addSection);
        },

        addSection: function () {

            // create and add section to DOM
            var section = new IngredientSection($ingredientsDiv);
            var $ingreidentSectionDiv = $('.ingredientSection', $ingredientsDiv).last();

            // create delete-button for this new section
            // and add it as last element to new section (TODO: should be AFTER div)
            var $deleteSectionButton = $(document.createElement('button'));
            $deleteSectionButton.html('Delete Section');
            $deleteSectionButton.addClass('deleteSection');
            $deleteSectionButton.attr("data-button-icon", "delete");

            $ingreidentSectionDiv.append($deleteSectionButton);

            recipe.utilities.convertToJQueryUiButton($deleteSectionButton);
            recipe.utilities.subscribe($deleteSectionButton, deleteSectionClicked);

            // keep track of all sections
            var sectionInfo = { 'section': section, 'deleteButton': $deleteSectionButton };
            sectionInfos.push(sectionInfo);

            return section;
        },

        getData: function () {
            var sections = [];
            for (var i = 0; i < sectionInfos.length; i++) {
                var section = sectionInfos[i].section;
                var sectionData = section.getData();
                if (sectionData) {
                    sections.push(sectionData);
                }
            }

            return {
                'Quantity': $('#ingredientQuantity').val(),
                'IngredientSections': sections
            };
        },

        setData: function (data) {

            var sections = (data && 'IngredientSections' in data) ? data['IngredientSections'] : undefined;
            var quantity = (data && 'Quantity' in data) ? data['Quantity'] : '';

            $('#ingredientQuantity').val(quantity);

            var section;
            if (sections && sections.length >= 1) {
                for (var i = 0; i < sections.length; i++) {
                    section = this.addSection();
                    section.setData(sections[i]);
                }
            } else {
                // add first section
                section = this.addSection();
                section.setData();
            }
        }
    };
})();

