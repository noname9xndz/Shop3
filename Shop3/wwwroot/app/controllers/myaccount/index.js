

var MyAccountController  = function () {

    this.initialize = function () {
        loadData();
        registerEvents();
    }

    function registerEvents() {

        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                txtFullName: { required: true },
                txtUserName: { required: true },
                txtPassword: {
                    required: true,
                    minlength: 6
                },
                txtEmail: {
                    required: true,
                    email: true
                }
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
                txtConfirmPassword: { equalTo: "#txtPassword" }
            }
        });

        $('#changePass').on('click', function (e) {
            e.preventDefault();
            $('#modal-add-edit').modal('show');


        });

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();

                var id = $('#hidId').val();
                var fullName = $('#txtFullName').val();
                var userName = $('#txtUserName').val();
                var password = $('#txtPassword').val();
                var email = $('#txtEmail').val();
                var avatar = $('#txtImageM').val();
                var checkMail = $('#checkMail').prop('checked') == true ? 1 : 0;
                saveUser(id, fullName, userName, password, email, avatar, checkMail);

            }
            return false;
        });

        $('#ChangePass').on('click', function (e) {
            if ($('#frmMaintainance2').valid()) {
                e.preventDefault();

                var id = $('#hidId').val();
                var newpass = $('#txtPasswordNew').val();
                var oldpassword = $('#txtOldPassword').val();
                var confirmpassword = $('#txtConfirmPassword').val();
                ChangePassUser(id, newpass, oldpassword, confirmpassword);
            }
            return false;
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

    };

    function resetFormMaintainance() {

        $('#txtOldPassword').val('');
        $('#txtPasswordNew').val('');
        $('#txtPassword').val('');
        $('#txtConfirmPassword').val('');
        $('#txtImageM').val('');
        $("#addMember").removeAttr("readonly");

    }

    function initRoleList(selectedRoles) {
        $.ajax({
            url: "Role/GetAll",
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {
                var template = $('#role-template').html();
                var data = response;
                var render = '';
                $.each(data, function (i, item) {
                    var checked = '';
                    if (selectedRoles !== undefined && selectedRoles.indexOf(item.Name) !== -1)
                        checked = 'checked';
                    render += Mustache.render(template,
                        {
                            Name: item.Name,
                            Description: item.Description,
                            Checked: checked
                        });
                });
                $('#list-roles').html(render);
            }
        });
    }

    function loadData() {
        $.ajax({
            type: "GET",
            url: "/Acc/GetAccount",
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
                $('#imgLoadImg').append('<img width="400"  data-path="' + data.Avatar + '" src="' + data.Avatar + '">');
                $('#txtImageM').val(data.Avatar);
                initRoleList(data.Roles);
                $('#checkMail').prop('checked', data.IsSendMail == 1);
                $('#txtImageM').val('');
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


    function saveUser(id, fullName, userName, password, email, avatar, checkMail) {
        $.ajax({
            type: "POST",
            url: "/User/UpdateUserByUser",
            data: {
                Id: id,
                FullName: fullName,
                UserName: userName,
                Password: password,
                oldPassword: password,
                Email: email,
                Avatar: avatar,
                IsSendMail: checkMail
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

    function ChangePassUser(id, newpassword, oldpassword, confirmPassword) {
        $.ajax({
            type: "POST",
            url: "/User/UpdateUserByUser",
            data: {
                Id: id,
                Password: newpassword,
                oldPassword: oldpassword,
                confirmPassword: confirmPassword

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

    function fileInputImage(files, data) {

        for (var i = 0; i < files.length; i++) {
            data.append(files[i].name, files[i]);
        }
        $.ajax({
            type: "POST",
            url: "Upload/UploadImage",
            contentType: false,
            processData: false,
            data: data,
            success: function (path) {
                $('#txtImageM').val(path);
                $('#imgLoadImg').closest('div').remove();
                $('#imgLoadImg').append('<img width="400"  data-path="' + path + '" src="' + path + '"></div>');
                common.notify('Upload image succesful!', 'success');
                $("#txtImageM").attr("readonly", true);
                
            },
            error: function () {
               // common.notify('There was error uploading files!', 'error');
                common.notify("You dont't upload files", 'error');
            }
        });
    }
}