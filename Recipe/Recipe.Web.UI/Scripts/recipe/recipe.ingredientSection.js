
// Namespace 'recipe'
if (typeof recipe == 'undefined') {
    recipe = {};
}

function IngredientSection($ingredientSectionsDiv) {    
    // members variables
    this.ingredientList = undefined;
    this.$ingredientSectionDiv = undefined;
     
    // initialize
    this.init($ingredientSectionsDiv);
}

IngredientSection.prototype = function () {

    var sectionData = '\
      <div class="ingredientSection"> \
        <input class="sectionHeaderInput idleField" type="text" maxlength="40" size="40" placeholder="Section Header" />(Section Header) \
        <ul class="sortableIngredients"> \
        </ul> \
        <button class="addRow" data-button-icon="add">Add Row</button> \
      </div>';

    return {
        init: function ($ingredientSectionsDiv) {

            // add a new section to the end of all sections
            var $sectionData = $(sectionData);
            $ingredientSectionsDiv.append($sectionData);
            var $ingredientSection = $('.ingredientSection', $ingredientSectionsDiv).last();

            // deal with focus-change on input-field
            var $sectionHeaderInput = $('.sectionHeaderInput', $ingredientSection);
            $sectionHeaderInput.focus(function () {
                $(this).removeClass('dileField').addClass('focusField');
            });
            $sectionHeaderInput.blur(function () {
                $(this).removeClass('focusField').addClass('dileField');
            });

            // create new list for this section
            var $ingredientList = $('.sortableIngredients', $ingredientSection);
            var $addRowButton = $('.addRow', $ingredientSection);

            var list = new IngredientList($ingredientList, $addRowButton);

            this.ingredientList = list;
            this.$ingredientSectionDiv = $ingredientSection;
        },

        remove: function () {
            // TODO: does this need any cleanup?
            this.$ingredientSectionDiv.remove();
        },

        getData: function () {
            // header data
            var $sectionHeaderInput = $('.sectionHeaderInput', this.$ingredientSectionDiv);
            var text = $sectionHeaderInput.val();
            var headerText = $.trim(text);

            // ingredients
            var items = this.ingredientList.getData();

            // header or text must exist to be valid!
            if (headerText.length == 0 && items.length == 0) {
                return undefined;
            }

            return { 'SectionHeader': headerText, 'Items': items };
        },

        setData: function (section) {
            // header data
            var $sectionHeaderInput = $('.sectionHeaderInput', this.$ingredientSectionDiv);
            var header = (section && ['SectionHeader'] in section) ? section['SectionHeader'] : '';
            $sectionHeaderInput.val(header);

            // ingredients
            var items = (section && 'Items' in section) ? section['Items'] : undefined;
            this.ingredientList.setData(items);
        }
    };
} ();

