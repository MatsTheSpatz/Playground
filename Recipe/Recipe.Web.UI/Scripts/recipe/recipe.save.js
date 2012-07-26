
// Namespace 'recipe'
if (typeof recipe == 'undefined') {
    recipe = {};
}

recipe.save = (function () {

    var saveInProgressDialogSelector = '#saveInProgress-dialog';
    var saveSucceededDialogSelector = '#saveSucceeded-dialog';


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

    function postData(jsonText) {

        openSaveInProgressDialog();

        var jqxhr = $.post("/Home/SetRecipe", jsonText)
            .success(function () {
                closeSaveInProgressDialog();
                openSaveSucceededDialog();
            })
            .error(function () {
                closeSaveInProgressDialog();
                alert('oops');
            });
    }

    function onContinueEditing() {
        closeSaveSucceededDialog();
    }

    function onDoneEditing() {
        closeSaveSucceededDialog();
        alert("NAVIGATE AWAY!");
    }

    return {
        init: function ($saveButton) {

            $saveButton.button({ icons: { primary: 'ui-icon-disk'} });
            $saveButton.click(function () {

                // collect data
                var data = {
                    'ingredients': recipe.ingredients.getData(),
                    'instructions': recipe.instructions.getData()
                };

                // post to server
                var jsonText = window.JSON.stringify(data);
                postData(jsonText);
            });

            initSaveInProgressDialog();
            initSaveSucceededDialog();
        }
    };
})();

