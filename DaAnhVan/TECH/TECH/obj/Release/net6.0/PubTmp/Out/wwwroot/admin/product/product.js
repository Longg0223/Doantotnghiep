(function ($) {
    var self = this;
    self.Data = [];
    self.ProductImages = {};
    self.IsUpdate = false;    
    self.Product = {
        id: null,
        name: null,
        category_id: "",
        avatar: "",
        price: "",
        color: "",
        quantity: "",
        short_desc: "",
        description: "",
        specifications: "",
        status: "",
        endow: "",
        type: "",
        differentiate:""
    }
    self.ProductSearch = {
        name: "",
        role: null,
        PageIndex: tedu.configs.pageIndex,
        PageSize: tedu.configs.pageSize
    }
    self.lstRole = [];

    self.addSerialNumber = function () {
        var index = 0;
        $("table tbody tr").each(function (index) {
            $(this).find('td:nth-child(1)').html(index + 1);
        });
    };
    self.Files = {};

    self.RenderTableHtml = function (data) {
        var html = "";
        if (data != "" && data.length > 0) {
            var index = 0;
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                html += "<tr>";
                html += "<td>" + (++index) + "</td>";
                html += "<td> <img src=/product-image/" + item.avatar + " class=\"item-image\" /></td>";
                html += "<td>" + item.name + "</td>";
                html += "<td>" + item.categorystr + "</td>";
                html += "<td>" + item.price + "</td>";            
                html += "<td style=\"text-align: center;\">" +

                    (item.status == 0 ? "<button  class=\"btn btn-dark custom-button\" onClick=UpdateStatus(" + item.id + ",1)><i class=\"bi bi-eye\"></i></button>" : "<button  class=\"btn btn-secondary custom-button\" onClick=UpdateStatus(" + item.id + ",0)><i class=\"bi bi-eye-slash\"></i></button>")  +
                    "<button  class=\"btn btn-primary custom-button\" onClick=\"Update(" + item.id +")\"><i  class=\"bi bi-pencil-square\"></i></button>" +
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
    self.Update = function (id) {
        if (id != null && id != "") {
            $(".txtPassword").hide();
            $("#titleModal").text("Cập nhật tài khoản");
            $(".btn-submit-format").text("Cập nhật");
            $(".custom-format").attr("disabled", "disabled");
            self.GetById(id, self.RenderHtmlByObject);
            self.Product.id = id;

            $(".product-add-update").show();
            $(".product-list").hide();
            self.IsUpdate = true;
        }
    }

    self.GetById = function (id, renderCallBack) {
        //self.ProductData = {};
        if (id != null && id != "") {
            $.ajax({
                url: '/Admin/Product/GetById',
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

    self.UpdateStatus = function (id,status) {
        $.ajax({
            url: '/Admin/Product/UpdateStatus',
            type: 'GET',
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
            tedu.confirm('Bạn có chắc muốn xóa sản phẩm này?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Product/Delete",
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

        self.ProductSearch.PageIndex = tedu.configs.pageIndex;
        self.ProductSearch.PageSize = tedu.configs.pageSize;

        $.ajax({
            url: '/Admin/Product/GetAllPaging',
            type: 'GET',
            data: self.ProductSearch,
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


    self.Init = function () {
        //self.GetUser();
        //self.GetAllRole();


        $(".btn-add").click(function () {
            //self.BindRoleHtml();
            //;           
            self.SetValueDefault();
            self.Product.Id = 0;
            $('#CreateOrUpdate').modal('show')
        })

        // hủy add và edit
        $(".cs-close-addedit,.btn-cancel-addedit").click(function () {
            $("#CreateEdit").css("display", "none");
        })

        

        $(".filesImages").change(function () {
            var files = $(this).prop('files')[0];

            var t = files.type.split('/').pop().toLowerCase();

            if (t != "jpeg" && t != "jpg" && t != "png" && t != "bmp" && t != "gif") {
                alert('Vui lòng chọn một tập tin hình ảnh hợp lệ!');
                //$("#avatar").val("");
                return false;
            }

            if (files.size > 2048000) {
                alert('Kích thước tải lên tối đa chỉ 2Mb');
                //$("#avatar").val("");
                return false;
            }

            var img = new Image();
            img.src = URL.createObjectURL(files);
            img.onload = function () {
                CheckWidthHeight(this.width, this.height);
            }
            var CheckWidthHeight = function (w, h) {
                if (w <= 300 && h <= 300) {
                    alert("Ảnh tối thiểu 300 x 300 px");
                }
                else {
                    $(".box-avatar").css({ 'background': 'url(' + img.src + ')', 'display': 'block' });                   
                    self.ProductImages = files;
                    //console.log(self.ProductImages);
                }
            }

        })


        //$('.filesImages').on('change', function () {
        //    var fileUpload = $(this).get(0);
        //    var files = fileUpload.files;
        //    if (files != null && files.length > 0) {
        //        var fileExtension = ['jpeg', 'jpg', 'png'];
        //        var html = "";
        //        for (var i = 0; i < files.length; i++) {
        //            if ($.inArray(files[i].type.split('/')[1].toLowerCase(), fileExtension) == -1) {
        //                alert("Only formats are allowed : " + fileExtension.join(', '));
        //            }
        //            else {
        //                console.log(files[i]);
        //                self.FileImages.push(files[i]);
        //                var src = URL.createObjectURL(files[i]);
        //                html += "<div class=\"box-item-image\"> <div class=\"image-upload item-image\" style=\"background-image:url(" + src + ")\"></div><a href=\"javascript:void(0)\" class=\"item-delete\" onclick=\"DeleteImage(0,this)\">Xóa</a></div>";
        //            }
        //        }
        //        if (html != "") {
        //            $(".image-default").hide();
        //            $(".box-images").append(html);
        //        }
        //    }

        //});


        //$("#birthday").datepicker({
        //    changeMonth: true,
        //    changeYear: true,
        //    dateFormat: 'dd/mm/yy',
        //    formatSubmit: 'yyyy/mm/dd'
        //});

        $(".btn-submit-search").click(function () {
            var id = $("#code_user_search").val();
            var fullName = $('#fullname_user_search').val();
            var userName = $('#name_user_search').val();
            var email = $('#email_user_search').val();
            var address = $('#address_user_search').val();
            var phoneNumber = $('#phone_user_search').val();
            var birthDay = $('#birthday_user_search').val();

            self.ProductSearch = {
                Id: id,
                FullName: fullName,
                UserName: userName,
                Email: email,
                Address: address,
                Phone: phoneNumber,
                Birthday: birthDay,
            }
            self.GetUser(self.ProductSearch);
        })

        $('body').on('click', '.btn-edit', function () {
            $(".user .modal-title").text("Chỉnh sửa thông tin người dùng");
            var id = $(this).attr('data-id');
            if (id !== null && id !== undefined) {
                self.GetUserById(id);
                $('#create').modal('show');
            }
        })

        $('.add-role').click(function () {
            $('#AddRole').modal('show');
        })

        $('body').on('click', '.btn-delete', function () {
            var id = $(this).attr('data-id');
            var fullname = $(this).attr('data-fullname');
            if (id !== null && id !== '') {
                self.confirmUser(fullname, id);
            }
        })
        $(".add-image").click(function () {
            $("#file-input").click();
        })

        $('body').on('click', '.btn-role-user', function () {
            var id = $(this).attr('data-id');
            $("#user_id").val(id);
            //self.GetAllRoles(id);           
        })

        $('body').on('click', '.btn-set-role', function () {
            var userId = parseInt($("#user_id").val());
            $.each($("#lst-role tr"), function (key, item) {
                var check = $(item).find('.ckRole').prop('checked');
                if (check == true) {
                    var id = parseInt($(item).find('.ckRole').val());
                    self.lstRole.push({
                        UserId: userId,
                        RoleId: id
                    });
                }
            })
            if (self.lstRole.length > 0) {
                self.SaveRoleForUser(self.lstRole, userId);
            }

        })

        $('.filesImages').on('change', function () {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;
            if (files != null && files.length > 0) {
                var fileExtension = ['jpeg', 'jpg', 'png'];
                var html = "";
                for (var i = 0; i < files.length; i++) {
                    if ($.inArray(files[i].type.split('/')[1].toLowerCase(), fileExtension) == -1) {
                        alert("Only formats are allowed : " + fileExtension.join(', '));
                    }
                    else {
                        //console.log(files[i]);
                        //self.FileImages.push(files[i]);
                        var src = URL.createObjectURL(files[i]);
                        html += "<div class=\"box-item-image\"> <div class=\"image-upload item-image\" style=\"background-image:url(" + src + ")\"></div></div>";
                    }
                }
                if (html != "") {
                    $(".image-default").hide();
                    $(".box-images").html(html);
                }
            }

        });
    }

    self.confirmUser = function (nameUser, id) {
        bootbox.confirm({
            message: '<div class="title-delete"><p>Bạn có chắc muốn xóa người dùng này?</p><p>' + nameUser + '</p></div>',
            centerVertical: true,
            buttons: {
                confirm: {
                    label: 'Đồng ý',
                    className: 'btn-success pull-left'
                },
                cancel: {
                    label: 'Hủy',
                    className: 'btn-danger '
                }
            },
            callback: function (result) {
                if (result === true) {
                    $.ajax({
                        type: "POST",
                        url: "/Admin/User/Delete",
                        dataType: "json",
                        data: {
                            id: id
                        },
                        beforeSend: function () {
                        },
                        complete: function () {
                        },
                        success: function (res) {
                            if (res.status) {
                                self.GetUser();
                                Notifly(res.message, "success");
                            }
                            else {
                                $.notify(res.message, 'error');
                            }
                        },
                        error: function () {
                        }
                    });
                }
            }
        });
    }

   

    self.AddImageAvatar = function () {
        var dataImage = new FormData();
        dataImage.append("image1", self.ProductImages);
        $.ajax({
            url: '/Admin/Product/UploadImageAvatar',
            type: 'POST',
            contentType: false,
            processData: false,
            data: dataImage,
            beforeSend: function () {
                //Loading('show');
            },
            complete: function () {
                //Loading('hiden');
            },
            success: function (response) {
                //if (response.success) {
                //    self.GetDataPaging(true);
                //    $('#_addUpdate').modal('hide');
                //}
            }
        })
    }


    //self.GetAllRoles = function (userId) {
    //    $.ajax({
    //        type: "GET",
    //        url: "/Admin/Role/GetAllRole",
    //        dataType: "json",
    //        beforeSend: function () {
    //        },
    //        complete: function () {
    //        },
    //        success: function (response) {

    //            if (response.Data !== null && response.Data.length > 0) {
    //                var data = response.Data;
    //                var html = "";
    //                $.each(data, function (key, item) {
    //                    html += '<tr data-id="' + item.Id + '">' +
    //                        "<td>" + item.Name + "</td>" +
    //                        "<td><label><input type='checkbox' value=" + item.Id + " class='ckRole'></label></td>" +
    //                        "</tr> ";
    //                })

    //                $("#lst-role").html(html);

    //                if (userId > 0) {
    //                    self.GetRoleByUserId(userId);
    //                }

    //            }

    //        },
    //        error: function () {
    //        }
    //    });
    //}

  

   
    // Set value default
    self.SetValueDefault = function () {
        self.Product.Id = null;
        $("#fullname").val("").attr("placeholder", "Nhập tên người dùng");
        $("#mobile").val("").attr("placeholder", "Nhập số điện thoại");
        $("#birthday").val("").attr("placeholder", "Ngày sinh");
        $("#email").val("").attr("placeholder", "Email");
        $("#username").val("").attr("placeholder", "Tên đăng nhập");
        $("#password").val("").attr("placeholder", "Mật khẩu");
        $("#address").val("").attr("placeholder", "Địa chỉ");
        $("#confirm_password").val("").attr("placeholder", "Nhập lại mật khẩu");
        $(".box-avatar").css("display", "none");
    }
    // Get User
   
    self.AddUser = function (userView) {
        $.ajax({
            url: '/Admin/Product/Add',
            type: 'POST',
            dataType: 'json',
            data: {
                ProductModelView: userView
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
                    window.location.href = '/admin/quan-ly-san-pham';
                }
                else {
                    if (response.isNameExist) {
                        tedu.notify('Tên đã tồn tại', 'error');
                        //$(".product-name-exist").show().text("Tên đã tồn tại");
                    }
                }
            }
        })
    }

    self.UpdateUser = function (userView) {
        $.ajax({
            url: '/Admin/Product/Update',
            type: 'POST',
            dataType: 'json',
            data: {
                ProductModelView: userView
            },
            beforeSend: function () {
                //Loading('show');
            },
            complete: function () {
                //Loading('hiden');
            },
            success: function (response) {
                if (response.success) {
                    tedu.notify('Cập nhật dữ liệu thành công', 'success');
                    self.GetDataPaging(true);
                    window.location.href = '/admin/quan-ly-san-pham';
                }
               
            }
        })
    }

    self.GetAllCategories = function () {
        $.ajax({
            url: '/Admin/Category/GetAll',
            type: 'GET',
            dataType: 'json',
            beforeSend: function () {
                Loading('show');
            },
            complete: function () {
                Loading('hiden');
            },
            success: function (response) {
                var html = "<option value =\"\">Chọn danh mục sản phẩm</option>";
                var htmlSearch = "<option value =\"\">Xem tất cả</option>"
                if (response.Data != null && response.Data.length > 0) {
                    for (var i = 0; i < response.Data.length; i++) {
                        var item = response.Data[i];
                        html += "<option value =" + item.id + ">" + item.name + "</option>";
                        htmlSearch += "<option value =" + item.id + ">" + item.name + "</option>";
                    }
                }
                $("#productcategoryid").html(html);
                $(".categorylist").html(htmlSearch);
            }
        })
    }

    self.GetTextFromHtml = function (html) {
        var dv = document.createElement("DIV");
        dv.innerHTML = html;
        return dv.textContent || dv.innerText || "";
    }  

    self.ValidateUser = function () {     

        jQuery.validator.addMethod("ckrequired", function (value, element) {
            var idname = $(element).attr('id');
            var editor = CKEDITOR.instances[idname];
            var ckValue = self.GetTextFromHtml(editor.getData()).replace(/<[^>]*>/gi, '').trim();
            if (ckValue.length === 0) {
                //if empty or trimmed value then remove extra spacing to current control  
                $(element).val(ckValue);
            } else {
                //If not empty then leave the value as it is  
                $(element).val(editor.getData());
            }
            return $(element).val().length > 0;
        }, "This field is required"); 


        $("#form-submit").validate({
            ignore: [],  
            rules:
            {
                productname: {
                    required: true,
                },
                productcategoryid: {
                    required: true,
                },
                producttypecolor: {
                    required: true
                },
                productnew: {
                    required: true
                },
                productquantity: {
                    required: true
                },
                productprice: {
                    required: true
                },
                productcolor: {
                    required: true
                },
                productquantity: {
                    required: true
                },
                //productavatar: {
                //    required: true
                //},
                productspecifications: {
                    /*required: true,*/
                    ckrequired: true,
                },
                //productendow: {
                //    required: true
                //},
                productshort_desc: {
                    required: true
                },
                productdescription: {
                    ckrequired: true
                }
                
            },
            messages:
            {
                productname: {
                    required: "Bạn chưa nhập tên sản phẩm",
                },
                productcategoryid: {
                    required: "Bạn chưa chọn danh mục sản phẩm",
                },
                producttypecolor: {
                    required: "Chọn màu sản phẩm"
                },
                productnew: {
                    required: "Bạn chưa chọn loại sản phẩm"
                },
                productquantity: {
                    required: "Bạn chưa nhập số sản phẩm"
                },
                productprice: {
                    required: "Bạn chưa nhập giá sản phẩm"
                },
                productcolor: {
                    required: "Bạn chưa nhập màu sản phẩm"
                },
                //productavatar: {
                //    required: "Bạn chưa chọn ảnh sản phẩm"
                //},
                productspecifications: {
                   /* required: "Bạn chưa nhập thông số kỹ thuật",*/
                    ckrequired: "Bạn chưa nhập thông số kỹ thuật",
                },
                //productendow: {
                //    required: "Bạn chưa nhập ưu đãi"
                //},
                productshort_desc: {
                    required: "Bạn chưa nhập mô tả ngắn"
                },
                productdescription: {
                    ckrequired: "Bạn chưa nhập mô tả sản phẩm"
                }
            },
            submitHandler: function (form) {
                //debugger
                //var description = CKEDITOR.instances.productdescription.getData();
                //if (description == null || description == "" || description == undefined) {
                //    swal("", "Vui lòng nhập mô ta chi tiết", "warning");
                //    return;
                //}
                //else if (description.length < 25) {
                //    swal("", "Mô tả tối thiểu 25 kí tự", "warning");
                //    return;
                //}

                self.GetValue();
                if (self.IsUpdate) {
                    self.UpdateUser(self.Product);
                }
                else {                    
                    self.AddUser(self.Product);
                }

                if (self.ProductImages != null && self.ProductImages != "") {
                    self.UploadFileImageProduct();
                }
            }
        });
    }

    self.GetValue = function () {
        self.Product.name = $("#productname").val();
        self.Product.category_id = $("#productcategoryid").val();
        
        if (self.ProductImages != null && self.ProductImages.name != null && self.ProductImages.name != "") {
            self.Product.avatar = self.ProductImages.name;
        }
        self.Product.price = $("#productprice").val();
        self.Product.color = $("#productcolor").val();
        self.Product.quantity = $("#productquantity").val();
        self.Product.short_desc = $("#productshort_desc").val();
        self.Product.description = CKEDITOR.instances.productdescription.getData();
        self.Product.specifications = CKEDITOR.instances.productspecifications.getData();  //$("#productspecifications").val();
        self.Product.endow = $("#productendow").val();
        self.Product.differentiate = $("#productnew").val();
    }

    self.RenderHtmlByObject = function (view) {
        $("#productname").val(view.name);
        $("#productcategoryid").val(view.category_id);
        $("#productprice").val(view.price);
        $("#productcolor").val(view.color);
        if (view.color != null && view.color != "") {
            $("#producttypecolor").val(0);
        }
        else {
            $("#producttypecolor").val(1);
        }
        self.Product.avatar = view.avatar;
        $("#productnew").val(view.differentiate);
        $("#productquantity").val(view.quantity);
        $("#productshort_desc").val(view.short_desc);
        CKEDITOR.instances.productdescription.setData(view.description);
        CKEDITOR.instances.productspecifications.setData(view.specifications);
        /*$("#productspecifications").val(view.specifications);*/
        $("#productendow").val(view.endow);
        $(".box-image").css({ "background-image": "url('/product-image/" + view.avatar + "')", "display": "block" });  
    }

    self.UploadFileImageProduct = function () {
        var dataImage = new FormData();
        dataImage.append(0, self.ProductImages);

        $.ajax({
            url: '/Admin/Product/UploadImageProduct',
            type: 'POST',
            contentType: false,
            processData: false,
            data: dataImage,
            beforeSend: function () {
                //Loading('show');
            },
            complete: function () {
                //Loading('hiden');
            },
            success: function (response) {
                //if (response.success) {
                //    self.GetDataPaging(true);
                //    $('#_addUpdate').modal('hide');
                //}
            }
        })
    }


    $(document).ready(function () {

        self.GetDataPaging();

        self.ValidateUser();

        self.GetAllCategories();

        CKEDITOR.replace('productdescription', {});
        CKEDITOR.replace('productspecifications', {});

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
            self.ProductSearch.name = null;
            self.ProductSearch.categoryId = $(this).val();
            self.GetDataPaging(true);
        });

        $('#ddlShowPage').on('change', function () {
            tedu.configs.pageSize = $(this).val();
            tedu.configs.pageIndex = 1;
            self.GetDataPaging(true);
        });

        $('input.form-search').on('input', function (e) {
            self.ProductSearch.name = $(this).val();
            self.GetDataPaging(true);
        });

        
        $('#productavatar').on('change', function () {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;
            if (files != null && files.length > 0) {
                var fileExtension = ['jpeg', 'jpg', 'png'];
                var html = "";
                for (var i = 0; i < files.length; i++) {
                    if ($.inArray(files[i].type.split('/')[1].toLowerCase(), fileExtension) == -1) {
                        alert("Only formats are allowed : " + fileExtension.join(', '));
                    }
                    else {
                        self.ProductImages = files[i];
                        var src = URL.createObjectURL(files[i]);
                        console.log(self.ProductImages);
                        $(".box-image").css({ "background-image":"url('" + src + "')","display":"block" });                        
                    }
                }
            }

        });
        $(".btn-back").click(function () {
            window.location.reload();
        })
        //$(".btn-add-product").click(function () {
          
        //})

       
    })
})(jQuery);