<script type="text/javascript">
    // define plugin
    (function ($) {

        var methods = {
            makeGreen: function () {
                return this.each(function () {
                    $(this).css({ background: 'green' });
                });
            },
            makeRed: function () {
                return this.each(function () {
                    $(this).css({ background: 'red' });
                });
            }
        };

        $.fn.colorChooser = function (method) {

            if (methods[method]) {
                return methods[method].apply(this);
            } else if (typeof method === 'object' || !method) {
                return methods.makeGreen.apply(this);
            } else {
                $.error('Method ' + method + ' does not exist on jQuery.colorChooser');
            }
        };

    })(jQuery);     
</script>