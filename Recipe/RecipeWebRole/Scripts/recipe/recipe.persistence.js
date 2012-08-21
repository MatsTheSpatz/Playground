
// Namespace 'recipe'
if (typeof recipe == 'undefined') {
    recipe = {};
}

recipe.persistence = (function () {

    var saveInProgressDialogSelector = '#saveInProgress-dialog';
    var saveSucceededDialogSelector = '#saveSucceeded-dialog';
    var saveFailedDialogSelector = '#saveFailed-dialog';
    var recipeAsTextSelector = '#recipeAsText-dialog';

    function initSaveInProgressDialog() {
        $(saveInProgressDialogSelector).dialog({
            height: 120,
            modal: true,
            closeOnEscape: false,
            draggable: false,
            resizable: false,
            title: 'Saving',
            buttons: {},
            dialogClass: 'progressbar-dialog',
            autoOpen: false
        });
    }

    function openSaveInProgressDialog() {
        $(saveInProgressDialogSelector).dialog('open');
    }

    function closeSaveInProgressDialog() {
        $(saveInProgressDialogSelector).dialog('close');
    }

    function initSaveSucceededDialog() {
        $(saveSucceededDialogSelector).dialog({
            height: 120,
            modal: true,
            title: 'Success',
            buttons: [
                { text: 'Continue editing this recipe', click: onContinueEditing },
                { text: "Close, I'am done", click: onDoneEditing}],
            dialogClass: 'normal-dialog',
            autoOpen: false
        });
    }

    function openSaveSucceededDialog() {
        $(saveSucceededDialogSelector).dialog('open');
    }

    function closeSaveSucceededDialog() {
        $(saveSucceededDialogSelector).dialog('close');
    }

    function initSaveFailedDialog() {
        $(saveFailedDialogSelector).dialog({
            height: 300,
            minHeight: 200,
            maxHeight: 400,
            width: 460,
            minWidth: 400,
            maxWidth: 600,
            modal: true,
            title: 'Failure',
            buttons: [
                { text: 'Try saving again now', click: onTrySaveAgain },
                { text: 'Store as cookie', click: onStoreCookie },
                { text: 'Show as text', click: onSaveText },
                { text: 'Cancel', click: onCancelSave}],
            dialogClass: 'normal-dialog',
            autoOpen: false
        });
    }

    function openSaveFailedDialog() {
        $(saveFailedDialogSelector).dialog('open');
    }

    function closeSaveFailedDialog() {
        $(saveFailedDialogSelector).dialog('close');
    }

    function initRecipeAsTextDialog() {
        $(recipeAsTextSelector).dialog({
            height: 520,
            minHeight: 260,
            maxHeight: 800,
            width: 450,
            minWidth: 300,
            maxWidth: 1000,
            modal: true,
            title: 'Recipe',
            buttons: [{ text: 'Close', click: function () { closeRecipeAsTextDialog(); } }],
            dialogClass: 'normal-dialog',
            autoOpen: false
        });
    }

    function openRecipeAsTextDialog() {
        var text = getData();
        $('textarea', $(recipeAsTextSelector)).val(text);

        $(recipeAsTextSelector).dialog('open');
    }

    function closeRecipeAsTextDialog() {
        $(recipeAsTextSelector).dialog('close');
    }

    function postDataToServer(jsonText) {

        openSaveInProgressDialog();

        var jqxhr = $.ajax({
            type: "POST",
            url: "/BlankRecipeEditor/Save",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: jsonText
        })
            .success(function () {
                closeSaveInProgressDialog();
                openSaveSucceededDialog();
            })
            .error(function (xhr, ajaxOptions, thrownError) {
                closeSaveInProgressDialog();

                var text;
                if (xhr.status >= 500) {
                    text = "The server produced an error or is overloaded.";
                } else if (xhr.status >= 400) {
                    text = "Network/Client problem: the server was not reached.";
                } else {
                    text = "Unexpected error.";
                }
                text += ' [Status-code:' + xhr.status + ']';

                $('p.save-failure-reason').html(text);
                openSaveFailedDialog();
            });
    }

    function onContinueEditing() {
        closeSaveSucceededDialog();
    }

    function onDoneEditing() {
        closeSaveSucceededDialog();
        alert("NAVIGATE AWAY!");
    }

    function onTrySaveAgain() {
        closeSaveFailedDialog();
        postDataToServer(getData());
    }

    function onStoreCookie() {
        closeSaveFailedDialog();

        var jsonText = getData();
        recipe.utilities.setCookie("recipeXYZ", jsonText, 30);
    }

    function onSaveText() {
        closeSaveFailedDialog();

        openRecipeAsTextDialog();
    }

    function onCancelSave() {
        closeSaveFailedDialog();
    }

    function getData() {

        var data = {};

        $.each(recipe.generalInfo.getData(), function (name, value) { data[name] = value; });
        $.each(recipe.ingredients.getData(), function (name, value) { data[name] = value; });
        $.each(recipe.instructions.getData(), function (name, value) { data[name] = value; });
        $.each(recipe.categories.getData(), function (name, value) { data[name] = value; });

        var jsonText = window.JSON.stringify(data);
        return jsonText;
    }


    return {
        init: function () {
            var $saveButton = $('#saveRecipe');
            var $showRecipeAsTextButton = $('#showRecipeAsText');

            $saveButton.button({ icons: { primary: 'ui-icon-transferthick-e-w'} });
            $saveButton.click(function () { postDataToServer(getData()); });

            $showRecipeAsTextButton.button({ icons: { primary: 'ui-icon-document'} });
            $showRecipeAsTextButton.click(function () { openRecipeAsTextDialog(); });

            initSaveInProgressDialog();
            initSaveSucceededDialog();
            initSaveFailedDialog();
            initRecipeAsTextDialog();
        },

        setData: function (data) {
            recipe.generalInfo.setData(data);
           // recipe.ingredients.setData(data ? data['Ingredients'] : undefined);
            recipe.ingredients.setData(data);
            recipe.instructions.setData(data);
            recipe.categories.setData(data);
        }


    };
})();

