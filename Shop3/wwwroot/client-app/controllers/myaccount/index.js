

var MyAccountController = function () {

    this.initialize = function () {
        loadData();
        registerEvents();
    }

    function registerEvents() {
        $.validator.addMethod('customphone', function (value, element) {
           // return this.optional(element) || /^(\+91-|\+91|0)?\d{10}$/.test(value);
            return this.optional(element) || /\(?([0-9]{3})\)?([ .-]?)([0-9]{3})\2([0-9]{4})/.test(value);
        }, "Please enter a valid phone number");

        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                txtFullName: { required: true },
                txtPassword: {
                    required: true,
                    minlength: 6
                },
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
//                txtOldPassword: {
//                    required: true,
//                    minlength: 6
//                },
               txtPasswordNew: {
                   required: true,
                    minlength: 6
                },
                txtConfirmPassword: { equalTo: "#txtPasswordNew" }
            }
        });

        $('#changePass').on('click', function (e) {
            e.preventDefault();
           $('#modal-add-edit').modal('show');


        });
        $('#changeUser').on('click', function (e) {
            e.preventDefault();
            if ($('#frmMaintainance').valid()) {
               var id =  $('#hidId').val();
               var fullName = $('#txtFullName').val();
               var email = $('#txtEmail').val();
               var password = $('#txtPassword').val();
               var address = $('#txtAddress').val();
               var phone = $('#txtPhoneNumber').val();
               var birthday = $('#txtBirthDay').val();
               var gender = $('input[name="gender"]:checked').val();
               saveUser(id, fullName, password, email, address, phone,birthday,gender);
            }


        });
        $('#returnhome').on('click', function (e) {
            e.preventDefault();
            window.location.href = "/";


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


    };

    function resetFormMaintainance() {

        $('#txtOldPassword').val('');
        $('#txtPasswordNew').val('');
        $('#txtPassword').val('');
        $('#txtConfirmPassword').val('');

    }

    function loadData() {
        $.ajax({
            type: "GET",
            url: "/MyAccount/GetAccount",
            dataType: "json",
            beforeSend: function () {
                common.startLoading();
            },
            success: function (response) {
                var data = response;
                resetFormMaintainance();
                $('#hidId').val(data.Id);
                $('#txtFullName').val(data.FullName);
                $('#txtEmail').val(data.Email);
                $('#txtAddress').val(data.Address);
                $('#txtPhoneNumber').val(data.PhoneNumber);
                $('#txtBirthDay').val(common.dateFormatJson(data.BirthDay));
                $('input[name="gender"]:checked').val([''+data.Gender+'']);
                data.Gender == 1 ? $('input[id=gender][value=1]').prop("checked", true) : $('input[id=gender][value=0]').prop("checked", true)
                CheckPasswordUser();
                common.stopLoading();


            },
            error: function () {
                common.notify('Có lỗi xảy ra', 'error');
                common.stopLoading();
            }
        });
    };

    function CheckPasswordUser() {
        $.ajax({
            type: "GET",
            url: "/MyAccount/CheckPasswordUser",
            dataType: "json",
            beforeSend: function () {
                common.startLoading();
            },
            success: function (response) {
                if (response.status == "error") {
                    $("#oldpass").attr("hidden", true);
                } else {
                    $("#oldpass").removeAttr("hidden");
                }
                common.stopLoading();
            },
            error: function () {
                common.stopLoading();
            }
        });
    };

    
    function saveUser(id, fullName, password, email, address, phone, birthday, gender){
        $.ajax({
            type: "POST",
            url: "/MyAccount/UpdateUserByUser",
            data: {
                Id: id,
                FullName: fullName,
                Password: password,
                oldPassword: password,
                Email: email,
                Address : address,
                PhoneNumber : phone,
                BirthDay : birthday,
                Gender : gender
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
            url: "/MyAccount/UpdateUserByUser",
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

    
}