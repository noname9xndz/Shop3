


//var AnnouncementController = function () {

//    this.initialize = function () {
//        loadAnnouncement();
//        registerEvents();
//    }

//    function registerEvents() {
//        // get thông báo theo id của user
//        $('body').on('click', '.btn-edit', function (e) {
//            e.preventDefault();
//            var that = $(this).data('id');
//            $.ajax({
//                type: "GET",
//                url: "/Admin/Role/GetById",
//                data: { id: that },
//                dataType: "json",
//                beforeSend: function () {
//                    common.startLoading();
//                },
//                success: function (response) {
//                    var data = response;
//                    $('#hidId').val(data.Id);
//                    $('#txtName').val(data.Name);
//                    $('#txtDescription').val(data.Description);
//                    $('#modal-add-edit').modal('show');
//                    common.stopLoading();

//                },
//                error: function (status) {
//                    common.notify('Có lỗi xảy ra', 'error');
//                    common.stopLoading();
//                }
//            });
//        });
//        $('body').on('click', '#seeall', function (e) {
//            e.preventDefault();
//            $.ajax({
//                type: "POST",
//                url: "/Admin/Announcement/MarkAsReadAll",
//                dataType: "json",
//                beforeSend: function () {
//                    common.startLoading();
//                },
//                success: function (response) {
//                    loadAnnouncement();
//                    common.stopLoading();

//                },
//                error: function (status) {
//                    common.notify('Có lỗi xảy ra', 'error');
//                    common.stopLoading();
//                }
//            });
//        });


//    };

//    function loadAnnouncement() {
//        $.ajax({
//            type: "GET",
//            url: "/admin/announcement/GetAllPaging",
//            data: {
//                page: common.configs.pageIndex,
//                pageSize: common.configs.pageSize
//            },
//            dataType: "json",
//            beforeSend: function () {
//                common.startLoading();
//            },
//            success: function (response) {

//                var template = $('#announcement-template').html();
//                var render = "";

//                if (response.RowCount > 0)
//                {
//                    $('#announcementArea').show();
//                    $.each(response.Results, function (i, item) {
//                        render += Mustache.render(template, {
//                            Content: item.Content,
//                            Id: item.Id,
//                            Title: item.Title,
//                            FullName: item.FullName,
//                            Avatar: item.Avatar == null ? '<img src="../../admin-side/images/users.png"  width="30" height ="30"/>' : '<img src="' + item.Avatar + '" width="30" height ="30" />'
//                        });
//                    });
//                    render += $('#announcement-tag-template').html();
//                    $("#totalAnnouncement").text(response.RowCount);
//                    if (render != undefined) {
//                        $('#annoncementList').html(render);
//                    }
//                }
//                else {
//                    $('#announcementArea').hide();
//                    $('#annoncementList').html('');
//                }
//                common.stopLoading();
//            },
//            error: function (status) {
//                console.log(status);
//            }

//        });
//    };


//}