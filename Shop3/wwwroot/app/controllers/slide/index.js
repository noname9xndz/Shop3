


var SlideController = function () {

    this.initialize = function () {
        loadAllSlide();
        registerEvents();
    }

    function registerEvents() {

        $("#ddl-show-page").on('change', function () {
            common.configs.pageSize = $(this).val();
            common.configs.pageIndex = 1;
            loadAllSlide(true);
        });

        $("#btnCreate").on('click', function () {
            resetForm();
            $('#addEditSlideModalByAlias').modal('show');

        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var alias = $(this).data('id');
            loadSlideByAlias(alias);
            $('#hidGroupAlias').val(alias);///
        });
        $('body').on('click', '#editsilde', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            loadSlideById(id);

        });

        $('body').on('click', '#removesilde', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            var hiddenGroupAlias = $('#hidGroupAlias').val();///
            deleteSlide(that, hiddenGroupAlias);
        });
        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var alias = $(this).data('id');
            deleteSlideByAlias(alias);

            $('#hidGroupAlias').val(alias);
            
        });

        $('#btnSave').on('click', function (e) {
            saveSlide(e);
        });

        $('#btnSelectImg').on('click', function () {
            $('#fileInputImage').click();
        });

        $("#fileInputImage").on('change', function () {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;
            var data = new FormData();
            fileInputImage(files, data);
        });

    };

    function loadAllSlide(isPageChanged) {
        var key = '';
        $.ajax({
            type: "GET",
            url: "/admin/Slide/GetAllSlidePagingByAlias",
            data: {
                //categoryId: $('#ddlCategorySearch').val(),
                //keyword: $('#txtKeyword').val(),
                keyword: key,
                page: common.configs.pageIndex,
                pageSize: common.configs.pageSize
            },
            dataType: "json",
            beforeSend: function () {
                common.startLoading();
            },
            success: function (response) {
                var template = $('#table-template').html();
                var render = "";
                if (response.RowCount > 0) {
                    $.each(response.Results, function (i, item) {
                        render += Mustache.render(template, {
                            GroupAlias: item.GroupAlias,
                            TotalSlide: item.TotalSlide
                        });
                    });
                    $("#lbl-total-records").text(response.RowCount);
                    if (render != undefined) {
                        $('#SlideTable').html(render);

                    }
                    wrapPaging(response.RowCount, function () {
                        loadAllSlide();
                    }, isPageChanged);


                }
                else {
                    $('#SlideTable').html('');
                }
            },
            error: function (status) {
                console.log(status);
            }

        });
    };

    function wrapPaging(recordCount, callBack, changePageSize) {
        var totalsize = Math.ceil(recordCount / common.configs.pageSize);
        if ($('#paginationUL a').length === 0 || changePageSize === true) {
            $('#paginationUL').empty();
            $('#paginationUL').removeData("twbs-pagination");
            $('#paginationUL').unbind("page");
        }
        $('#paginationUL').twbsPagination({
            totalPages: totalsize,
            visiblePages: 7,
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
    function loadSlideByAlias(alias) {
        $.ajax({
            type: "GET",
            url: "/admin/Slide/GetAllSlideByAlias",
            data: {
                groupAlias: alias
            },
            dataType: "json",
            beforeSend: function () {
                common.startLoading();
            },
            success: function (response) {
                var template = $('#slide-template').html();
                var render = "";
                $.each(response, function (i, item) {
                    render += Mustache.render(template, {
                        Id: item.Id,
                        Name: item.Name,
                        Image: item.Image
                    });
                });
                $('#slideM').html(render);
                $('#modal-add-edit').modal('show');
            },
            error: function (status) {
                console.log(status);
            }

        });
    };

    function loadSlideById(id) {
        $.ajax({
            type: "GET",
            url: "/admin/Slide/GetSlideById",
            data: {
                id: id
            },
            dataType: "json",
            beforeSend: function () {
                common.startLoading();
            },
            success: function (response) {

                var data = response;
                $('#hidIdM').val(data.Id);
                $('#txtName').val(data.Name);
                $('#txtGroupAlias').val(data.GroupAlias);
                $('#hidGroupAlias').val(data.GroupAlias);
                $('#txtDesc').val(data.Description);
                $('#txtContent').val(data.Content);
                $('#txtImage').val(data.Image);
                $('#txtUrl').val(data.Url);
                $('#txtDisplayOrder').val(data.DisplayOrder);
                $('#ckStatus').prop('checked', data.Status == 1);

                $('#addEditSlideModalByAlias').modal('show');
            },
            error: function (status) {
                console.log(status);
            }

        });
    };

    function fileInputImage(files, data) {

        for (var i = 0; i < files.length; i++) {
            data.append(files[i].name, files[i]);
        }
        $.ajax({
            type: "POST",
            url: "/Admin/Upload/UploadImage",
            contentType: false,
            processData: false,
            data: data,
            success: function (path) {
                $('#txtImage').val(path);
                common.notify('Upload image succesful!', 'success');

            },
            error: function () {
                common.notify('There was error uploading files!', 'error');
            }
        });
    }


    function resetForm() {
        $('#Id').val(0);
        $('#txtGroupAlias').val('');
        $('#hidGroupAlias').val(0);
        $('#txtName').val('');
        $('#txtDesc').val('');
        $('#txtContent').val('');
        $('#txtImage').val('');
        $('#txtUrl').val('');
        $('#txtDisplayOrder').val('');
        $('#ckStatus').prop('checked', true);

    }


    function saveSlide(e) {

        e.preventDefault();

        var hiddenGroupAlias = $('#hidGroupAlias').val();
        var id = $('#hidIdM').val();
        var name = $('#txtName').val();
        var groupAlias = $('#txtGroupAlias').val();
        var description = $('#txtDesc').val();
        var content = $('#txtContent').val();
        var image = $('#txtImage').val();
        var url = $('#txtUrl').val();
        var displayOrder = $('#txtDisplayOrder').val();
        var status = $('#ckStatus').prop('checked') == true ? true : false;

        $.ajax({
            type: "POST",
            url: "/Admin/Slide/SaveSlide",
            data: {
                Id: id,
                Name: name,
                GroupAlias: groupAlias,
                Description: description,
                Content: content,
                Image: image,
                Url: url,
                DisplayOrder: displayOrder,
                Status: status

            },
            dataType: "json",
            beforeSend: function () {
                common.startLoading();
            },
            success: function (response) {
                common.notify('Update slide successful', 'success');
                $('#addEditSlideModalByAlias').modal('hide');

                common.stopLoading();
                loadAllSlide();
                if (hiddenGroupAlias != 0) {
                    loadSlideByAlias(hiddenGroupAlias);
                } else {
                    loadSlideByAlias(groupAlias);
                }
                resetForm();

            },
            error: function () {
                common.notify('Has an error in save slide progress', 'error');
                common.stopLoading();
            }
        });


    }

    function deleteSlide(id, hiddenGroupAlias) {
        common.confirm('Are you sure to delete?', function () {
            $.ajax({
                type: "POST",
                url: "/Admin/Slide/DeleteSlideById",
                data: { id: id },
                dataType: "json",
                beforeSend: function () {
                    common.startLoading();
                },
                success: function (response) {
                    common.notify('Delete successful', 'success');
                    if (hiddenGroupAlias != 0) {
                        loadSlideByAlias(hiddenGroupAlias);
                    }
                    else if (groupAlias != 0) {
                        loadSlideByAlias(groupAlias);
                    }
                    loadAllSlide();
                    common.stopLoading();
                },
                error: function (status) {
                    common.notify('Has an error in delete progress', 'error');
                    common.stopLoading();
                }
            });
        });
    }

    function deleteSlideByAlias(alias) {
        common.confirm('Are you sure to delete?', function () {
            $.ajax({
                type: "POST",
                url: "/Admin/Slide/DeleteSlideByAlias",
                data: { alias: alias },
                dataType: "json",
                beforeSend: function () {
                    common.startLoading();
                },
                success: function (response) {
                    common.notify('Delete successful', 'success');
                    common.stopLoading();
                    loadAllSlide();
                },
                error: function (status) {
                    common.notify('Has an error in delete progress', 'error');
                    common.stopLoading();
                }
            });
        });
    }

}