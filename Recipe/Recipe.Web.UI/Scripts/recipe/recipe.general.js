
// Namespace 'recipe'
if (typeof recipe == 'undefined') {
    recipe = {};
}

recipe.general = (function () {

    // id
    function getRecipeId() {
        return $('#recipeId').html();
    }

    function setRecipeId(text) {
        return $('#recipeId').html(text);
    }

    // name
    function getRecipeName() {
        return $('#recipeName').val();
    }

    function setRecipeName(text) {
        return $('#recipeName').val(text);
    }

    return {
        init: function () {

        },

        getData: function () {
            return {
                'Id': getRecipeId(),
                'Name': getRecipeName()
            };
        },

        setData: function (obj) {
            setRecipeId(obj['Id']);
            setRecipeName(obj['Name']);
        }
    };
})();

