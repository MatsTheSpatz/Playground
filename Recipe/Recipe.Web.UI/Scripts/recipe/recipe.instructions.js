
// Namespace 'recipe'
if (typeof recipe == 'undefined') {
    recipe = {};
}

recipe.formats = {
    Number: 0,
    Bullet: 1,
    Dash: 2,
    None: 3
};

recipe.instructions = (function () {

    var $enclosingDiv;
    var lastFocusedElement;
    var editorWasOpenOnMouseDown = false;
    var levelFormats = [recipe.formats.Number, recipe.formats.Bullet, recipe.formats.Dash];

    function createAddInstructionButton() {
        var $addButton = $(document.createElement('button'));
        $addButton.html('Add Row');
        $addButton.addClass('addInstruction');
        $addButton.attr("data-button-icon", "add");

        $addButton.click(function () {
            var $newInstructionDiv = addRow();
            openEditor($newInstructionDiv);
        });

        recipe.utilities.convertToJQueryUiButton($addButton);
        return $addButton;
    }

    function addRow(text, level) {
        var $newInstructionDiv = createInstructionDiv(text, level);

        // insert after last existing instruction (if there is one...)
        var $lastInstructionDiv = $('div.instruction', $enclosingDiv).last();
        if ($lastInstructionDiv.length == 0) {
            $enclosingDiv.prepend($newInstructionDiv);  // first one!
        } else {
            $lastInstructionDiv.after($newInstructionDiv);
        }

        updateMarkers();
        return $newInstructionDiv;
    }

    function createInstructionDiv(text, level) {
        var $instructionDiv = $(document.createElement('div'));
        $instructionDiv.addClass('instruction');

        var $marker = createMarker();
        var $textSpan = createTextSpan(text);
        var $buttonLeft = createIndentLeftButton();
        var $buttonRight = createIndentRightButton();
        var $buttonDelete = createDeleteInstructionButton();

        $instructionDiv.append($marker, $textSpan, $buttonLeft, $buttonRight, $buttonDelete);

        if (!level) {
            level = 0;
        }
        setIndentationLevel($instructionDiv, level);
        return $instructionDiv;
    }

    function createMarker() {
        var $marker = $(document.createElement('span'));
        $marker.addClass('marker');
        $marker.html('1.');
        return $marker;
    }

    function createTextSpan(text) {
        var $textSpan = $(document.createElement('span'));
        $textSpan.addClass('instructionText');
        $textSpan.html(text ? text : '');
        
        return $textSpan;
    }

    function createIndentLeftButton() {
        var $buttonLeft = $(document.createElement('button'));
        $buttonLeft.html('L');
        $buttonLeft.addClass('moveLevelLeft');
        $buttonLeft.attr("data-button-icon", "left");

        $buttonLeft.click(function (event) {
            var $button = $(event.target);
            var $instructionDiv = $button.parent('div.instruction');
            decreaseIndentationLevel($instructionDiv);
            updateMarkers();
        });

        recipe.utilities.convertToJQueryUiButton($buttonLeft);
        return $buttonLeft;
    }

    function createIndentRightButton() {
        var $buttonRight = $(document.createElement('button'));
        $buttonRight.html('R');
        $buttonRight.addClass('moveLevelRight');
        $buttonRight.attr("data-button-icon", "right");

        $buttonRight.click(function (event) {
            var $button = $(event.target);
            var $instructionDiv = $button.parent('div.instruction');
            increaseIndentationLevel($instructionDiv);
            updateMarkers();
        });

        recipe.utilities.convertToJQueryUiButton($buttonRight);
        return $buttonRight;
    }

    function createDeleteInstructionButton() {
        var $buttonDelete = $(document.createElement('button'));
        $buttonDelete.html('X');
        $buttonDelete.addClass('deleteInstruction');
        $buttonDelete.attr("data-button-icon", "delete");

        $buttonDelete.click(function (event) {
            var $button = $(event.target);
            var $instructionDiv = $button.parent('div.instruction');
            $instructionDiv.remove();
        });

        recipe.utilities.convertToJQueryUiButton($buttonDelete);
        return $buttonDelete;
    }

    function getText($instructionDiv) {
        var $textSpan = $('.instructionText', $instructionDiv);
        var text = $textSpan.html();
        return $.trim(text);
    }

    function getRowData($instructionDiv) {
        var level = getIndentationLevel($instructionDiv);
        var text = getText($instructionDiv);

        if (!text || text.length == 0) {
            return undefined;
        }
        return { 'Text': text, 'Level': level };
    }

    function getIndentationLevel($instructionDiv) {
        var levelText = $('span.marker', $instructionDiv).attr('indentation');
        var level = levelText.substring(5, 6);
        return parseInt(level);
    }

    function setIndentationLevel($instructionDiv, level) {
        if (level < 0) {
            level = 0;
        } else if (level > 2) {
            level = 2;
        }
        // indentation level goes into marker and instruction text!
        $('span.marker', $instructionDiv).attr('indentation', 'level' + level);
        $('span.instructionText', $instructionDiv).attr('indentation', 'level' + level);
    }

    function increaseIndentationLevel($instructionDiv) {
        var level = getIndentationLevel($instructionDiv);
        setIndentationLevel($instructionDiv, level + 1);
    }

    function decreaseIndentationLevel($instructionDiv) {
        var level = getIndentationLevel($instructionDiv);
        setIndentationLevel($instructionDiv, level - 1);
    }

    function onEnterKeyDownInTextArea($textarea) {
        // no need to close editor - it will close as part of losing the focus!

        var $thisInstructionDiv = $textarea.parent('div.instruction');
        var $nextInstructionDiv = $thisInstructionDiv.next('div.instruction');

        if (!$nextInstructionDiv || $nextInstructionDiv.length < 1) {
            // this was the last instruction.
            // create a new instruction and open the editor on the new line..
            var level = getIndentationLevel($thisInstructionDiv);
            $nextInstructionDiv = createInstructionDiv('', level);
            $nextInstructionDiv.insertAfter($thisInstructionDiv);
        }

        openEditor($nextInstructionDiv);
    }

    function onLostFocusInTextArea($textarea) {
        var $instructionDiv = $textarea.parent('div.instruction');
        closeEditor($instructionDiv);
    }

    function openEditor($instructionDiv) {

        var $spanText = $('span.instructionText', $instructionDiv).first();
        var $textarea = $(document.createElement('textarea'));

        var width = $spanText.css('width');
        var leftMargin = $spanText.css('margin-left');
        var rightMargin = $spanText.css('margin-right');

        $textarea.addClass('instruction');
        $textarea.attr("cols", "4");
        $textarea.attr("rows", "4");
        $textarea.css("width", width);
        $textarea.css("margin-left", leftMargin);
        $textarea.css("margin-right", rightMargin);

        var currentText = $spanText.html();
        $textarea.val(currentText);

        // key down
        $textarea.keydown(function (e) {
            if (e.keyCode == 13 || e.key == "Enter") { // keycode for chrome

                e.preventDefault();  // problem with ie: the event is used to trigger a 'delete row'-click.
                e.stopPropagation();

                onEnterKeyDownInTextArea($(e.target));
            }
        });

        // lose focus
        $textarea.blur(function (e) {
            onLostFocusInTextArea($(e.target));
        });

        // get focus
        $textarea.focus(function (e) {
            var $textarea = $(e.target).first();
            var text = $textarea.val().trim();

            setSelectionRange(e.target, 0, text.length);
        });

        $spanText.replaceWith($textarea);
        $textarea.get(0).focus();
    }

    function closeEditor($instructionDiv) {

        // read text from open textarea.
        var $textarea = $('textarea', $instructionDiv).first();
        var text = $textarea.val(); // don't use html()        
        text = $.trim(text);

        if (text.length >= 1) {
            // replace textarea with textspan.
            var $textSpan = createTextSpan(text);
            $textarea.replaceWith($textSpan);

            //var $marker = $('span.marker', $instructionDiv);
            var indentationLevel = getIndentationLevel($instructionDiv);
            setIndentationLevel($instructionDiv, indentationLevel);
        } else {
            // drop this entry
            $instructionDiv.remove();
        }
    }

    function setSelectionRange(element, selectionStart, selectionEnd) {
        if (element.setSelectionRange) {
            element.focus();
            element.setSelectionRange(selectionStart, selectionEnd);
        }
        else if (element.createTextRange) {
            var range = element.createTextRange();
            range.collapse(true);
            range.moveEnd('character', selectionEnd);
            range.moveStart('character', selectionStart);
            range.select();
        }
    }

    function updateMarkers() {
        var counterByLevel = [0, 0, 0];

        $('div.instruction').each(function () {
            var $instructionDiv = this;
            var level = getIndentationLevel($instructionDiv);

            // reset counter of higher level
            if (level <= 1) counterByLevel[2] = 0;
            if (level <= 0) counterByLevel[1] = 0;

            // increase this level
            counterByLevel[level]++;

            updateMarker($instructionDiv, level, counterByLevel[level]);
        });
    }

    function updateMarker($instructionDiv, level, number) {
        var format = levelFormats[level];
        var text;
        if (format == recipe.formats.Number) {
            text = number + '.';
        } else if (format == recipe.formats.Bullet) {
            text = '#';
        } else if (format == recipe.formats.Dash) {
            text = '-';
        } else {
            text = ' ';
        }
        $('span.marker', $instructionDiv).html(text);
    }

    function isValidFormat(format) {
        return (format == recipe.formats.Number ||
                format == recipe.formats.Bullet ||
                format == recipe.formats.Dash ||
                format == recipe.formats.None);
    }

    return {
        init: function ($instructionsDiv) {

            $enclosingDiv = $instructionsDiv;
            $enclosingDiv.append(createAddInstructionButton());

            // Handle click on entire document!
            // Problem was: 
            //    When click is made on instrument-text below open textarea,
            //    the click event is only interpreted after the textarea was closed (blur-event before click-event).
            //    Hence there is no target for the click anymore.
            // Workaround:
            //    mousedown-event fires before blur event. Catch location there.
            $(document).click(function (e) {
                if ($(lastFocusedElement).hasClass('instructionText')) {
                    var $instructionDiv = $(lastFocusedElement).parent('div.instruction');
                    openEditor($instructionDiv);
                }
            });
            $(document).mouseup(function () {
                if (editorWasOpenOnMouseDown &&
                    $(lastFocusedElement).parent('button.addInstruction').length == 1) {
                    var $newInstructionDiv = addRow();
                    openEditor($newInstructionDiv);
                }
            });
            $(document).mousedown(function (e) {
                window.mouseXPos = e.clientX;
                window.mouseYPos = e.clientY;

                // remember where user pressed the mouse.
                lastFocusedElement = document.elementFromPoint(window.mouseXPos, window.mouseYPos);
                editorWasOpenOnMouseDown = $('textarea', $enclosingDiv).length == 1;
            });
        },

        setLevelFormat: function (level, format) {
            if (level < 0 || level > 2) {
                throw new Error('Invalid level');
            }

            if (isNaN(format)) {
                if (format in recipe.formats) {
                    format = recipe.formats[format];
                } else {
                    throw new Error('Invalid format');
                }
            }
            if (!isValidFormat(format)) {
                throw new Error('Invalid format');
            }

            levelFormats[level] = format;
            updateMarkers();
        },

        setLevelFormats: function (level0Format, level1Format, level2Format) {
            this.setLevelFormat(0, level0Format);
            this.setLevelFormat(1, level1Format);
            this.setLevelFormat(2, level2Format);
        },

        getData: function () {
            var instructions = [];
            $('.instruction', this.$enclosingDiv).each(function () {
                var instruction = getRowData($(this));
                if (instruction) {
                    instructions.push(instruction);
                }
            });
            return (instructions.length == 0) ? undefined : instructions;
        },

        setData: function (instructions) {
            if (instructions && instructions.length >= 1) {
                // fill in existing data
                for (var i = 0; i < instructions.length; i++) {
                    var rowData = instructions[i];
                    addRow(rowData.Text, rowData.Level);
                }
            } else {
                // add two empty instruction fields
                for (var i = 0; i < 2; i++) {
                    addRow('', 0);
                }
            }
        }
    };
})();

