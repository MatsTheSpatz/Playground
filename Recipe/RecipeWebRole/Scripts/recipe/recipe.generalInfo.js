
// Namespace 'recipe'
if (typeof recipe == 'undefined') {
    recipe = {};
}

recipe.generalInfo = (function () {

    // name
    function getRecipeName() {
        return $('#recipeName').val();
    }

    function setRecipeName(text) {
        return $('#recipeName').val(text);
    }

    // description
    function getDescription() {
        return $('#description').val();
    }

    function setDescription(text) {
        return $('#description').val(text);
    }

    function getDateTime(elementSelector) {
        
        var dateTimeText = $(elementSelector).attr('data-dateTime');
        if (dateTimeText && dateTimeText.length > 0) {
            var dateTime = new Date(dateTimeText);
            return dateTime.toISOString();
        }

        return undefined;
    }

    function setDateTime(elementSelector, propertyName, obj) {

        var dateText = '-';
        var date = undefined;

        if (obj && propertyName in obj) {
            var text = obj[propertyName];
            if (text && text.length > 0) {
                date = new Date(parseInt(text.substr(6)));
                dateText = formatDateTime(date);
            }
        }

        $(elementSelector).html(dateText);
        $(elementSelector).attr('data-dateTime', date);
    }

    function formatDateTime(dateTime) {

        var month = dateTime.getMonth() + 1;
        var day = dateTime.getDate();
        var year = dateTime.getFullYear();
        var hours = dateTime.getHours();
        var minutes = dateTime.getMinutes();

        return day + '.' + month + '.' + year + ' um ' + hours + ':' + (minutes < 10 ? '0' : '') + minutes;
    }


    return {
        init: function () {

        },

        getData: function () {
            return {
                'Id': $('#recipeId').html(),
                'Author': $('#author').html(),

                'Name': getRecipeName(),
                'Description': getDescription(),

                'CreationDate': getDateTime('#creationDate'),
                'ModificationDate': getDateTime('#modificationDate')
            };
        },

        setData: function (obj) {

            var recipeId = (obj && 'Id' in obj) ? obj['Id'] : '-';
            $('#recipeId').html(recipeId);

            var author = (obj && 'Author' in obj) ? obj['Author'] : '';
            $('#author').html(author);

            var name = (obj && 'Name' in obj) ? obj['Name'] : '';
            setRecipeName(name);

            var description = (obj && 'Description' in obj) ? obj['Description'] : '';
            setDescription(description);

            setDateTime('#creationDate', 'CreationDate', obj);
            setDateTime('#modificationDate', 'ModificationDate', obj);
        }
    };
})();

