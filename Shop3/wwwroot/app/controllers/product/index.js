

var productController = function () {
    this.initialize = function () {
        loadCategories();
        loadData();
        registerEvents();
    }
    function registerEvents() {
        // load phân trang
        $('#ddlShowPage').on('change', function () {
            common.configs.pageSize = $(this).val();
            common.configs.pageIndex = 1;
            loadData(true);
        });

        $('#btnSearch').on('click', function () {
            loadData();
        });

        $('#txtKeyword').on('keypress', function (e) {
            if (e.which === 13) { // 13 : phím enter
                loadData();
            }
        });
       
      

    }
    // lấy và đẩy dữ liệu product ra view dùng mustache : https://github.com/janl/mustache.js
    function loadData(isPageChanged) {

        var template = $('#table-product-template').html();
            var render = "";

        $.ajax({
            type: 'GET',
            data: {
                categoryId:  $('#ddlCategorySearch').val(),
                keyword: $('#txtKeyword').val(),
                page: common.configs.pageIndex,
                pageSize: common.configs.pageSize
            },
            url: '/admin/product/GetAllPaging',
            dataType: 'json',
            success: function (response) {

                $.each(response.Results, function (i, item) { // response.Results lấy ra danh sách 
                    render += Mustache.render(template, {
                        Id: item.Id,
                        Name: item.Name,
                        Image: item.Image == null ? '<img src="../admin-side/images/user.png"  width="25"/>' : '<img src="' + item.Image + '" width=25 />',
                        CategoryName: item.ProductCategory.Name,
                        Price: common.formatNumber(item.Price, 0),
                        CreatedDate: common.dateTimeFormatJson(item.DateCreated),
                        Status: common.getStatus(item.Status)
                    });

                    $('#lblTotalRecords').text(response.RowCount); // tổng số bản ghi

                    if (render != '') {
                        $("#tbl-product").html(render);
                    }

                    wrapPaging(response.RowCount, function () {
                        loadData();
                    }, isPageChanged);

                });
            },
            error: function (status) {  
                console.log(status);
                common.notify('Cannot loading data', 'error');
            }
        });
    }

    // phân trang cho product dùng plugin : twbs-pagination xem thêm : https://github.com/josecebe/twbs-pagination
    function wrapPaging(recordCount, callBack, changePageSize) { // changePageSize : load ra phân trang hay đổi trang
        var totalsize = Math.ceil(recordCount / common.configs.pageSize);
        //Unbind pagination if it existed or click change pagesize
        if ($('#paginationUL a').length === 0 || changePageSize === true) {
            $('#paginationUL').empty();
            $('#paginationUL').removeData("twbs-pagination");
            $('#paginationUL').unbind("page");
        }
        //Bind Pagination Event
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

    // load danh sách các category
    function loadCategories() {
        $.ajax({
            type: 'GET',
            url: '/admin/product/GetAllCategories',
            dataType: 'json',
            success: function (response) {
                var render = "<option value=''>--Select category--</option>";
                $.each(response, function (i, item) {
                    render += "<option value='" + item.Id + "'>" + item.Name + "</option>"
                });
                $('#ddlCategorySearch').html(render);
            },
            error: function (status) {
                console.log(status);
                common.notify('Cannot loading product category data', 'error');
            }
        });
    }


}
