

var MyAccountController = function () {

    this.initialize = function () {
        loadData();
        registerEvents();
    }

    function registerEvents() {
        $.validator.addMethod('customphone', function (value, element) {
            return this.optional(element) || /\(?([0-9]{3})\)?([ .-]?)([0-9]{3})\2([0-9]{4})/.test(value);
        }, "Please enter a valid phone number");

        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                txtFullName: { required: true },
                txtUserName: { required: true },
                //txtPassword: {
                //    required: true,
                //    minlength: 6
                //},
                txtEmail: {
                    required: true,
                    email: true
                },
                txtPhoneNumber: 'customphone'
            }
        });

        $('#frmMaintainance2').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                txtOldPassword: {
                    required: true,
                    minlength: 6
                },
                txtPasswordNew: {
                    required: true,
                    minlength: 6
                },
                txtConfirmPassword: { equalTo: "#txtPasswordNew" }
            }
        });

        $('#changepassword').on('click', function (e) {
            e.preventDefault();
            $('#modal-add-edit').modal('show');


        });
        $('#btnSaveInfo').on('click', function (e) {
            e.preventDefault();
            if ($('#frmMaintainance').valid()) {
                var id = $('#hidId').val();
                var fullName = $('#txtFullName').val();
                var userName = $('#txtUserName').val();
                var email = $('#txtEmail').val();
                var password = $('#txtPassword').val();
                var address = $('#txtAddress').val();
                var phone = $('#txtPhoneNumber').val();
                var birthday = $('#single_cal3').val();
                var gender = $('input[name="gender"]:checked').val();
                saveUser(id, userName, fullName, password, email, address, phone, birthday, gender);
            }

        });

        


        $('#ChangePass').on('click', function (e) {
            if ($('#frmMaintainance2').valid()) {
            e.preventDefault();

            var id = $('#hidId').val();
            var newpass = $('#txtPasswordNew').val();
            var oldpassword = $('#txtOldPassword').val();
            var confirmpassword = $('#txtConfirmPassword').val();
            var fullName = $('#txtFullName').val();
            ChangePassUser(id, newpass, oldpassword, confirmpassword, fullName);
             }
             return false;
        });


        //$('#btnSelectImg').on('click', function () {
        //    $('#fileInputImageM').click();
        //});

        //$("#fileInputImageM").on('change', function () {
        //    var fileUpload = $(this).get(0);
        //    var files = fileUpload.files;
        //    var data = new FormData();
        //    fileInputImage(files, data);
        //});

    };

    function resetFormMaintainance() {

        $('#txtOldPassword').val('');
        $('#txtPasswordNew').val('');
        $('#txtPassword').val('');
        $('#txtConfirmPassword').val('');

    }

    function initRoleList(selectedRoles) {
        $.ajax({
            url: "/Admin/Role/GetAll",
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {
                var template = $('#role-template').html();
                var data = response;
                var render = '';
                $.each(data, function (i, item) {
                    var checked = '';
                    if (selectedRoles !== undefined && selectedRoles.indexOf(item.Name) !== -1) {

                        checked = 'checked';
                        render += Mustache.render(template,
                            {
                                Name: item.Name,
                                Description: item.Description,
                                Checked: checked
                            });
                    }

                });
                $('#list-roles').html(render);
            }
        });
    }

    function loadData() {
        $.ajax({
            type: "GET",
            url: "/Admin/MyAcc/GetAccount",
            dataType: "json",
            beforeSend: function () {
                common.startLoading();
            },
            success: function (response) {
                var data = response;
                resetFormMaintainance();
                $('#hidId').val(data.Id);
                $('#txtFullName').val(data.FullName);
                $('#txtUserName').val(data.UserName);
                $('#txtEmail').val(data.Email);
                $('#txtAddress').val(data.Address);
                $('#txtPhoneNumber').val(data.PhoneNumber);
                $('#single_cal3').val(common.dateFormatJson(data.BirthDay));
                $('input[name="gender"]:checked').val(['' + data.Gender + '']);
                data.Gender == 1
                    ? $('input[id=gender][value=1]').prop("checked", true)
                    : $('input[id=gender][value=0]').prop("checked", true);
                initRoleList(data.Roles);
                common.stopLoading();


            },
            error: function () {
                common.notify('Có lỗi xảy ra', 'error');
                common.stopLoading();
            }
        });
    };

    function clearFileInput(ctrl) {
        try {
            ctrl.value = null;
        } catch (ex) { }
        if (ctrl.value) {
            ctrl.parentNode.replaceChild(ctrl.cloneNode(true), ctrl);
        }
    }


    function saveUser(id, username, fullname, password, email, address, phone, birthday, gender) {
        $.ajax({
            type: "POST",
            url: "/Admin/MyAcc/UpdateUserByUser",
            data: {
                Id: id,
                FullName: fullname,
                Username: username,
                Password: password,
                oldPassword: password,
                Email: email,
                Address: address,
                PhoneNumber: phone,
                BirthDay: birthday,
                Gender: gender
            },
            dataType: "json",
            beforeSend: function () {
                common.startLoading();
            },
            success: function (response) {
                common.notify('Save user succesful', 'success');
                resetFormMaintainance();
                loadData();

                common.stopLoading();
            },
            error: function () {
                common.notify('Has an error', 'error');
                common.stopLoading();
            }
        });
    }

    function ChangePassUser(id, newpassword, oldpassword, confirmPassword, fullName) {
        $.ajax({
            type: "POST",
            url: "/Admin/MyAcc/UpdateUserByUser",
            data: {
                Id: id,
                Password: newpassword,
                oldPassword: oldpassword,
                confirmPassword: confirmPassword,
                FullName: fullName

            },
            dataType: "json",
            beforeSend: function () {
                common.startLoading();
            },
            success: function (response) {
                common.notify('change password succesful', 'success');
                resetFormMaintainance();
                $('#modal-add-edit').modal('hide');

            },
            error: function () {
                common.notify('Has an error', 'error');
                common.stopLoading();
            }
        });
    }
    //function fileInputImage(files, data) {

    //    for (var i = 0; i < files.length; i++) {
    //        data.append(files[i].name, files[i]);
    //    }
    //    $.ajax({
    //        type: "POST",
    //        url: "Upload/UploadImage",
    //        contentType: false,
    //        processData: false,
    //        data: data,
    //        success: function (path) {
    //            $('#txtImageM').val(path);
    //            $('#imgLoadImg').closest('div').remove();
    //            $('#imgLoadImg').append('<img width="400"  data-path="' + path + '" src="' + path + '"></div>');
    //            common.notify('Upload image succesful!', 'success');
    //            $("#txtImageM").attr("readonly", true);

    //        },
    //        error: function () {
    //           // common.notify('There was error uploading files!', 'error');
    //            common.notify("You dont't upload files", 'error');
    //        }
    //    });
    //}
}