
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
            var id = (obj && 'Id' in obj) ? obj['Id'] : '-';
            setRecipeId(id);

            var name = (obj && 'Name' in obj) ? obj['Name'] : '';
            setRecipeName(name);
            
//            if (obj) {
//                if ('Id' in obj)
//                    setRecipeId(obj['Id']);

//                if ('Name' in obj)
//                    setRecipeName(obj['Name']);
//            }
        }
    };
})();

