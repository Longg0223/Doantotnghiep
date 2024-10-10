(function ($) {
    var self = this;
    self.Data = [];
    self.IsUpdate = false;    
    self.Order = {
        id: null,
        code :"",
        user_id :"",
        note :"",
        review: "",
        payment: "",
        status: "",
        total: "",
        fee_ship: "",
        created_at: "",
        customer_name:""
    }
    self.OrderSearch = {
        name: "",
        payment: null,
        PageIndex: tedu.configs.pageIndex,
        PageSize: tedu.configs.pageSize
    }

    self.RenderTableHtml = function (data) {
        var html = "";
        if (data != "" && data.length > 0) {
            var index = 0;
            for (var i = 0; i < data.length; i++) {
                var item = data[i];

                var htmlSelect = "";
                if (item.status == 0) {
                    htmlSelect = "<select  class='form-select btn-outline-success ' onChange=UpdateStatus(" + item.id + ",this)>" +
                        "<option  value = '0' selected> Đang chờ xử lý</option>" +
                        "<option  value='1'>Đã hoàn thành</option>" +
                        "<option  value='2'>Đã huỷ</option></select>";
                }
                else if (item.status == 1) {
                    htmlSelect = "<select  class='form-select btn-outline-secondary ' onChange=UpdateStatus(" + item.id + ",this)>" +
                        "<option  value = '0' selected> Đang chờ xử lý</option>" +
                        "<option  value='1' selected>Đã hoàn thành</option>" +
                        "<option  value='2'>Đã huỷ</option></select>";
                }
                else {
                    htmlSelect = "<select  class='form-select btn-outline-danger ' onChange=UpdateStatus(" + item.id + ",this)>" +
                        "<option  value = '0' selected> Đang chờ xử lý</option>" +
                        "<option  value='1' selected>Đã hoàn thành</option>" +
                        "<option  value='2' selected>Đã huỷ</option></select>";
                }

                html += "<tr>";
                html += "<td>" + (++index) + "</td>";
                /*html += "<td><a href=\"javascript: void (0)\" onClick=DetailOrder(" + item.id + ")>" + item.code + "</td>";*/
                html += "<td><a href=\"javascript: void (0)\">" + item.code + "</td>";
                html += "<td>" + item.full_name + "</td>";
                html += "<td>" + item.totalstr + "</td>";
                html += "<td>" + item.paymentstr + "</td>"; 
                html += "<td>" + item.created_atstr + "</td>"; 
                html += "<td style=\"text-align: center;\">" +
                    htmlSelect
                   +
                    "</td>";
                
                html += "</tr>";
            }
        }
        else {
            html += "<tr><td colspan=\"10\" style=\"text-align:center\">Không có dữ liệu</td></tr>";
        }
        $("#tblData").html(html);
    };
    self.DetailOrder = function (id) {
        window.location.href = "/admin/chi-tiet-don-hang/" + id;
    }
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
                url: '/Admin/Orders/GetById',
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

    self.UpdateStatus = function (id, tag) {
        var status = $(tag).val();
        $.ajax({
            url: '/Admin/Orders/UpdateStatus',
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
                    url: "/Admin/Orders/Delete",
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

        self.OrderSearch.PageIndex = tedu.configs.pageIndex;
        self.OrderSearch.PageSize = tedu.configs.pageSize;

        $.ajax({
            url: '/Admin/Orders/GetAllPaging',
            type: 'GET',
            data: self.OrderSearch,
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
            url: '/Admin/Orders/Add',
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
                        $(".product-name-exist").show().text("Phone đã tồn tại");
                    }
                }
            }
        })
    }

    self.UpdateUser = function (userView) {
        $.ajax({
            url: '/Admin/Orders/Update',
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

    self.ValidateUser = function () {     

        $("#form-submit").validate({
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
                    required: true
                },
                productendow: {
                    required: true
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
                    required: "Bạn chưa nhập thông số kỹ thuật"
                },
                productendow: {
                    required: "Bạn chưa nhập ưu đãi"
                },
                productshort_desc: {
                    required: "Bạn chưa nhập mô tả ngắn"
                },
                productdescription: {
                    required: "Bạn chưa nhập mô tả sản phẩm"
                }
            },
            submitHandler: function (form) {               
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
        self.Product.specifications = $("#productspecifications").val();
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
        $("#productspecifications").val(view.specifications);
        $("#productendow").val(view.endow);
        $(".box-image").css({ "background-image": "url('/product-image/" + view.avatar + "')", "display": "block" });  
    }



    $(document).ready(function () {

        self.GetDataPaging();

       // self.ValidateUser();

      
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
            self.OrderSearch.name = null;
            self.OrderSearch.status = $(this).val();
            self.GetDataPaging(true);
        });

        $('#ddlShowPage').on('change', function () {
            tedu.configs.pageSize = $(this).val();
            tedu.configs.pageIndex = 1;
            self.GetDataPaging(true);
        });

        $('input.form-search').on('input', function (e) {
            self.OrderSearch.name = $(this).val();
            self.GetDataPaging(true);
        });

        
       
    })
})(jQuery);