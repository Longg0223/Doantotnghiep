(function ($) {
    var self = this;
    self.IsUpdate = false;    
    self.Contract = {
        id: null,
        full_name: null,
        password: "",
        email: "",
        address: "",
        phone_number: "",
        role: 0  
    }
    self.ContractSearch = {
        name: "",
        status: null,
        PageIndex: tedu.configs.pageIndex,
        PageSize: tedu.configs.pageSize
    }

    self.Files = {};

    self.RenderTableHtml = function (data) {
        var html = "";
        if (data != "" && data.length > 0) {
            var index = 0;
            for (var i = 0; i < data.length; i++) {
                var item = data[i];

                var htmlSelect = "";
                if (item.status == 0) {
                    htmlSelect = "<select  class='form-select btn-outline-secondary ' onChange=update(" + item.id + ",this)>" +
                        "<option  value = '0'> Đang chờ xử lý</option>" +
                        "<option  value='1'>Đã liên hệ</option>" +
                        "<option  value='2'>Đã huỷ</option></select>";
                }
                else if (item.status == 1) {
                    htmlSelect = "<select  class='form-select btn-outline-primary'>" +
                        "<option  value='1'>Đã liên hệ</option></select>";
                }
                else {
                    htmlSelect = "<select  class='form-select btn-outline-danger'>" +
                        "<option  value='2'>Đã hủy</option></select>";
                }
                html += "<tr>";
                html += "<td>" + (++index) + "</td>";
                html += "<td> <a href=\"javascript: void (0)\" onClick=Detail(" + item.id + ")>" + item.full_name + "</a></td>";
                html += "<td>" + item.phone_number + "</td>";
                html += "<td>" + item.datestr + "</td>";   
                html += "<td>" + htmlSelect + "</td>";   
                html += "<td style=\"text-align: center;\">" +
                    "<button  class=\"btn btn-danger custom-button\" onClick=\"Deleted(" + item.id +")\"><i  class=\"bi bi-trash\"></i></button>" +
                    "</td>";
                
                html += "</tr>";
            }
        }
        else {
            html += "<tr><td colspan=\"10\" style=\"text-align:center\">Không có dữ liệu</td></tr>";
        }
        $("#tblData").html(html);
    };
    self.Detail = function (id) {
        if (id != null && id != "") {
            $.ajax({
                url: '/Admin/Contract/GetById',
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
                        var item = response.Data;
                        $("#DetailModal").modal('show');
                        $(".full-name").text(item.full_name);
                        $(".number").text(item.phone_number);
                        $(".datestr").text(item.datestr);
                        $(".note").text(item.note);

                    }
                }
            })
        }
    }
    self.update = function (id, tag) {
        if (id != null && id != "") {
            var status = $(tag).val();
            $.ajax({
                url: '/Admin/Contract/UpdateStatus',
                type: 'POST',
                dataType: 'json',
                data: {
                    id: id,
                    status: status
                },
                beforeSend: function () {
                    //Loading('show');
                },
                complete: function () {
                    ////Loading('hiden');
                },
                success: function (response) {
                    if (response.success) {
                        //self.GetImageByProductId(id);
                        self.GetDataPaging(true);
                        tedu.notify('Cập nhật trạng thái thành công', 'success');
                    }
                }
            })
        }
    }    

    self.WrapPaging = function (recordCount, callBack, changePageSize) {
        var totalsize = Math.ceil(recordCount / tedu.configs.pageSize);
        //Unbind pagination if it existed or click change pagesize
        if ($('#paginationUL a').length === 0 || changePageSize === true) {
            $('#paginationUL').empty();
            $('#paginationUL').removeData("twbs-pagination");
            $('#paginationUL').unbind("page");
        }
        //Bind Pagination Event
        $('#paginationUL').twbsPagination({
            totalPages: totalsize,
            visiblePages: 7,
            first: '<<',
            prev: '<',
            next: '>',
            last: '>>',
            onPageClick: function (event, p) {
                tedu.configs.pageIndex = p;
                setTimeout(callBack(), 200);
            }
        });
    }
    self.Deleted = function (id) {
        if (id != null && id != "") {
            tedu.confirm('Bạn có chắc muốn xóa liên hệ này?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Contract/Delete",
                    data: { id: id },
                    beforeSend: function () {
                        // tedu.start//Loading();
                    },
                    success: function () {
                        tedu.notify('Đã xóa thành công', 'success');
                        //tedu.stop//Loading();
                        //loadData();
                        self.GetDataPaging(true);
                    },
                    error: function () {
                        tedu.notify('Has an error', 'error');
                        tedu.stop//Loading();
                    }
                });
            });
        }
    }

    self.GetDataPaging = function (isPageChanged) {

        self.ContractSearch.PageIndex = tedu.configs.pageIndex;
        self.ContractSearch.PageSize = tedu.configs.pageSize;

        $.ajax({
            url: '/Admin/Contract/GetAllPaging',
            type: 'GET',
            data: self.ContractSearch,
            dataType: 'json',
            beforeSend: function () {
                //Loading('show');
            },
            complete: function () {
                //Loading('hiden');
            },
            success: function (response) {
                self.RenderTableHtml(response.data.Results);
                $('#lblTotalRecords').text(response.data.RowCount);
                if (response.data.RowCount != null && response.data.RowCount > 0) {
                    self.WrapPaging(response.data.RowCount, function () {
                        GetDataPaging();
                    }, isPageChanged);
                }

            }
        })

    };


    self.AddUser = function (userView) {
        $.ajax({
            url: '/Admin/AppUsers/Add',
            type: 'POST',
            dataType: 'json',
            data: {
                UserModelView: userView
            },
            beforeSend: function () {
                //Loading('show');
            },
            complete: function () {
                //Loading('hiden');
            },
            success: function (response) {
                if (response.success) {
                    tedu.notify('Thêm mới dữ liệu thành công', 'success');
                    self.GetDataPaging(true);
                    $('#userModal').modal('hide');
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
                },
                phone_number: {
                    required: true,
                    //headphone: false,
                    minlength: 10,
                    min: 0,
                    maxlength: 10,
                    number: isNaN
                },
                password: {
                    required: true
                },
                email: {
                    required: true
                },
                role: {
                    required: true
                }
            },
            messages:
            {
                full_name: {
                    required: "Họ tên không được để trống",
                },
                phone_number: {
                    required: "Số điện thoại không được để trống",
                    headphone: "Số điện thoại không hợp lệ"
                },
                password: {
                    required: "Mật khẩu không được để trống",
                    min: "Mật khẩu ít nhất 6 kí tự"
                },
                email: {
                    required: "Email không được để trống"
                },
                role: {
                    required: "Loại tài khoản không được để trống"
                }
            },
            submitHandler: function (form) {               
                self.GetValue();
                //var isNoUpdateImage = false;
                //if (self.UserImages !== null && self.UserImages.name != "" && self.UserImages.name != undefined) {
                //    self.User.Avartar = "/avatar/" + self.UserImages.name;
                //}
                //else {
                //    isNoUpdateImage = true;
                //}

                if (self.IsUpdate) {
                    self.UpdateUser(self.User);
                }
                else {                    
                    self.AddUser(self.User);
                }
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

    Set.SetValue = function () {
        if (self.User.Id !== null && self.User.Id !== '') {
            $("#fullname").val(self.User.FullName);
            $("#mobile").val(self.User.PhoneNumber);
            $("#email").val(self.User.Email);
            $("#address").val(self.User.Address);
            $("#username").val(self.User.UserName);
            $("#password").val(self.User.PassWord);

            var dataSelecteRole = $('.data-select2').select2("data");
            if (dataSelecteRole != null && dataSelecteRole.length > 0) {
                for (var i = 0; i < dataSelecteRole.length; i++) {
                    var item = dataSelecteRole[i];
                    if (item.id != null) {
                        self.User.Roles.push(parseInt(item.id));
                    }
                }
            }

            $("#birthday").val(moment(self.User.BirthDay).format('DD/MM/YYYY'));

            $(".box-avatar").css({ 'background': 'url(/' + (self.User.Avatar !== null ? self.User.Avatar : "image/admin/avatar.jpg") + ')', 'display': 'block' });
        }
    }

    self.RenderHtmlByObject = function (view) {
        $("#full_name").val(view.full_name);
        $("#phone_number").val(view.phone_number);
        $("#email").val(view.email);
        $("#address").val(view.address);
        $("#role").val(view.role);
        $("#password").val(view.password);
    }

    self.SubmitUser = function (user) {
        var form_data = new FormData();

        form_data.append("Id", self.User.Id);
        form_data.append("FullName", self.User.FullName);
        form_data.append("PhoneNumber", self.User.PhoneNumber);
        form_data.append("Email", self.User.Email);
        form_data.append("Address", self.User.Address);
        form_data.append("UserName", self.User.UserName);
        form_data.append("Password", self.User.Password);

        //var roles = [];
        //$.each($('input[name="ckRoles"]'), function (i, item) {
        //    if ($(item).prop('checked') === true)
        //        roles.push($(item).prop('value'));
        //});
        //if (roles.length > 0) {
        //    $.each(roles, function (key, item) {
        //        form_data.append("Roles", item);
        //    })
        //}

        //form_data.append("BirthDayTest", self.User.BirthDay);
        //form_data.append("BirthDay", new Date(date).toUTCString());
        //form_data.append("Files", self.Files);
        $.ajax({
            type: "POST",
            url: "/Admin/AppUsers/SaveEntity",
            data: form_data,
            contentType: false,
            processData: false,
            beforeSend: function () {
            },
            complete: function () {
                $(".add-edit-user").css("display", "inline-block");
            },
            success: function (response) {
                if (response.Status == true) {
                    //self.GetUser();
                    $('#create').modal('hide');
                    $.notify(response.message, 'success');
                }
                else {
                    $.notify(response.message, 'error');
                }
            },
            error: function (eror) {
                console.log(eror);
            }
        });
    }

    $(document).ready(function () {
        self.GetDataPaging();
        //self.Init();
        self.ValidateUser();
        //self.GetAllRole();
       /* $('.data-select2').select2();*/
        $(".modal").on("hidden.bs.modal", function () {
            $(this).find('form').trigger('reset');
            $("form").validate().resetForm();
            $("label.error").hide();
            $(".error").removeClass("error");
        });

        $(".btn-addorupdate").click(function () {
            $(".custom-format").removeAttr("disabled");
            $("#titleModal").text("Thêm mới tài khoản");
            $(".txtPassword").show();
            $(".btn-submit-format").text("Thêm mới");
            self.IsUpdate = false;
            $('#userModal').modal('show');
        })
        $('#select-right').on('change', function () {
            $('input.form-search').val("");
            self.ContractSearch.name = null;
            self.ContractSearch.status = $(this).val();
            self.GetDataPaging(true);
        });

        $('#ddlShowPage').on('change', function () {
            tedu.configs.pageSize = $(this).val();
            tedu.configs.pageIndex = 1;
            self.GetDataPaging(true);
        });

        $('input.form-search').on('input', function (e) {
            self.ContractSearch.name = $(this).val();
            self.GetDataPaging(true);
        });
       
    })
})(jQuery);