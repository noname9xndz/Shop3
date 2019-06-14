
var BaseController = function () {

    
    var self = this;
    var cachedObj = {
        colors: [],
        sizes: []
    }

    this.initialize = function () {

        loadColors();
        loadSizes();
        registerEvents();
        
    }
    
    function registerEvents() {
        $('body').on('click','.add-to-cart,#add-to-cart',function (e) {
                e.preventDefault();
                var id = $(this).data('id');
                $.ajax({
                    url: '/Cart/AddToCart',
                    type: 'post',
                    data: {
                        productId: id,
                        quantity: 1,
                        color: 1,
                        size: 1
                    },
                    success: function (response) {
                        // common.notify('Product was added to cart', 'success');
                        common.notify(resources["AddCartOK"], 'success'); // đa ngôn ngữ truy cập dạng key value
                        loadHeaderCart();
                    }
                });
            });

        $('body').on('click','.remove-cart',function (e) {
                e.preventDefault();
                var id = $(this).data('id');
                $.ajax({
                    url: '/Cart/RemoveFromCart',
                    type: 'post',
                    data: {
                        productId: id
                    },
                    success: function (response) {
                        //common.notify('Product was removed', 'success');
                        common.notify(resources["RemoveCartOK"], 'success');
                        loadHeaderCart();
                    }
                });
            });

        
        $('body').on('click', '.quick-view', function (e) {
            e.preventDefault();
            $('#modal-add-edit').modal('show');
            var that = $(this).data('id');
            //loadQuantities(that);
            GetProductBy(that);
            

        });

        $('body').on('click', '.add-to-cart-quickview', function (e) {

            $('#modal-add-edit').modal('hide');
            //e.preventDefault();

            var id = parseInt($(this).data('id'));
            var colorId = parseInt($('.ddlColorId').val());
            var sizeId = parseInt($('.ddlSizeId').val());
            $.ajax({
                url: '/Cart/AddToCart',
                type: 'post',
                dataType: 'json',
                data: {
                    productId: id,
                    quantity: parseInt($('#txtQuantity').val()),
                    color: colorId,
                    size: sizeId
                },
                success: function () {
                    common.notify('Product was added successful', 'success');
                    loadHeaderCart(); // load lại view component để cập nhật danh sách sp add vào giỏ hàng
                }
            });
        });

        $('body').on('click', '#add_to_wishlist', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "POST",
                url: "/Cart/addWishList",
                data: { productId : that },
                dataType: "json",
                beforeSend: function () {
                    common.startLoading();
                },
                success: function (response) {
                    if (response.status == "error") {

                        common.notify('Product already exists in the wish list', 'error');
                        common.stopLoading();
                    } else {
                        common.notify('add to wishlist successful', 'success');
                        common.stopLoading();
                    }
                    

                },
                error: function (status) {
                    common.notify('please login', 'error');
                    common.stopLoading();
                }
            });
        });

        
    }

    function loadHeaderCart() {
        $("#headerCart").load("/AjaxContent/HeaderCart"); // load lại view component để cập nhật danh sách sp add vào giỏ hàng
    }

    function GetProductBy(id) {

        var template = $('#table-template').html();
        var render = "";

        $.ajax({
            type: "GET",
            url: "/Home/GetById",
            data: { id: id },
            dataType: "json",
            success: function (response) {
               render += Mustache.render(template,
                        {
                            Id: response.Id,
                            Name: response.Name,
                            Image: response.Image == null
                                ? '<img src="../admin-side/images/user.png?w=321" />'
                                : '<img src="' + response.Image + '?w=321"/>',
                            // Price: common.formatNumber(item.Price, 0),
                            Price: response.PromotionPrice > 0 && response.PromotionPrice < response.Price ? response.PromotionPrice : response.Price,
                            Description: response.Description == null ? "no description" : response.Description,
                            Status: common.getStatus(response.Status),
                            Colors: getColorOptions(),
                            Sizes: getSizeOptions()

                        });
               
                if (render != '') {
                    $("#tbl-content").html(render);
                }

            },
            error: function (status) {
                common.notify('Cannot loading data', 'error');
                common.stopLoading();
            }
        });
    }

    function loadColors() {
        return $.ajax({
            type: "GET",
            url: "/Home/GetColors",
            dataType: "json",
            success: function (response) {
                cachedObj.colors = response;
            },
            error: function () {
                common.notify('Cannot loading data', 'error');
            }
        });
    }

    function loadSizes() {
        return $.ajax({
            type: "GET",
            url: "/Home/GetSizes",
            dataType: "json",
            success: function (response) {
                cachedObj.sizes = response;
            },
            error: function () {
                common.notify('Cannot loading data', 'error');
            }
        });
    }


    function getColorOptions() {
        var colors = "<select class='form-control ddlColorId'>";
        $.each(cachedObj.colors, function (i, color) {
               colors += '<option value="' + color.Id + '">' + color.Name + '</option>';
        });
        colors += "</select>";
        return colors;
    }

    function getSizeOptions() {
        var sizes = "<select class='form-control ddlSizeId'>";
        $.each(cachedObj.sizes, function (i, size) {
                sizes += '<option value="' + size.Id + '" selected="select">' + size.Name + '</option>';
        });
        sizes += "</select>";
        return sizes;
    }

    

    

    

}