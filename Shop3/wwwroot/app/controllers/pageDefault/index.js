


var PageDefaultController = function() {

    this.initialize = function() {
        loadPageDefault();
        registerEvents();
        registerControls();
    }

    function registerEvents() {
        
        

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
             editPageDefault(that);
        });

       

        $('#btnSave').on('click', function (e) {
            savePageDefault(e);
        });

    };

    function loadPageDefault() {
        $.ajax({
            type: "GET",
            url: "/admin/PageDefault/GetAllPageDefault",
            dataType: "json",
            beforeSend: function() {
                common.startLoading();
            },
            success: function(response) {

                var template = $('#table-template').html();
                var render = "";
                $.each(response,function(i, item) {
                        render += Mustache.render(template,
                            {
                                Id : item.Id,
                                Title: item.Title,
                                Content: item.Content
                        });
                    });
                if (render != undefined) {
                    $('#defaultpage-table').html(render);
                } else {
                    $('#defaultpage-table').html('');
                }
                common.stopLoading();
            },
            error: function(status) {
                console.log(status);
            }

        });
    };
    
    function registerControls() {
        CKEDITOR.replace('txtContent', {});

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
        $('#Id').val(0);
        $('#txtTitle').val('');
        CKEDITOR.instances.txtContent.setData('');

    }

    function editPageDefault(id) {
        $.ajax({
            type: "GET",
            url: "/Admin/PageDefault/GetPageDefaultById",
            data: { id: id },
            dataType: "json",
            beforeSend: function () {
                common.startLoading();
            },
            success: function (response) {

                var data = response;
                $('#Id').val(data.Id);
                $('#txtTitle').val(data.Title);
                CKEDITOR.instances.txtContent.setData(data.Content);
                $('#modal-add-edit').modal('show');
                common.stopLoading();

            },
            error: function (status) {
                common.notify('Có lỗi xảy ra', 'error');
                common.stopLoading();
            }
        });
    }

    function savePageDefault(e) {
        if ($('#frmMaintainance').valid()) {
            e.preventDefault();
            var id = $('#Id').val();
            var title = $('#txtTitle').val();
            var content = CKEDITOR.instances.txtContent.getData();

            $.ajax({
                type: "POST",
                url: "/Admin/PageDefault/UpdatePageDefault",
                data: {
                    Id: id,
                    title: title,
                    Content: content
                    
                },
                dataType: "json",
                beforeSend: function () {
                    common.startLoading();
                },
                success: function (response) {
                    common.notify('Update successful', 'success');
                    loadPageDefault();
                    $('#modal-add-edit').modal('hide');
                    resetForm();
                    common.stopLoading();
                    
                },
                error: function () {
                    common.notify('Has an error in save progress', 'error');
                    common.stopLoading();
                }
            });
            return false;
        }

    }

  

}