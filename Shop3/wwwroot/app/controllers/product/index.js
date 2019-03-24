

var productController = function () {
    this.initialize = function () {
        loadData();
    }
    function registerEvents() {

    }
    function loadData() {

        var template = $('#table-product-template').html();
            var render = "";

        $.ajax({
            type: 'GET',
            url: '/admin/product/GetAll',
            dataType: 'json',
            success: function (response) {
                $.each(response, function (i, item) {
                    render += Mustache.render(template, {
                        Id: item.Id,
                        Name: item.Name,
                        Image: item.Image == null ? '<img src="../admin-side/images/user.png"  width="25"/>' : '<img src="' + item.Image + '" width=25 />',
                        CategoryName: item.ProductCategory.Name,
                        Price: common.formatNumber(item.Price, 0),
                        CreatedDate: common.dateTimeFormatJson(item.DateCreated),
                        Status: common.getStatus(item.Status)
                    });
                    if (render != '') {
                        $("#tbl-product").html(render);
                    }
                });
            },
            error: function (status) {
                console.log(status);
                tedu.notify('Cannot loading data', 'error');
            }
        })
    }
}
