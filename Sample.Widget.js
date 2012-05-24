<script type="text/javascript">
    // WIDGET

    var colorPrototype = {

        // function & properties by widget convention
        options: { r:200, g:20, b:10 },

        // Set up the widget
        _create: function () {
            
        },

        _init: function () {
            this.options.g = 120;
        },
        
        // custom functions
        setR: function (rValue) {
            this.options.r = rValue;
        },
        
        getR: function () {
            return this.options.r;
        },
        
        setG: function (gValue) {
            this.options.g = gValue;
        },        

        getG: function () {
            return this.options.g;
        }
    };

    $.widget('ui.matswidget', colorPrototype);

</script>