

var CatalogController = function () {

    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {
        $('.largemenu').on('click', function (e) {
            e.preventDefault();
            $(".products-grid").removeAttr("hidden");
            $(".products-list").attr("hidden", true);
            
        });

        $('.listmenu').on('click', function (e) {
            e.preventDefault();
            $(".products-list").removeAttr("hidden");
            $(".products-grid").attr("hidden", true);

        });

     
    }


    }