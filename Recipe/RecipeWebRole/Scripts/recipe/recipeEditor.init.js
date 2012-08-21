

// Namespace 'recipe'
if (typeof recipeEditor == 'undefined') {
    recipe = {};
}

recipeEditor.init = function () {

    // activate accordions before tabs!
    $("#instructionFormat").accordion({
        collapsible: true,
        minHeight: 260
    });

    $("#tabs").tabs();

    // init tabs
    recipe.generalInfo.init();
    recipe.ingredients.init();
    recipe.instructions.init();
    recipe.categories.init();

    // init persistence
    recipe.persistence.init();


    $(window).resize(function () {
        var height = $(window).height() - 124;
        $('.ui-tabs-panel').css('height', height + 'px');
    });
    $(window).trigger("resize"); 
};
