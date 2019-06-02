


var ContactController = function() {

    this.initialize = function() {
        loadContact();
        registerEvents();
        registerControls();
    }

    function registerEvents() {
        
        $("#btnCreate").on('click', function () {
            resetForm();
            $('#modal-add-edit').modal('show');

        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            editContact(that);
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            deleteContact(that);
        });

        $('#btnSave').on('click', function (e) {
            saveContact(e);
        });

    };

    function loadContact() {
        $.ajax({
            type: "GET",
            url: "/admin/Contact/GetAll",
            dataType: "json",
            beforeSend: function() {
                common.startLoading();
            },
            success: function(response) {

                var template = $('#table-template').html();
                var render = "";
                $.each(response,
                    function(i, item) {
                        render += Mustache.render(template,
                            {
                                Id : item.Id,
                                Name: item.Name,
                                Phone: item.Phone,
                                Email: item.Email,
                                Website: item.Website,
                                Address: item.Address,
                                Status: common.getStatus(item.Status)
                            });
                    });
                if (render != undefined) {
                    $('#contact-table').html(render);
                } else {
                    $('#contact-table').html('');
                }
                common.stopLoading();
            },
            error: function(status) {
                console.log(status);
            }

        });
    };
    
    function registerControls() {
        CKEDITOR.replace('txtOther', {});

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
        $('#txtName').val('');
        $('#txtPhone').val('');
        $('#txtWebsite').val('');
        $('#txtEmail').val('');
        $('#txtAddress').val('');
        $('#txtStatus').val('');
        $('#txtLat').val('');
        $('#txtLng').val('');
        
        CKEDITOR.instances.txtOther.setData('');
        $('#ckStatus').prop('checked', true);

    }

    function editContact(id) {
        $.ajax({
            type: "GET",
            url: "/Admin/Contact/GetById",
            data: { id: id },
            dataType: "json",
            beforeSend: function () {
                common.startLoading();
            },
            success: function (response) {

                var data = response;
                $('#Id').val(data.Id);
                $('#txtName').val(data.Name);
                $('#txtPhone').val(data.Phone);
                $('#txtEmail').val(data.Email);
                $('#txtWebsite').val(data.Website);
                $('#txtAddress').val(data.Address);
                $('#txtLat').val(data.Lat);
                $('#txtLng').val(data.Lng);

                CKEDITOR.instances.txtOther.setData(data.Other);
                $('#ckStatus').prop('checked', data.Status == 1);
                $('#modal-add-edit').modal('show');
                common.stopLoading();

            },
            error: function (status) {
                common.notify('Có lỗi xảy ra', 'error');
                common.stopLoading();
            }
        });
    }

    function saveContact(e) {
        if ($('#frmMaintainance').valid()) {
            e.preventDefault();
            var id = $('#Id').val();
            var name = $('#txtName').val();
            var phone = $('#txtPhone').val();
            var email = $('#txtEmail').val();
            var website = $('#txtWebsite').val();
            var address = $('#txtAddress').val();
            var lat = $('#txtLat').val();
            var lng = $('#txtLng').val();
            
            var other = CKEDITOR.instances.txtOther.getData();
            var status = $('#ckStatus').prop('checked') == true ? 1 : 0;

            $.ajax({
                type: "POST",
                url: "/Admin/Contact/SaveEntity",
                data: {
                    Id: id,
                    Name: name,
                    Phone: phone,
                    Email: email,
                    Website: website,
                    Address: address,
                    Lat : lat,
                    Lng : lng,
                    Other : other,
                    Status: status
                    
                },
                dataType: "json",
                beforeSend: function () {
                    common.startLoading();
                },
                success: function (response) {
                    common.notify('Update contact successful', 'success');
                    $('#modal-add-edit').modal('hide');
                    resetForm();
                    common.stopLoading();
                    loadContact();
                },
                error: function () {
                    common.notify('Has an error in save contact progress', 'error');
                    common.stopLoading();
                }
            });
            return false;
        }

    }

    function deleteContact(id) {
        common.confirm('Are you sure to delete?', function () {
            $.ajax({
                type: "POST",
                url: "/Admin/Contact/Delete",
                data: { id: id },
                dataType: "json",
                beforeSend: function () {
                    common.startLoading();
                },
                success: function (response) {
                    common.notify('Delete successful', 'success');
                    common.stopLoading();
                    loadContact();
                },
                error: function (status) {
                    common.notify('Has an error in delete progress', 'error');
                    common.stopLoading();
                }
            });
        });
    }

}