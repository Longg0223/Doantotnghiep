(function ($) {
    var self = this;
    self.IsUpdate = false;    
    self.User = {
        id: null,
        full_name: null,
        password: "",
        email: "",
        address: "",
        phone_number: "",
        role: 0  
    }
    self.GetById = function (id, renderCallBack) {
        //self.userData = {};
        if (id != null && id != "") {
            $.ajax({
                url: '/Admin/AppUsers/GetById',
                type: 'GET',
                dataType: 'json',
                data: {
                    id: id
                },
                beforeSend: function () {
                    //Loading('show');
                },
                complete: function () {
                    ////Loading('hiden');
                },
                success: function (response) {
                    if (response.Data != null) {
                        //self.GetImageByProductId(id);
                        renderCallBack(response.Data);
                        self.Id = id;
                        
                    }
                }
            })
        }
    }

    self.AddUser = function (userView) {
        $.ajax({
            url: '/Users/AddRegister',
            type: 'POST',
            dataType: 'json',
            data: {
                UserModelView: userView
            },
            beforeSend: function () {
                Loading('show');
            },
            complete: function () {
                Loading('hiden');
            },
            success: function (response) {
                if (response.success) {                   
                    window.location.href = '/';
                }
                else {
                    if (response.isPhoneExist) {
                        $(".phone_number-exist").show().text("Phone đã tồn tại");
                    }
                    if (response.isMailExist) {
                        $(".email-exist").show().text("Email đã tồn tại");
                    }
                }
            }
        })
    }

    self.UpdateUser = function (userView) {
        $.ajax({
            url: '/Admin/AppUsers/Update',
            type: 'POST',
            dataType: 'json',
            data: {
                UserModelView: userView
            },
            beforeSend: function () {
                Loading('show');
            },
            complete: function () {
                Loading('hiden');
            },
            success: function (response) {
                if (response.success) {
                    tedu.notify('Cập nhật dữ liệu thành công', 'success');
                    self.GetDataPaging(true);
                    $('#userModal').modal('hide');
                }
               
            }
        })
    }


    self.ValidateLoginUser = function () {
        $("#form-login").validate({
            rules:
            {
                email_phone: {
                    required: true,
                },                
                password: {
                    required: true,
                    minlength: 8
                }
                
            },
            messages:
            {
                email_phone: {
                    required: "Email hoặc Số điện thoại không được trống",
                },               
                password: {
                    required: "Mật khẩu không được để trống",
                    minlength: "Mật khẩu ít nhất 8 kí tự"
                }
            },
            submitHandler: function (form) {

                var userName = $("#email_phone").val();
                var passWord = $("#password").val();
                self.AppUserLogin(userName, passWord);
            }
        });
    }


    self.AppUserLogin = function (userName, passWord) {
        debugger
        $.ajax({
            url: '/Users/AppLogin',
            type: 'POST',
            dataType: 'json',
            data: {
                userName: userName,
                passWord: passWord
            },
            beforeSend: function () {
                Loading('show');
            },
            complete: function () {
                Loading('hiden');
            },
            success: function (response) {
                if (response.success == false) {
                    $(".results").show();
                    $(".text-error-custom").text("Tên đăng nhập hoặc mật khẩu không đúng");
                    //window.location.href = '/';
                }
                else if (response.success.status == 1) {
                    $(".results").show();
                    $(".text-error-custom").text("Tài khoản chưa được kích hoạt");
                }
                else {
                    if (response.success.role == 0) {
                        window.location.href = '/admin';
                    }
                    else {
                        window.location.href = '/'
                    }                   
                }
            }
        })
    }

    self.ValidateUser = function () {
        jQuery.validator.addMethod("headphone", function (value, element) {
            var vnf_regex = /((032|033|034|035|036|037|038|039|056|058|059|070|076|077|078|079|081|082|083|084|085|086|087|088|089|090|091|092|093|094|096|097|098|099)+([0-9]{7})\b)/g;
            return vnf_regex.test(value);
        });
        jQuery.validator.addMethod("rigidemail", function (value, element) {
            var testemail = /^([^@\s]+)@((?:[-a-z0-9]+\.)+[a-z]{2,})$/i;
            return testemail.test(value);
        });
        $("#form-submit").validate({
            rules:
            {
                full_name: {
                    required: true,
                    minlength: 8
                },
                phone_number: {
                    required: true,
                    headphone: true,
                    minlength: 10,
                    maxlength: 11,
                    number: isNaN
                },
                password: {
                    required: true,
                    minlength: 8,
                },
                email: {
                    required: true,
                    rigidemail: true
                },
                confirm_password: {
                    required: true,
                    equalTo: "#password"
                }//,
                //confirm: {
                //    required: "Bạn chưa nhập lại mật khẩu.",
                //    equalTo: "Mật khẩu không đúng.",
                //    min: "Mật khẩu ít nhất 8 kí tự"
                //}
            },
            messages:
            {
                full_name: {
                    required: "Họ tên không được để trống",
                    minlength:"Tên tối thiểu 8 kí tự"
                },
                phone_number: {
                    required: "Số điện thoại không được để trống",
                    headphone: "Số điện thoại không hợp lệ",
                    minlength: "Số điện thoại từ 10 đến 11 kí tự",
                    maxlength: "Số điện thoại từ 10 đến 11 kí tự",
                },
                password: {
                    required: "Mật khẩu không được để trống",
                    minlength: "Mật khẩu tối thiểu 8 kí tự",
                },
                email: {
                    required: "Email không được để trống",
                    rigidemail: "Email không đúng định dạng",
                },
                confirm_password: {
                    required: "Bạn chưa nhập lại mật khẩu.",
                    equalTo: "Mật khẩu không đúng.",
                    min: "Mật khẩu ít nhất 8 kí tự"
                }
            },
            submitHandler: function (form) {               
                self.GetValue();
               
                self.AddUser(self.User);
            }
        });
    }

    self.GetValue = function () {
        self.User.full_name = $("#full_name").val();
        self.User.phone_number = $("#phone_number").val();
        self.User.email = $("#email").val();
        self.User.address = $("#address").val();
        self.User.role = $("#role").val(); 
        self.User.password = $("#password").val();      

    }


    self.RenderHtmlByObject = function (view) {
        $("#full_name").val(view.full_name);
        $("#phone_number").val(view.phone_number);
        $("#email").val(view.email);
        $("#address").val(view.address);
        $("#role").val(view.role);
        $("#password").val(view.password);
    }


    $(document).ready(function () {
        self.ValidateUser();
        self.ValidateLoginUser();
    })
})(jQuery);