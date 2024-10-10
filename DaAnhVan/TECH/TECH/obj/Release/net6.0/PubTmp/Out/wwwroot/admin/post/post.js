(function ($) {
    var self = this;
    self.Data = [];
    self.PostImages = {};
    self.IsUpdate = false;    
    self.Post = {
        id: null,
        title: null,
        avatar: "",
        short_content: "",
        content: "",
        status: "",
    }
    self.PostSearch = {
        name: "",
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
                //var htmlSelect = "";
                //if (item.status == 0) {
                //    htmlSelect = "<select  class='form-select btn-outline-success ' onChange=update(" + item.id + ",this)>" +
                //        "<option  value = '0' selected> Đang chờ xử lý</option>" +
                //        "<option  value='1'>Đã hoàn thành</option>" +
                //        "<option  value='2'>Đã huỷ</option></select>";
                //}
                //else if (item.status == 1) {
                //    htmlSelect = "<select  class='form-select btn-outline-secondary ' onChange=update(" + item.id + ",this)>" +
                //        "<option  value = '0' selected> Đang chờ xử lý</option>" +
                //        "<option  value='1' selected>Đã hoàn thành</option>" +
                //        "<option  value='2'>Đã huỷ</option></select>";
                //}
                //else {
                //    htmlSelect = "<select  class='form-select btn-outline-danger ' onChange=update(" + item.id + ",this)>" +
                //        "<option  value = '0' selected> Đang chờ xử lý</option>" +
                //        "<option  value='1' selected>Đã hoàn thành</option>" +
                //        "<option  value='2' selected>Đã huỷ</option></select>";
                //}



                html += "<tr>";
                html += "<td>" + (++index) + "</td>";
                html += "<td> <img src=/product-post/" + item.avatar + " class=\"item-image\" /></td>";
                html += "<td>" + item.title + "</td>";
                html += "<td>" + item.author_name + "</td>";
                html += "<td>" + item.create_atstr + "</td>";      
                //html += "<td>" + htmlSelect + "</td>";      
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
            $("#titleModal").text("Cập nhật tài khoản");
            $(".btn-submit-format").text("Cập nhật");
            self.GetById(id, self.RenderHtmlByObject);
            self.Post.id = id;

            $(".product-add-update").show();
            $(".product-list").hide();
            self.IsUpdate = true;
        }
    }

    self.GetById = function (id, renderCallBack) {
        //self.PostData = {};
        if (id != null && id != "") {
            $.ajax({
                url: '/Admin/Post/GetById',
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
            url: '/Admin/Post/UpdateStatus',
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
            tedu.confirm('Bạn có chắc muốn xóa bài viết này?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Post/Delete",
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

        self.PostSearch.PageIndex = tedu.configs.pageIndex;
        self.PostSearch.PageSize = tedu.configs.pageSize;

        $.ajax({
            url: '/Admin/Post/GetAllPaging',
            type: 'GET',
            data: self.PostSearch,
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
                else {
                    $("#paginationUL").hide();
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
            self.Post.Id = 0;
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
                    self.PostImages = files;
                    //console.log(self.PostImages);
                }
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

   

   
    self.AddUser = function (userView) {
        $.ajax({
            url: '/Admin/Post/Add',
            type: 'POST',
            dataType: 'json',
            data: {
                PostModelView: userView
            },
            beforeSend: function () {
                Loading('show');
            },
            complete: function () {
                Loading('hiden');
            },
            success: function (response) {
                if (response.success) {
                    tedu.notify('Thêm mới dữ liệu thành công', 'success');
                    self.GetDataPaging(true);
                    window.location.href = '/admin/quan-ly-bai-viet';
                }
                else {
                    if (response.isNameExist) {
                        $(".product-name-exist").show().text("Tên tiêu đề đã tồn tại");
                    }
                }
            }
        })
    }

    self.UpdateUser = function (userView) {
        $.ajax({
            url: '/Admin/Post/Update',
            type: 'POST',
            dataType: 'json',
            data: {
                PostModelView: userView
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
                    window.location.href = '/admin/quan-ly-bai-viet';
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
                    minlength: 5
                },   
                productshort_desc: {
                    required: true,
                    minlength: 5
                },
                productdescription: {
                    ckrequired: true
                }
              
            },
            messages:
            {
                productname: {
                    required: "Bạn chưa nhập tiêu đề",
                    minlength: "Tiêu đề tối thiểu 5 kí tự"
                },               
                productshort_desc: {
                    required: "Bạn chưa nhập mô tả ngắn",
                    minlength: "Mô tả ngắn tối thiểu 5 kí tự"
                },
                productdescription: {
                    ckrequired: "Bạn chưa nhập mô tả bài viết"
                }
            },
            submitHandler: function (form) {               
                self.GetValue();
                //var description = CKEDITOR.instances.productdescription.getData();
                //if (description == null || description == "") {
                //    swal("", "Vui lòng nhập mô ta chi tiết", "warning");
                //    return;
                //}
                //else if (description.length < 25) {
                //    swal("", "Mô tả tối thiểu 25 kí tự", "warning");
                //    return;
                //}

                if (self.IsUpdate) {                   
                    self.UpdateUser(self.Post);
                    if (self.PostImages != null && self.PostImages != "") {
                        self.UploadFileImageProduct();
                    }
                    
                }
                else {
                    //console.log(self.PostImages);
                    if ($.isEmptyObject(self.PostImages)) {
                        swal("", "Vui lòng chọn hình ảnh", "warning");
                    } else {
                        self.AddUser(self.Post);
                    }
                }

                
            }
        });
    }

    self.GetValue = function () {
        self.Post.title = $("#productname").val();               
        self.Post.short_content = $("#productshort_desc").val();
        if (self.PostImages != null && self.PostImages.name != null && self.PostImages.name != "") {
            self.Post.avatar = self.PostImages.name;
        }

        self.Post.content = CKEDITOR.instances.productdescription.getData();       
    }

    self.RenderHtmlByObject = function (view) {
        $("#productname").val(view.title);
        self.Post.avatar = view.avatar;
        $("#productshort_desc").val(view.short_content);
        CKEDITOR.instances.productdescription.setData(view.content);
        $(".box-image").css({ "background-image": "url('/product-post/" + view.avatar + "')", "display": "block" });  
    }

    self.UploadFileImageProduct = function () {
        var dataImage = new FormData();
        dataImage.append(0, self.PostImages);

        $.ajax({
            url: '/Admin/Post/UploadImageProduct',
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
            self.PostSearch.name = null;
            self.PostSearch.categoryId = $(this).val();
            self.GetDataPaging(true);
        });

        $('#ddlShowPage').on('change', function () {
            tedu.configs.pageSize = $(this).val();
            tedu.configs.pageIndex = 1;
            self.GetDataPaging(true);
        });

        $('input.form-search').on('input', function (e) {
            self.PostSearch.name = $(this).val();
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
                        self.PostImages = files[i];
                        var src = URL.createObjectURL(files[i]);
                        console.log(self.PostImages);
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