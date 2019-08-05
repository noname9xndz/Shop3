


var ImageManagement = function () {
    var self = this;
    var parent = parent;

    var imagesAdd = [];
    var imagesRemove = [];

    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {
        $('body').on('click', '.btn-images', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $('#hidId').val(that);
            clearFileInput($("#fileImage"));
            loadImages();
            $('#modal-image-manage').modal('show');
        });

        $('body').on('click', '.btn-delete-image', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            imagesRemove.push(id);
            $(this).closest('div').remove(); // lấy div cha gần nhất remove img
            
        });

        $("#fileImage").on('change', function () {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;
            var data = new FormData();
            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }
            //todo don't get path current
            $.ajax({
                type: "POST",
                url: "/Admin/Upload/UploadImage",
                contentType: false,
                processData: false,
                data: data,
                success: function (path) {
                    clearFileInput($("#fileImage"));
                    imagesAdd.push(path); // đường dẫn img , data-path đẩy vào tương đối, src tuyệt đối 
                    $('#image-list').append('<div class="col-md-3"><img width="100"  data-path="' + path + '" src="' + path + '"></div>');
                    common.notify('Đã tải ảnh lên thành công!', 'success');

                },
                error: function () {
                    common.notify('There was error uploading files!', 'error');
                }
            });
        });

        $("#btnSaveImages").on('click', function () {
            var imageList = [];
            $.each($('#image-list').find('img'), function (i, item) {
                imageList.push($(this).data('path')); // đọc đường dẫn file
            });
            // console.log(imagesAdd);
            //console.log(imageList);

            $.ajax({
                url: '/admin/Product/SaveImages',
                data: {
                    productId: $('#hidId').val(),
                    imagesAdd: imagesAdd,
                    imagesRemove: imagesRemove
                },
                type: 'post',
                dataType: 'json',
                success: function (response) {
                    $('#modal-image-manage').modal('hide');
                    $('#image-list').html('');
                    common.notify('Save successful', 'success');
                    clearFileInput($("#fileImage"));
                    imagesAdd = [];
                    imagesRemove = [];
                },
                error: function () {
                    imagesAdd = [];
                    imagesRemove = [];
                }
            });
        });
    }
    
    function loadImages() {
        $.ajax({
            url: '/admin/Product/GetImages',
            data: {
                productId: $('#hidId').val()
            },
            type: 'get',
            dataType: 'json',
            success: function (response) {
                var render = '';
                $.each(response, function (i, item) {
                    render += '<div class="col-md-3"><img width="100" src="' + item.Path + '"><br/><a href="javascript:void(0)" class="btn-delete-image" data-id="' + item.Id + '">Xóa</a></div>'
                });
                $('#image-list').html(render);
                clearFileInput('#fileImage');
            }
        });
    }

    function clearFileInput(ctrl) {
        try {
            ctrl.value = null;
            $(ctrl).val('');
        } catch (ex) { }
        if (ctrl.value) {
            ctrl.parentNode.replaceChild(ctrl.cloneNode(true), ctrl);
        }
    }
}