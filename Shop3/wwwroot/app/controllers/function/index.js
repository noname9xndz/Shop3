


var FunctionController = function() {

    this.initialize = function() {
        loadData();
        registerEvents();
    }

    function registerEvents() {

        $('#ddlShowPage').on('change',
            function() {
                common.configs.pageSize = $(this).val();
                common.configs.pageIndex = 1;
                loadData();
            });

    };

    function loadData() {
        $.ajax({
                type: "GET",
                url: "/admin/Function/GetAll",
                dataType: "json",
                beforeSend: function() {
                    common.startLoading();
                },
                success: function(response) {

                    var template = $('#function-table-template').html();
                    var render = "";

                    //if (response.RowCount > 0) {

                        $.each(response, function(i, item) {
                                render += Mustache.render(template,
                                    {
                                        Id: item.Id,
                                        Name: item.Name,
                                        URL: item.URL,
                                        IconCss: item.IconCss,
                                        Status: common.getStatus(item.Status)
                                    });
                            });
                        $('#lblTotalRecords').text(response.RowCount); // tổng số bản ghi
                        if (render != undefined) {
                            $('#functionList').html(render);
                            wrapPaging(response.RowCount,
                                function() {
                                    loadData();
                                },
                                isPageChanged);
                        }
                    //} else {
                    //    $('#functionList').html('');
                    //}
                    common.stopLoading();
                },
                error: function(status) {
                    console.log(status);
                }

            });

        
    };

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
            onPageClick: function(event, p) {
                common.configs.pageIndex = p;
                setTimeout(callBack(), 200);
            }
        });
    }


}

    