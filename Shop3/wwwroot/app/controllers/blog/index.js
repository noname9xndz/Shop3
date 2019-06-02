


var BlogController = function () {

    this.initialize = function () {
        // loadBlog();
        loadBlogTable();
        registerEvents();
        registerControls();
    }

    function registerEvents() {
        // load phân trang
        $('#ddlShowPage').on('change', function () {
            common.configs.pageSize = $(this).val();
            common.configs.pageIndex = 1;
            loadBlogTable(true);
        });
        $("#btnCreate").on('click', function () {
            resetForm();
            $('#modal-add-edit').modal('show');

        });


        $('#btnSelectImg').on('click', function () {
            $('#fileInputImageM').click();
        });

        $("#fileInputImageM").on('change', function () {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;
            var data = new FormData();
            fileInputImage(files, data);
        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            editBlog(that);
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            deleteBlog(that);
        });

        $('#btnSave').on('click', function (e) {
            saveBlog(e);
        });
        
    };



    function loadBlogTable(isPageChanged) {

        var template = $('#table-template').html();
        var render = "";
        var key = ""
        $.ajax({
            type: 'GET',
            data: {
                //categoryId: $('#ddlCategorySearch').val(),
                //keyword: $('#txtKeyword').val(),
                keyword: key,
                page: common.configs.pageIndex,
                pageSize: common.configs.pageSize
            },
            url: '/admin/Blog/GetAllPaging',
            dataType: 'json',
            beforeSend: function () {
                common.startLoading();
            },
            success: function (response) {
                $.each(response.Results, function (i, item) { // response.Results lấy ra danh sách 
                    render += Mustache.render(template, {
                        Id: item.Id,
                        Name: item.Name,
                        Image: item.Image == null ? '<img src="../admin-side/images/users.png"  width="25"/>' : '<img src="' + item.Image + '" width=25 />',
                        Description: item.Name,
                        CreatedDate: common.dateTimeFormatJson(item.DateCreated),
                        DateModified: common.dateTimeFormatJson(item.DateModified),
                        Status: common.getStatus(item.Status)
                    });
                });

                $('#lblTotalRecords').text(response.RowCount); // tổng số bản ghi

                if (render != '') {
                    $("#BlogTable").html(render);
                }

                //wrapPaging(response.RowCount, function () {
                //    loadBlog();
                //}, isPageChanged);


            },
            error: function (status) {
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


    function registerControls() {
        CKEDITOR.replace('txtContentM', {});

        //Fix: cannot click on element ck in modal
        $.fn.modal.Constructor.prototype.enforceFocus = function () {
            $(document)
                .off('focusin.bs.modal') // guard against infinite focus loop
                .on('focusin.bs.modal', $.proxy(function (e) {
                    if (
                        this.$element[0] !== e.target && !this.$element.has(e.target).length
                        // CKEditor compatibility fix start.
                        && !$(e.target).closest('.cke_dialog, .cke').length
                        // CKEditor compatibility fix end.
                    ) {
                        this.$element.trigger('focus');
                    }
                }, this));
        };

    }


    function resetForm() {
        $('#hidIdM').val(0);
        $('#txtNameM').val('');
        $('#txtDescM').val('');
        $('#txtImageM').val('');
        $('#txtTagM').val('');
        $('#txtMetakeywordM').val('');
        $('#txtMetaDescriptionM').val('');
        $('#txtSeoPageTitleM').val('');
        $('#txtSeoAliasM').val('');

        CKEDITOR.instances.txtContentM.setData('');
        $('#ckStatusM').prop('checked', true);
        $('#ckHotM').prop('checked', false);
        $('#ckShowHomeM').prop('checked', false);

    }


    function editBlog(id) {
        $.ajax({
            type: "GET",
            url: "/Admin/Blog/GetById",
            data: { id: id },
            dataType: "json",
            beforeSend: function () {
                common.startLoading();
            },
            success: function (response) {
                var data = response;
                $('#hidIdM').val(data.Id);
                $('#txtNameM').val(data.Name);
                $('#txtDescM').val(data.Description);
                $('#txtImageM').val(data.Image);
                $('#txtTagM').val(data.Tags);
                $('#txtMetakeywordM').val(data.SeoKeywords);
                $('#txtMetaDescriptionM').val(data.SeoDescription);
                $('#txtSeoPageTitleM').val(data.SeoPageTitle);
                $('#txtSeoAliasM').val(data.SeoAlias);

                CKEDITOR.instances.txtContentM.setData(data.Content);
                $('#ckStatusM').prop('checked', data.Status == 1);
                $('#ckHotM').prop('checked', data.HotFlag);
                $('#ckShowHomeM').prop('checked', data.HomeFlag);

                $('#modal-add-edit').modal('show');
                $('#modal-view').modal('show');

            },
            error: function (status) {
                common.notify('Có lỗi xảy ra', 'error');
                common.stopLoading();
            }
        });
    }

   
    function saveBlog(e) {
        if ($('#frmMaintainance').valid()) {
            e.preventDefault();
            var id = $('#hidIdM').val();
            var name = $('#txtNameM').val();
            var description = $('#txtDescM').val();
            var image = $('#txtImageM').val();
            var tags = $('#txtTagM').val();
            var seoKeyword = $('#txtMetakeywordM').val();
            var seoMetaDescription = $('#txtMetaDescriptionM').val();
            var seoPageTitle = $('#txtSeoPageTitleM').val();
            var seoAlias = $('#txtSeoAliasM').val();

            var content = CKEDITOR.instances.txtContentM.getData();
            var status = $('#ckStatusM').prop('checked') == true ? 1 : 0;
            var hot = $('#ckHotM').prop('checked');
            var showHome = $('#ckShowHomeM').prop('checked');

            $.ajax({
                type: "POST",
                url: "/Admin/Blog/SaveEntity",
                data: {
                    Id: id,
                    Name: name,
                    Image: image,
                    Description: description,
                    Content: content,
                    HomeFlag: showHome,
                    HotFlag: hot,
                    Tags: tags,
                    Status: status,
                    SeoPageTitle: seoPageTitle,
                    SeoAlias: seoAlias,
                    SeoKeywords: seoKeyword,
                    SeoDescription: seoMetaDescription
                },
                dataType: "json",
                beforeSend: function () {
                    common.startLoading();
                },
                success: function (response) {
                    common.notify('Update blog successful', 'success');
                    $('#modal-add-edit').modal('hide');
                    resetForm();
                    common.stopLoading();
                    loadBlogTable(true);
                },
                error: function () {
                    common.notify('Has an error in save blog progress', 'error');
                    common.stopLoading();
                }
            });
            return false;
        }

    }


    function deleteBlog(id) {
        common.confirm('Are you sure to delete?', function () {
            $.ajax({
                type: "POST",
                url: "/Admin/Blog/Delete",
                data: { id: id },
                dataType: "json",
                beforeSend: function () {
                    common.startLoading();
                },
                success: function (response) {
                    common.notify('Delete successful', 'success');
                    common.stopLoading();
                    loadBlogTable();
                },
                error: function (status) {
                    common.notify('Has an error in delete progress', 'error');
                    common.stopLoading();
                }
            });
        });
    }

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
                $('#txtImageM').val(path);
                common.notify('Upload image succesful!', 'success');

            },
            error: function () {
                common.notify('There was error uploading files!', 'error');
            }
        });
    }



}