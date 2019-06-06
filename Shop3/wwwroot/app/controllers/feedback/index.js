


var FeedBackController = function() {

    this.initialize = function() {
        loadFeedBack();
        registerEvents();
        //registerControls();
    }

    function registerEvents() {
        // load phân trang
        $('#ddlShowPage').on('change', function () {
            common.configs.pageSize = $(this).val();
            common.configs.pageIndex = 1;
            loadFeedBack(true);
        });

        $('#btnSearch').on('click', function () {
            loadFeedBack();
        });

        $('#txtKeyword').on('keypress', function (e) {
            if (e.which === 13) { // 13 : phím enter
                loadFeedBack();
            }
        });
        
        $('body').on('click', '#mail', function (e) {
            e.preventDefault();
            $(".mail_view").removeAttr("hidden");
            //$('#mail_list_column').addClass('col-sm-3')
            //     .removeClass('.col-sm-12');
            var id = $(this).data('id');
            loadFeedBackDetail(id);
        }); 
        $('body').on('click', '#btnDeleteFb', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            deleteFeedBack(id);
        });

    };

    function loadFeedBack(isPageChanged) {

        var template = $('#table-template').html();
        var render = "";

        $.ajax({
            type: 'GET',
            data: {
                keyword: $('#txtKeyword').val(),
                page: common.configs.pageIndex,
                pageSize: common.configs.pageSize
            },
            url: '/admin/FeedBack/GetAll',
            dataType: 'json',
            success: function (response) {
                if (response.status == "error") {
                    common.notify(response.message, 'error');
                    $(".mail_list_column,.mail_view,.dataTables_info,.pagination").attr("hidden", true);
                    $("#btnCreate").removeAttr("hidden"); 
                }
                else{
                    //console.log(response);
                    $.each(response.Results, function (i, item) {
                        render += Mustache.render(template, {
                            Id: item.Id,
                            Name: item.Name,
                            Message: item.Message,
                            DateCreated: common.dateTimeFormatJson(item.DateCreated),
                            Status: item.Status
                        });
                    });

                    $('#lblTotalRecords').text(response.RowCount);

                    if (render != '') {
                        $("#mail-table").html(render);
                    }

                    wrapPaging(response.RowCount, function () {
                        loadFeedBack();
                    }, isPageChanged);

                }
               

            },
            error: function (status) {
                console.log(status);
                common.notify('Cannot loading data', 'error');
            }
        });
    }

    function loadFeedBackDetail(id) {

        var template = $('#mailDetail-template').html();
        var render = "";

        $.ajax({
            type: 'GET',
            data: {
                id : id
            },
            url: '/admin/FeedBack/GetById',
            dataType: 'json',
            success: function (response) {
                var item = response
                    render += Mustache.render(template, {
                        Id: item.Id,
                        Name: item.Name,
                        Message: item.Message,
                        Email : item.Email,
                        DateCreated: common.dateTimeFormatJson(item.DateCreated),
                        Status: item.Status
                });
                if (render != '') {
                    $("#mailDetail-table").html(render);
                }
                loadFeedBack();

            },
            error: function (status) {
                console.log(status);
                common.notify('Cannot loading data', 'error');
            }
        });
    }

    
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

  
    function deleteFeedBack(id) {
        common.confirm('Are you sure to delete?', function () {
            $.ajax({
                type: "POST",
                url: "/Admin/FeedBack/Delete",
                data: { id: id },
                dataType: "json",
                beforeSend: function () {
                    common.startLoading();
                },
                success: function (response) {
                    common.notify('Delete successful', 'success');
                    common.stopLoading();
                    loadFeedBack();
                },
                error: function (status) {
                    common.notify('Has an error in delete progress', 'error');
                    common.stopLoading();
                }
            });
        });
    }

}