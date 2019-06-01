


var AnnouncementJSController = function () {

    this.initialize = function () {
        loadAnnouncement();
        loadAnnouncementTable(true);
        registerEvents();
    }

    function registerEvents() {

        // get thông báo theo id của user
        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/Admin/Role/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    common.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#hidId').val(data.Id);
                    $('#txtName').val(data.Name);
                    $('#txtDescription').val(data.Description);
                    $('#modal-add-edit').modal('show');
                    common.stopLoading();

                },
                error: function (status) {
                    common.notify('Có lỗi xảy ra', 'error');
                    common.stopLoading();
                }
            });
        });

        $('#ddlShowPage').on('change', function () {
            common.configs.pageSize = $(this).val();
            common.configs.pageIndex = 1;
            loadAnnouncementTable(true);
        });
        $('body').on('click', '#seeall,#btnViewAll', function (e) {
            e.preventDefault();
            $.ajax({
                type: "POST",
                url: "/Admin/Announcement/MarkAsReadAll",
                dataType: "json",
                beforeSend: function () {
                    common.startLoading();
                },
                success: function (response) {
                    loadAnnouncement();
                    loadAnnouncementTable(true);
                    common.stopLoading();

                },
                error: function (status) {
                    common.notify('Có lỗi xảy ra', 'error');
                    common.stopLoading();
                }
            });
        });
        $('body').on('click', '#btnView', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "POST",
                url: "/Admin/Announcement/MarkAsRead",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    common.startLoading();
                },
                success: function (response) {
                    loadAnnouncementTable(true);
                    loadAnnouncement();
                    common.stopLoading();

                },
                error: function (status) {
                    common.notify('Có lỗi xảy ra', 'error');
                    common.stopLoading();
                }
            });
        });
        $('body').on('click', '#btnDelete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "POST",
                url: "/Admin/Announcement/Delete",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    common.startLoading();
                },
                success: function (response) {

                    common.notify('Delete successful', 'success');
                    common.stopLoading();
                    loadAnnouncementTable(true);
                    loadAnnouncement();

                },
                error: function (status) {
                    common.notify('Has an error in delete progress', 'error');
                    common.stopLoading();
                }
            });
        });

        $('body').on('click', '#btnDeleteSeen', function (e) {
            e.preventDefault();
            var key = "Seen";
            deleteAll(key);
        });
        $('body').on('click', '#btnDeleteUnRead', function (e) {
            e.preventDefault();
            var key = "UnRead";
            deleteAll(key);
        });
        $('body').on('click', '#btnDeleteAll', function (e) {
            e.preventDefault();
            var key = " ";
            deleteAll(key);
        });

    };

    function loadAnnouncementTable(isPageChanged) {

        if ($("body").find("#announcement-table-template").length > 0) {

            $.ajax({
                type: "GET",
                url: "/admin/announcement/GetAllAnnouncementOfUserPaging",
                data: {
                    page: common.configs.pageIndex,
                    pageSize: common.configs.pageSize
                },
                dataType: "json",
                beforeSend: function() {
                    common.startLoading();
                },
                success: function(response) {

                    var template = $('#announcement-table-template').html();
                    var render = "";

                    if (response.RowCount > 0) {

                        $.each(response.Results,
                            function(i, item) {
                                render += Mustache.render(template,
                                    {
                                        Content: item.Content,
                                        Id: item.Id,
                                        DateCreated: common.dateTimeFormatJson(item.DateCreated),
                                        Title: item.Title,
                                        FullName: item.FullName,
                                        Status: common.getStatus2(item.Status)
                                    });
                            });
                        $('#lblTotalRecords').text(response.RowCount); // tổng số bản ghi
                        if (render != undefined) {
                            $('#annoncementAllList').html(render);
                            wrapPaging(response.RowCount,
                                function() {
                                    loadAnnouncement();
                                },
                                isPageChanged);
                        }
                    } else {
                        $('#annoncementAllList').html('');
                    }
                    common.stopLoading();
                },
                error: function(status) {
                    console.log(status);
                }

            });

        } else {

        }
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
            onPageClick: function (event, p) {
                common.configs.pageIndex = p;
                setTimeout(callBack(), 200);
            }
        });
    }

    function loadAnnouncement() {
        $.ajax({
            type: "GET",
            url: "/admin/announcement/GetAllPaging",
            data: {
                page: common.configs.pageIndex,
                pageSize: common.configs.pageSize
            },
            dataType: "json",
            beforeSend: function () {
                common.startLoading();
            },
            success: function (response) {

                var template = $('#announcement-template').html();
                var render = "";

                if (response.RowCount > 0) {
                    $('#announcementArea').show();
                    $.each(response.Results, function (i, item) {
                        render += Mustache.render(template, {
                            Content: item.Content,
                            Id: item.Id,
                            Title: item.Title,
                            FullName: item.FullName,
                            Avatar: item.Avatar == null ? '<img src="../../admin-side/images/users.png"  width="30" height ="30"/>' : '<img src="' + item.Avatar + '" width="30" height ="30" />'
                        });
                    });
                    render += $('#announcement-tag-template').html();
                    $("#totalAnnouncement").text(response.RowCount);
                    if (render != undefined) {
                        $('#annoncementList').html(render);
                    }
                }
                else {
                    $('#announcementArea').hide();
                    $('#annoncementList').html('');
                }
                common.stopLoading();
            },
            error: function (status) {
                console.log(status);
            }

        });
    };

    function deleteAll(key) {
        common.confirm('Are you sure to delete?', function () {
            $.ajax({
                type: "POST",
                url: "/Admin/Announcement/DeleteAll",
                data: { key: key },
                dataType: "json",
                beforeSend: function () {
                    common.startLoading();
                },
                success: function (response) {

                    common.notify('Delete successful', 'success');
                    common.stopLoading();
                    loadAnnouncementTable(true);
                    loadAnnouncement();

                },
                error: function (status) {
                    common.notify('Has an error in delete progress', 'error');
                    common.stopLoading();
                }
            });
        });
    }
}