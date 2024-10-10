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
    self.ReviewSearch = {
        name: "",
        PageIndex: tedu.configs.pageIndex,
        PageSize: tedu.configs.pageSize
    }

    self.Files = {};

    self.RenderTableHtml = function (data) {
        self.Data = data;
        var html = "";
        if (data != "" && data.length > 0) {
            var index = 0;
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                var starHtml = "<div>";
                if (item.star != null && item.star > 0) {
                    for (var j = 1; j <= 5; j++) {
                        if (j <= item.star) {
                            starHtml += "</span><span >&nbsp;<i  class=\"bi bi-star-fill star-color\"></i></span>";
                        }
                        else {
                            starHtml += "<span ><i data-v-c39dc58e class='bi bi-star star-color'></i>&nbsp;</span>";
                        }

                    }
                }
                else {
                    starHtml += "Chưa có đánh giá nào";
                }
                starHtml += "<div/>";

                html += "<tr>";
                html += "<td>" + (++index) + "</td>";
                html += "<td><a href=\"javascript:void(0)\" onClick=ShowDetail(" + item.id + ")>" + item.ordersModelView.code + "</td>";
                html += "<td>" + item.userModelView.full_name + "</td>";
                html += "<td>" + item.userModelView.phone_number + "</td>";
                html += "<td>" + item.create_atstr + "</td>";     
                html += "<td>" + starHtml + "</td>";        
                html += "<td style=\"text-align: center;\">" +

                    (item.status == 0 ? "<button  class=\"btn btn-dark custom-button\" onClick=UpdateStatus(" + item.id + ",1)><i class=\"bi bi-eye\"></i></button>" : "<button  class=\"btn btn-secondary custom-button\" onClick=UpdateStatus(" + item.id + ",0)><i class=\"bi bi-eye-slash\"></i></button>")  +                    
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
    self.ShowDetail = function (id) {
        var _data = self.Data.find(p => p.id == id);
        if (_data != null) {
            $("#DetailModal .full_name").text(_data.userModelView.full_name);
            $("#DetailModal .phone").text(_data.userModelView.phone_number);
            $("#DetailModal .date_str").text(_data.create_atstr);
            $("#DetailModal .comment").text(_data.comment != null ? _data.comment : "");
            $("#DetailModal").modal('show');
        }
        /*console.log(_data);*/
    }
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
                url: '/Admin/Reviews/GetById',
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
            url: '/Admin/Reviews/UpdateStatus',
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
            tedu.confirm('Bạn có chắc muốn đánh giá này?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Reviews/Delete",
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

        self.ReviewSearch.PageIndex = tedu.configs.pageIndex;
        self.ReviewSearch.PageSize = tedu.configs.pageSize;

        $.ajax({
            url: '/Admin/Reviews/GetAllPaging',
            type: 'GET',
            data: self.ReviewSearch,
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
            url: '/Admin/Reviews/Add',
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
                    tedu.notify('Thêm mới dữ liệu thành công', 'success');
                    self.GetDataPaging(true);
                    window.location.href = '/admin/quan-ly-bai-viet';
                }
                else {
                    if (response.isNameExist) {
                        $(".product-name-exist").show().text("Phone đã tồn tại");
                    }
                }
            }
        })
    }

    self.UpdateUser = function (userView) {
        $.ajax({
            url: '/Admin/Reviews/Update',
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

    self.ValidateUser = function () {     

        $("#form-submit").validate({
            rules:
            {
                productname: {
                    required: true,
                },   
                productshort_desc: {
                    required: true
                },
                productdescription: {
                    required: true
                }
                
            },
            messages:
            {
                productname: {
                    required: "Bạn chưa nhập tiêu đề",
                },               
                productshort_desc: {
                    required: "Bạn chưa nhập mô tả ngắn"
                },
                productdescription: {
                    required: "Bạn chưa nhập mô tả"
                }
            },
            submitHandler: function (form) {               
                self.GetValue();

                if (self.IsUpdate) {
                    self.UpdateUser(self.Post);
                }
                else {                    
                    self.AddUser(self.Post);
                }

                if (self.PostImages != null && self.PostImages != "") {
                    self.UploadFileImageProduct();
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
            url: '/Admin/Reviews/UploadImageProduct',
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

        /*self.ValidateUser();*/

       /* self.GetAllCategories();*/

        /*CKEDITOR.replace('productdescription', {});*/

        $(".modal").on("hidden.bs.modal", function () {
            $(this).find('form').trigger('reset');
            /*$("form").validate().resetForm();*/
            $("label.error").hide();
            $(".error").removeClass("error");
        });

        //$(".btn-addorupdate").click(function () {
        //    $(".custom-format").removeAttr("disabled");
        //    $("#titleModal").text("Thêm mới tài khoản");
        //    $(".txtPassword").show();
        //    $(".btn-submit-format").text("Thêm mới");
        //    self.IsUpdate = false;
        //    $('#userModal').modal('show');
        //})
        $('#select-right').on('change', function () {
            $('input.form-search').val("");
            self.ReviewSearch.name = null;
            self.ReviewSearch.star = $(this).val();
            self.GetDataPaging(true);
        });

        $('#ddlShowPage').on('change', function () {
            tedu.configs.pageSize = $(this).val();
            tedu.configs.pageIndex = 1;
            self.GetDataPaging(true);
        });

        $('input.form-search').on('input', function (e) {
            self.ReviewSearch.name = $(this).val();
            self.GetDataPaging(true);
        });

        
        //$('#productavatar').on('change', function () {
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
        //                self.PostImages = files[i];
        //                var src = URL.createObjectURL(files[i]);
        //                console.log(self.PostImages);
        //                $(".box-image").css({ "background-image":"url('" + src + "')","display":"block" });                        
        //            }
        //        }
        //    }

        //});
        //$(".btn-back").click(function () {
        //    window.location.reload();
        //})
        //$(".btn-add-product").click(function () {
          
        //})

       
    })
})(jQuery);