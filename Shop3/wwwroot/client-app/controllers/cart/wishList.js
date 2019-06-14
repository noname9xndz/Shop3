



var wishProductController = function () {

    //var  billDetail = new billDetailController();
   // var  bill = new billController();

    this.initialize = function () {
        loadWishList();
        registerEvents();
       // billDetail.initialize();
        //bill.initialize();
    }

    function registerEvents() {
        $('#ddlShowPage').on('change',
            function() {
                common.configs.pageSize = $(this).val();
                common.configs.pageIndex = 1;
                loadWishList(true);
            });

        $('body').on('click','#removeWishProduct',function(e) {
                e.preventDefault();
                var that = $(this).data('id');
                removeWishProduct(that);
                loadWishList(true);
        });

        $('body').on('click', '.all-cart', function (e) {
            e.preventDefault();
            addAllWishProductToCart();
           
        });
        

       
    }

    function loadWishList(isPageChanged)  {

            var template = $('#table-template').html();
            var render = "";

            $.ajax({
                type: 'POST',
                data: {
                    page: common.configs.pageIndex,
                    pageSize: common.configs.pageSize
                },
                url: '/Cart/GetAllWishListPaging',
                dataType: 'json',
                success: function (response) {
                    $.each(response.Results, function (i, item) { 
                        render += Mustache.render(template, {
                            Id: item.Id,
                            Name: item.Name,
                            Image: item.Image == null ? '<img src="../admin-side/images/user.png"  width="25"/>' : '<img src="' + item.Image + '" width=25 />',
                            Price: item.PromotionPrice < item.Price && item.PromotionPrice != null 
                                ?  common.formatNumber(item.PromotionPrice, 0)
                                : common.formatNumber(item.Price, 0),
                            Link : '/' + item.SeoAlias + '-p.' + item.Id + '.html'
                        });
                    });

                    $('#lblTotalRecords').text(response.RowCount); // tổng số bản ghi
                    if (render != '') {
                        $("#table-wishlist").html(render);
                    }

                    wrapPaging(response.RowCount, function () {
                        loadWishList();
                    }, isPageChanged);


                },
                error: function (status) {
                    console.log(status);
                    common.notify('Cannot loading data', 'error');
                }
            });
        }
    
    function wrapPaging(recordCount, callBack, changePageSize) { // changePageSize : load ra phân trang hay đổi trang
        var totalsize = Math.ceil(recordCount / common.configs.pageSize);
        if ($('#paginationUL a').length === 0 || changePageSize === true) {
            $('#paginationUL').empty();
            $('#paginationUL').removeData("twbs-pagination");
            $('#paginationUL').unbind("page");
        }
        if (recordCount > 10) {
            $('#paginationUL').twbsPagination({
                totalPages: totalsize,
                visiblePages: 7, //max page hiển thị
                first: 'Đầu',
                prev: 'Trước',
                next: 'Tiếp',
                last: 'Cuối',
                onPageClick: function (event, p) {
                    common.configs.pageIndex = p;
                    setTimeout(callBack(), 200);
                }
            });
        }
        
    }

    function removeWishProduct(id)
    {
        $.ajax({
                type: "POST",
                url: "/Cart/RemovewishList",
                data: { productId : id },
                dataType: "json",
                beforeSend: function () {
                    common.startLoading();
                },
                success: function (response) {
                    common.notify('Delete successful', 'success');

                    common.stopLoading();
                    loadWishList();
                },
                error: function (status) {
                    common.notify('Has an error in delete progress', 'error');
                    common.stopLoading();
                }
            });
       
    }

    function loadHeaderCart() {
        $("#headerCart").load("/AjaxContent/HeaderCart"); // load lại view component để cập nhật danh sách sp add vào giỏ hàng
    }

    function addAllWishProductToCart() {
        $.ajax({
            type: "POST",
            url: "/Cart/addAllWishProductToCart",
            dataType: "json",
            success: function (response) {
                common.notify('add All Wish Product To Cart successful', 'success');
                loadHeaderCart();
            },
            error: function (status) {
                console.log(status);
                common.notify('Has an error in add progress', 'error');
               // common.stopLoading();
            }
        });

    }
    
   
}