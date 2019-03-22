


var loginController = function () {
    // phương thức tĩnh
    this.initialize = function () {
        registerEvents();
    }
    // register các event
    var registerEvents = function () {

        // sk validation cho form login
        $('#frmLogin').validate({

            errorClass: 'red',
            ignore: [], //
            lang: 'vi',
            rules: {
                userName: {
                    required: true,
                    minlength: 4,
                    maxlength:30
                },
                password: {
                    required: true,
                    minlength: 4,
                    maxlength: 30
                }
            }

        });

        // sk click nút đăng nhập
        $('#btnLogin').on('click', function (e) {
            if ($('#frmLogin').valid()) {

                e.preventDefault();
                var user = $('#txtUserName').val();
                var password = $('#txtPassword').val();
                login(user, password);
            }
          
        });

    }

    var login = function (user, pass) {

        $.ajax({
            type: 'POST',
            data: {
                UserName: user,
                Password : pass
            },
            dataType: 'json',
            url: '/admin/login/authen',
            success: function (res) {
                if (res.Success) {
                    window.location.href = "/Admin/Home/Index";
                    ;
                }
                else {
                    common.notify('Đăng nhập không thành công', 'error');
                }
            }
        })
    }

}