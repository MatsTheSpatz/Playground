
// Namespace 'recipe'
if (typeof recipe == 'undefined') {
    recipe = {};
}

recipe.categories = (function () {

    // 'DishCategory' and 'Season' is a server-side an enum and transferred as index!
    var dishCategoryMap = ["aperitif", "soup", "starter", "maincourse", "dessert", "cookies", "other"];
    var seasonMap = ["spring", "summer", "fall", "winter"];
    var skillLevelMap = ["Undefined", "Novice", "Beginner", "Average", "Advanced", "Expert"];

    function mapIndexToString(map, numericItems) {
        var textItems = new Array();
        for (var i = 0; i < numericItems.length; i++) {
            var index = numericItems[i];
            var text = map[index];
            textItems.push(text);
        }
        return textItems;
    }

    function isChecked(elementSelector) {
        var selector = elementSelector + ':checked';
        return ($(selector).val() != undefined);
    }

    function getCheckedItems(fieldsetSelector) {
        var items = new Array();
        var selector = fieldsetSelector + ' input:checked';
        $(selector).each(function (index, element) {
            var id = $(element).attr('id');
            items.push(id);
        });
        return items;
    }

    function setCheckedItems(fieldsetSelector, items) {

        var selector = fieldsetSelector + ' input';
        $(selector).each(function (index, element) {

            var elementName = $(element).attr('id');
            var isChecked = ($.inArray(elementName, items) >= 0);

            $(element).attr('checked', isChecked);
        });
    }

    return {
        init: function () {
            $('div.rateit').rateit();

            // tooltip for rating
            var tooltipvalues = skillLevelMap;
            $("#skillLevel").bind('over', function (event, value) {
                $(this).attr('title', tooltipvalues[value]);
            });
        },

        getData: function () {

            return {
                'IsVegetarian': isChecked('#vegetarian'),
                'IsSuitedAsPresent': isChecked('#present'),
                'DishCategories': getCheckedItems('#dishCategory'),
                'Seasons': getCheckedItems('#season'),
                'SkillLevel': $('#skillLevel').rateit('value')
            };
        },

        setData: function (obj) {

            var isVegetarian = (obj) && ('IsVegetarian' in obj) && (obj['IsVegetarian'] == true);
            $('#vegetarian').attr('checked', isVegetarian);

            var isSuitedAsPresent = (obj) && ('IsSuitedAsPresent' in obj) && (obj['IsSuitedAsPresent'] == true);
            $('#present').attr('checked', isSuitedAsPresent);

            var dishCategoryIndices = (obj && ('DishCategories' in obj) && obj['DishCategories']) ? obj['DishCategories'] : new Array();
            var dishCategories = mapIndexToString(dishCategoryMap, dishCategoryIndices);
            setCheckedItems('#dishCategory', dishCategories);

            var seasonIndices = (obj && ('Seasons' in obj) && obj['Seasons']) ? obj['Seasons'] : new Array();
            var seasons = mapIndexToString(seasonMap, seasonIndices);
            setCheckedItems('#season', seasons);

            var skillLevel = (obj && ('SkillLevel' in obj) && obj['SkillLevel']) ? obj['SkillLevel'] : 3;
            $('#skillLevel').rateit('value', skillLevel);
        }
    };
})();

