
var BaseController = function () {
    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {
        $('body').on('click', '.add-to-cart', function (e) {
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

        $('body').on('click', '.remove-cart', function (e) {
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
    }

    function loadHeaderCart() {
        $("#headerCart").load("/AjaxContent/HeaderCart");// load lại view component để cập nhật danh sách sp add vào giỏ hàng
    }

}