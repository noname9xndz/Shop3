

var CartController = function () {
    var cachedObj = {
        colors: [],
        sizes: [],
    }
    this.initialize = function () {
        $.when(loadColors(), // when đảm bảo thực thi trong when trước sau đó mới then (thay cho callback)
               loadSizes())
            .then(function () {
                loadData();
            });

        registerEvents();
    }

    function registerEvents() {

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            $.ajax({
                url: '/Cart/RemoveFromCart',
                type: 'post',
                data: {
                    productId: id
                },
                success: function () {
                    common.notify('Removing product is successful.', 'success');
                    loadHeaderCart(); // load lại view component để cập nhật danh sách sp add vào giỏ hàng
                    loadData();
                }
            });
        });

        $('body').on('keyup', '.txtQuantity', function (e) {
            e.preventDefault();
            var id = parseInt($(this).closest('tr').data('id'));
            var colorId = $(this).val();
            var qty = $(this).closest('tr').find('.txtQuantity').first().val(); // lấy ra tr cha đầu tiên và find class txtQuantity và lấy giá trị đầu
            var sizeId = $(this).closest('tr').find('.ddlSizeId').first().val();
            if (qty > 0) {
                $.ajax({
                    url: '/Cart/UpdateCart',
                    type: 'post',
                    data: {
                        productId: id,
                        quantity: qty,
                        color: colorId,
                        size: sizeId
                    },
                    success: function () {
                        common.notify('Update quantity is successful', 'success');
                        loadHeaderCart(); // load lại view component để cập nhật danh sách sp add vào giỏ hàng
                        loadData();
                    }
                });
            } else {
                common.notify('Your quantity is invalid', 'error');
            }

        });

        $('body').on('change', '.ddlColorId', function (e) {
            e.preventDefault();
            var id = parseInt($(this).closest('tr').data('id'));
            var colorId = $(this).val();
            var q = $(this).closest('tr').find('.txtQuantity').first().val(); // lấy ra tr cha đầu tiên và find class txtQuantity và lấy giá trị đầu
            var sizeId = $(this).closest('tr').find('.ddlSizeId').first().val();

            if (q > 0) {
                $.ajax({
                    url: '/Cart/UpdateCart',
                    type: 'post',
                    data: {
                        productId: id,
                        quantity: q,
                        color: colorId,
                        size: sizeId
                    },
                    success: function () {
                        common.notify('Update quantity is successful', 'success');
                        loadHeaderCart();
                        loadData();
                    }
                });
            } else {
                common.notify('Your quantity is invalid', 'error');
            }

        });

        $('body').on('change', '.ddlSizeId', function (e) {
            e.preventDefault();
            var id = parseInt($(this).closest('tr').data('id'));
            var sizeId = $(this).val();
            var q = parseInt($(this).closest('tr').find('.txtQuantity').first().val());
            var colorId = parseInt($(this).closest('tr').find('.ddlColorId').first().val());
            if (q > 0) {
                $.ajax({
                    url: '/Cart/UpdateCart',
                    type: 'post',
                    data: {
                        productId: id,
                        quantity: q,
                        color: colorId,
                        size: sizeId
                    },
                    success: function () {
                        common.notify('Update quantity is successful', 'success');
                        loadHeaderCart();
                        loadData();
                    }
                });
            } else {
                common.notify('Your quantity is invalid', 'error');
            }

        });
        $('#btnClearAll').on('click', function (e) {
            e.preventDefault();
            $.ajax({
                url: '/Cart/ClearCart',
                type: 'post',
                success: function () {
                    common.notify('Clear cart is successful', 'success');
                    loadHeaderCart();
                    loadData();
                }
            });
        });
    }

    function loadColors() {
        return $.ajax({
            type: "GET",
            url: "/Cart/GetColors",
            dataType: "json",
            success: function (response) {
                cachedObj.colors = response;
            },
            error: function () {
                common.notify('Has an error in progress', 'error');
            }
        });
    }

    function loadSizes() {
        return $.ajax({
            type: "GET",
            url: "/Cart/GetSizes",
            dataType: "json",
            success: function (response) {
                cachedObj.sizes = response;
            },
            error: function () {
                common.notify('Has an error in progress', 'error');
            }
        });
    }
    // get colors đẩy vào dropdown list
    function getColorOptions(selectedId) {
       // var colors = "<select class='form-control ddlColorId'><option value='0'></option>";
        var colors = "<select class='form-control ddlColorId'>";
        $.each(cachedObj.colors, function (i, color) {
            if (selectedId === color.Id)
                colors += '<option value="' + color.Id + '" selected="select">' + color.Name + '</option>';
            else
                colors += '<option value="' + color.Id + '">' + color.Name + '</option>';
        });
        colors += "</select>";
        return colors;
    }
    // get size đẩy vào dropdown list
    function getSizeOptions(selectedId) {
       // var sizes = "<select class='form-control ddlSizeId'> <option value='0'></option>";
        var sizes = "<select class='form-control ddlSizeId'>";
        $.each(cachedObj.sizes, function (i, size) {
            if (selectedId === size.Id)
                sizes += '<option value="' + size.Id + '" selected="select">' + size.Name + '</option>';
            else
                sizes += '<option value="' + size.Id + '">' + size.Name + '</option>';
        });
        sizes += "</select>";
        return sizes;
    }

    function loadHeaderCart() {
        $("#headerCart").load("/AjaxContent/HeaderCart");// load lại view component để cập nhật danh sách sp add vào giỏ hàng
    }

    function loadData() {
        $.ajax({
            url: '/Cart/GetCart',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                var template = $('#template-cart').html();
                var render = "";
                var totalAmount = 0;
                $.each(response, function (i, item) {
                    render += Mustache.render(template,
                        {
                            ProductId: item.Product.Id,
                            ProductName: item.Product.Name,
                            Image: item.Product.Image,
                            Price: common.formatNumber(item.Price, 0),
                            Quantity: item.Quantity,
                            Colors: getColorOptions(item.Color == null ? 0 : item.Color.Id),
                            Sizes: getSizeOptions(item.Size == null ? "" : item.Size.Id),
                            Amount: common.formatNumber(item.Price * item.Quantity, 0),
                            Url: '/' + item.Product.SeoAlias + "-p." + item.Product.Id + ".html"
                        });
                    totalAmount += item.Price * item.Quantity;
                });

                $('#lblTotalAmount').text(common.formatNumber(totalAmount, 0));

                if (render !== "")
                    $('#table-cart-content').html(render);
                else
                    $('#table-cart-content').html('You have no product in cart');

            }
        });
        return false;
    }
}