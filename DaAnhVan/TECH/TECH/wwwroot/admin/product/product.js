(function ($) {
    var self = this;
    self.Data = [];
    self.ProductImages = [];
    self.ProductServerImages = [];
    self.ProductUpdateImage = [];
   
    self.IsUpdate = false;
    // quantity start
    self.ProductIdQuantity = 0;
    self.Sizes = [36,37,38,39,40,41,42,43,44,45];
    self.ListUpdateQuantity = [];
    self.ListDeletedQuantity = [];
    self.Colors = [];
    // quantity end
    self.Product = {
        id: null,
        name: null,
        category_id: "",
        avatar: "",
        price: "",
        //color: "",
        quantity: "",
        short_desc: "",
        description: "",
        specifications: "",
        shortdescription:"",
        trademarkId:0,
        status: "",
        endow: "",
        type: "",
        differentiate: "",
        number_import: 0,
        cover_type: null
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
                if (item.avatar != null) {
                    html += "<td> <img src=/product-image/" + item.avatar + " class=\"item-image\" /></td>";
                } else {
                    html += "<td> <img src=/image-default/default.png class=\"item-image\" /></td>";
                }
                html += "<td>" + item.name + "</td>";
                html += "<td>" + item.categorystr + "</td>";
                html += "<td>" + item.trademarkStr + "</td>";
                html += "<td>" + item.price_import_str + "</td>";
                html += "<td>" + item.price_sell_str + "</td>";               
                html += "<td>" + item.price_reduced_str + "</td>";
                html += "<td>" + item.number_import + "</td>";
                //html += "<td>" + item.cover_type_str + "</td>";
                //html += "<td>" + item.price_reduced_str + "</td>";
                html += "<td style=\"text-align: center;\">" +

                    (item.ishidden == 0 ? "<button  class=\"btn btn-dark custom-button\" onClick=UpdateStatus(" + item.id + ",1)><i class=\"bi bi-eye custom-icon\"></i></button>" : "<button  class=\"btn btn-secondary custom-button\" onClick=UpdateStatus(" + item.id + ",0)><i class=\"bi bi-eye-slash custom-icon\"></i></button>") +
                    "<button  class=\"btn btn-primary custom-button\" onClick=\"UpdateView(" + item.id + ")\"><i  class=\"bi bi-pencil-square custom-icon\"></i></button>" +
                    //"<button  class=\"btn btn-success custom-button\" onClick=\"Quantity(" + item.id + ")\"><i  class=\"bi bi-calculator custom-icon\"></i></button>" +
                    "<button  class=\"btn btn-danger custom-button\" onClick=\"Deleted(" + item.id + ")\"><i  class=\"bi bi-trash custom-icon\"></i></button>" +

                    "</td>";

                html += "</tr>";
            }
        }
        else {
            html += "<tr><td colspan=\"10\" style=\"text-align:center\">Không có dữ liệu</td></tr>";
        }
        $("#tblData").html(html);
    };

    // Quantity start
    self.InitQuantity = function () {
        /*self.GetColor();*/
        //self.GetSize();
        $(".btn-create-row-quantity").click(function () {
            var html = self.AddRowHtml();
            $("#quantity #tblData").append(html);
            $(".no-data").hide();
        });
        $("#form-submit-quantity").on("submit", function (e) {
            e.preventDefault();
            var lstAddQuantity = self.GetQuantityView("new");
            if (lstAddQuantity != null && lstAddQuantity.length > 0) {
                self.AddQuantity(lstAddQuantity);
            }
            if (self.ListUpdateQuantity != null && self.ListUpdateQuantity.length > 0) {
                var lstUpdateQuantity = [];
                for (var i = 0; i < self.ListUpdateQuantity.length; i++) {
                    var item = self.ListUpdateQuantity[i];
                    var quantityView = self.GetQuantityView(item.toString());
                    if (quantityView != null) {
                        lstUpdateQuantity.push(quantityView);
                    }
                }
                if (lstUpdateQuantity != null && lstUpdateQuantity.length > 0) {
                    console.log(lstUpdateQuantity);
                    self.UpdateServerQuantity(lstUpdateQuantity);
                }                
            }
            if (self.ListDeletedQuantity != null && self.ListDeletedQuantity.length > 0) {
                self.DeletedServerQuantity(self.ListDeletedQuantity);
            }
        })
    }
    self.GetQuantityView = function (className) {
        if (className == "new") {
            var lstAddQuantity = [];
            var classNameCustom = "#quantity #tblData " + "." + className;
            $(classNameCustom).each(function () {
                var id = $(this).attr("data-quantity");
                if (id != null && id != "") {
                    id = parseInt(id);
                }
                var color = $(this).find(".colors").val();
                if (color != null && color != "") {
                    color = parseInt(color);
                }
                var size = $(this).find(".sizes").val();
                if (size != null && size != "") {
                    size = parseInt(size);
                }
                var totalimport = $(this).find(".totalimport").val();
                if (totalimport != null && totalimport != "") {
                    totalimport = parseInt(totalimport);
                }
                lstAddQuantity.push({
                    Id: id,
                    AppSizeId: size,
                    ColorId: color,
                    TotalImported: totalimport,
                    ProductId: self.ProductIdQuantity
                });
            });
            return lstAddQuantity;
        }
        else {
            var lstAddQuantity = {
                Id: 0,
                AppSizeId: 0,
                ColorId: 0,
                TotalImported: 0,
                ProductId: 0
            };
            var classNameCustom = "#quantity #tblData " + "." + className;
            var id = $(classNameCustom).attr("data-quantity");
            if (id != null && id != "") {
                id = parseInt(id);
            }
            var color = $(classNameCustom).find(".colors").val();
            if (color != null && color != "") {
                color = parseInt(color);
            }
            var size = $(classNameCustom).find(".sizes").val();
            if (size != null && size != "") {
                size = parseInt(size);
            }
            var totalimport = $(classNameCustom).find(".totalimport").val();
            if (totalimport != null && totalimport != "") {
                totalimport = parseInt(totalimport);
            }
            lstAddQuantity={
                Id: id,
                AppSizeId: size,
                ColorId: color,
                TotalImported: totalimport,
                ProductId: self.ProductIdQuantity
            };
            return lstAddQuantity;
        }
       
    }
    self.AddRowHtml = function () {
        var index = 0;
        var html = "<tr class=\"new\">";
        html += "<td>" + (++index) + "</td>";
       /* html += "<td>" + self.GetColorByIdOrAll(0) + "</td>";*/
        html += "<td>" + self.GetSizeByIdOrAll(0) + "</td>";
        html += "<td><input type=\"number\" class=\"form-control totalimport\" min=\"0\" required></td>";
        //html += "<td></td>";
        html += "<td style=\"text-align: center;\">" +
            /* "<button  class=\"btn btn-primary custom-button\" onClick=\"UpdateView()\"><i  class=\"bi bi-pencil-square custom-icon\"></i></button>" +*/
            "<button  class=\"btn btn-danger custom-button\" onClick=\"DeletedHtml(this)\"><i  class=\"bi bi-trash custom-icon\"></i></button>" +
            "</td>";
        html += "</tr>";
        return html;
    }
    self.DeletedHtml = function (tag) {
        $(tag).closest(".new").remove();
    }
    self.GetProductQuantityForProductId = function (productId) {
        $.ajax({
            url: '/Admin/ProductQuantity/GetProductQuantityForProductId',
            type: 'GET',
            data: {
                productId: productId
            },
            dataType: 'json',
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                self.ProductQuantityRenderTableHtml(response.Data);
            }
        });
    };

    self.ProductQuantityRenderTableHtml = function (data) {
        var html = "";
        if (data != "" && data.length > 0) {
            var index = 0;
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                html += "<tr data-quantity=" + item.Id + " class=" + item.Id +">";
                html += "<td>" + (++index) + "</td>";
               /* html += "<td>" + self.GetColorByIdOrAll(item.ColorId,true) + "</td>";*/
                html += "<td>" + self.GetSizeByIdOrAll(item.AppSizeId,true) + "</td>";
                html += "<td><input type=\"number\" class=\"form-control totalimport\" min=\"0\" required value=" + item.TotalImported + " disabled></td>";
                //html += "<td>" + (item.TotalSell != null ? item.TotalSell:"") + "</td>";
                html += "<td style=\"text-align: center;\">"+
                    "<button type=\"button\" class=\"btn btn-primary custom-button\" onClick=\"UpdateQuantity(" + item.Id + ")\"><i  class=\"bi bi-pencil-square custom-icon\"></i></button>" +
                    "<button type=\"button\" class=\"btn btn-danger custom-button\" onClick=\"DeletedQuantity(" + item.Id + ")\"><i  class=\"bi bi-trash custom-icon\"></i></button>" +
                    "</td>";
                html += "</tr>";
            }
        }
        else {
            html += "<tr class=\"no-data\"><td colspan=\"10\" style=\"text-align:center\">Không có dữ liệu</td></tr>";
        }
        $("#quantity #tblData").html(html);
        $('#quantity').modal('show');
    };
    self.UpdateQuantity = function (id) {
        if (id > 0) {
            self.ListUpdateQuantity.push(id);
            var classNameSelect = "." + id.toString() + " select";
            var classNameInput = "." + id.toString() + " .totalimport";
            var className = classNameSelect + "," + classNameInput;
            $(className).removeAttr('disabled');
        } 
    }
    self.DeletedQuantity = function (id) {
        if (id > 0) {
            self.ListDeletedQuantity.push(id);
            var className = "." + id.toString();
            $(className).remove();
        }
    }
    self.GetColorByIdOrAll = function (colorId, isRenderAPI) {
        var html = "";
        if (isRenderAPI) {
            html = "<select class=\"form-select colors\" required disabled><option value=\"\"> Chọn màu </option>";
        }
        else {
            html = "<select class=\"form-select colors\" required><option value=\"\"> Chọn màu </option>";
        }
        if (self.Colors != null && self.Colors.length > 0) {
            for (var i = 0; i < self.Colors.length; i++) {
                var item = self.Colors[i];
                if (item.id == colorId) {
                    html += "<option value=\"" + item.id + "\" selected>" + (item.name != "" ? item.name : (item.code != "" ? item.code : "")) + "</option>";
                }
                else {
                    html += "<option value=\"" + item.id + "\">" + (item.name != "" ? item.name : (item.code != "" ? item.code : "")) + "</option>";
                }
            }
        }
        html += "</select>";
        return html;
    }

    self.GetSizeByIdOrAll = function (sizeId, isRenderAPI) {
        var html = "";
        if (isRenderAPI) {
            html = "<select class=\"form-select sizes\" required disabled><option value=\"\"> Chọn kích thước </option>";
        }
        else {
            html = "<select class=\"form-select sizes\" required><option value=\"\"> Chọn kích thước </option>";
        }
        if (self.Sizes != null && self.Sizes.length > 0) {
            for (var i = 0; i < self.Sizes.length; i++) {
                var item = self.Sizes[i];
                if (item.id == sizeId) {
                    html += "<option value=\"" + item.id + "\" selected>" + item.name + "</option>";
                }
                else {
                    html += "<option value=\"" + item.id + "\">" + item.name + "</option>";
                }
            }
        }
        html += "</select>";
        return html;
    }

    //self.GetColor = function () {
    //    $.ajax({
    //        url: '/Admin/Colors/GetAll',
    //        type: 'GET',
    //        dataType: 'json',
    //        beforeSend: function () {
    //        },
    //        complete: function () {
    //        },
    //        success: function (response) {

    //            if (response.Data != null && response.Data.length > 0) {
    //                self.Colors = response.Data;
    //            }
    //        }
    //    });
    //};

    //self.GetSize = function () {
    //    $.ajax({
    //        url: '/Admin/Sizes/GetAll',
    //        type: 'GET',
    //        dataType: 'json',
    //        beforeSend: function () {
    //        },
    //        complete: function () {
    //        },
    //        success: function (response) {
    //            if (response.Data != null && response.Data.length > 0) {
    //                self.Sizes = response.Data;
    //            }
    //        }
    //    });

    //};

    self.AddQuantity = function (_quantities) {
        $.ajax({
            url: '/Admin/ProductQuantity/Add',
            type: 'POST',
            dataType: 'json',
            data: {
                quantities: _quantities
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
                    if (self.ProductIdQuantity > 0) {
                        self.Quantity(self.ProductIdQuantity);
                    }
                   /* window.location.href = '/admin/quan-ly-san-pham';*/
                }
                else {
                    tedu.notify('Thêm mới dữ liệu không thành công', 'error');
                }
            }
        })
    }
    self.UpdateServerQuantity = function (_quantities) {
        $.ajax({
            url: '/Admin/ProductQuantity/Update',
            type: 'POST',
            dataType: 'json',
            data: {
                quantities: _quantities
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
                    if (self.ProductIdQuantity > 0) {
                        self.Quantity(self.ProductIdQuantity);
                    }                   
                    //window.location.href = '/admin/quan-ly-san-pham';
                }
                else {
                    tedu.notify('Cập nhật dữ liệu không thành công', 'error');
                }
            }
        })
    }
    self.DeletedServerQuantity = function (quantities) {
        $.ajax({
            url: '/Admin/ProductQuantity/Deleted',
            type: 'POST',
            dataType: 'json',
            data: {
                ids: quantities
            },
            beforeSend: function () {
                //Loading('show');
            },
            complete: function () {
                //Loading('hiden');
            },
            success: function (response) {
                if (response.success) {
                    tedu.notify('Xóa dữ liệu thành công', 'success');
                    //window.location.href = '/admin/quan-ly-san-pham';
                }
                else {
                    tedu.notify('Cập nhật dữ liệu không thành công', 'error');
                }
            }
        })
    }
    // Quantity end

    self.UpdateView = function (id) {
        if (id != null && id != "") {
            //$(".btn-submit-format").text("Cập nhật");
            //$(".product-title").text("Cập nhật sản phẩm");
            $(".custom-format").attr("disabled", "disabled");
            self.GetById(id, self.RenderHtmlByObject);
            self.Product.id = id;

            $(".product-update").show();
            $(".product-list").hide();
            self.IsUpdate = true;
        }
    }
    self.Quantity = function (id) {
        self.GetProductQuantityForProductId(id);
        self.ProductIdQuantity = id;
    }

    self.GetById = function (id, renderCallBack) {
        if (id != null && id != "") {
            $.ajax({
                url: '/Admin/Product/GetById',
                type: 'GET',
                dataType: 'json',
                data: {
                    id: id
                },
                beforeSend: function () {
                },
                complete: function () {
                },
                success: function (response) {
                    if (response.Data != null) {
                        renderCallBack(response.Data);
                        self.Id = id;

                    }
                }
            })
        }
    }

    self.UpdateStatus = function (id, status) {
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

    self.AddProduct = function (userView) {
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
                    if (self.ProductImages != null && self.ProductImages != "") {
                        self.UploadFileImageProduct(response.id);
                    }
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

    self.Update = function (userView) {
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
                producttrademark: {
                    required: true
                },
                productstatus: {
                    required: true
                },
                productpricesell: {
                    required: true
                },
                //productpriceimport: {
                //    required: true
                //},
                productdescription: {
                    ckrequired: true
                },
                //shortdescription: {
                //    ckrequired: true
                //},
                productpriceimport: {
                    required: true
                },
                productcovertype: {
                    required: true
                }

            },
            messages:
            {
                productname: {
                    required: "Bạn chưa nhập tên sản phẩm",
                },
                productstatus: {
                    required: "Bạn chưa chọn trạng thái sản phẩm",
                },
                producttrademark: {
                    required: "Bạn chưa nhập thương hiệu sản phẩm",
                },
                productcategoryid: {
                    required: "Bạn chưa chọn danh mục sản phẩm",
                },
                productpricesell: {
                    required: "Bạn chưa nhập giá bán sản phẩm"
                },
                productpriceimport: {
                    required: "Bạn chưa nhập giá nhập sản phẩm"
                },
                productdescription: {
                    ckrequired: "Bạn chưa nhập mô tả sản phẩm"
                },
                productpriceimport: {
                    required: "Bạn chưa nhập số lượng sản phẩm"
                },
                productcovertype: {
                    required: "Bạn chưa chọn loài bìa"
                },
                //shortdescription: {
                //    ckrequired: "Bạn chưa nhập mô tả ngắn"
                //}
            },
            submitHandler: function (form) {
                self.GetValue();
                if (self.IsUpdate) {
                    self.Update(self.Product);
                    if (self.ProductImages != null && self.ProductImages != "") {
                        self.UploadFileImageProduct(self.Product.id);
                    }
                    if (self.ProductUpdateImage != null && self.ProductUpdateImage.length > 0) {
                        self.RemoveImageServer(self.ProductUpdateImage);
                    }
                }
                else {
                    self.AddProduct(self.Product);
                }
            }
        });
    }

    self.RemoveImageServer = function (productUpdateImages) {
        $.ajax({
            url: '/Admin/Product/RemoveImage',
            type: 'POST',
            dataType: 'json',
            data: {
                images: productUpdateImages
            },
            beforeSend: function () {
                Loading('show');
            },
            complete: function () {
                Loading('hiden');
            },
            success: function (response) {

            }
        })
    }


    self.GetValue = function () {
        self.Product.name = $("#productname").val();
        self.Product.category_id = $("#productcategoryid").val();

        //if (self.ProductImages != null && self.ProductImages.name != null && self.ProductImages.name != "") {
        //    self.Product.avatar = self.ProductImages.name;
        //}
        self.Product.price_sell = $("#productpricesell").val();
        self.Product.price_reduced = $("#productreduced").val();
        self.Product.price_import = $("#productpriceimport").val();
        self.Product.trademark = $("#producttrademark option:selected").text();
        self.Product.trademarkId = $("#producttrademark").val();
        self.Product.status = $("#productstatus").val();

        self.Product.number_import = $("#number_import").val();
        self.Product.cover_type = $("#productcovertype").val();

        self.Product.description = CKEDITOR.instances.productdescription.getData();
        //self.Product.shortdescription = CKEDITOR.instances.shortdescription.getData();
    }

    self.RenderHtmlByObject = function (view) {
        $("#productname").val(view.name);
        $("#productcategoryid").val(view.category_id);
        //$("#productprice").val(view.price);
        //$("#productcolor").val(view.color);
        //if (view.color != null && view.color != "") {
        //    $("#producttypecolor").val(0);
        //}
        //else {
        //    $("#producttypecolor").val(1);
        //}
        self.Product.avatar = view.avatar;
        $("#productquantity").val(view.quantity);
        $("#productshort_desc").val(view.short_desc);

        $("#productpricesell").val(view.price_sell);
        $("#productreduced").val(view.price_reduced);
        $("#productpriceimport").val(view.price_import);
        $("#producttrademark").val(view.trademarkId);
        $("#productstatus").val(view.status);

        $("#number_import").val(view.number_import);
        $("#productcovertype").val(view.cover_type);

        if (view.ImageModelView != null && view.ImageModelView.length > 0) {
            self.ProductServerImages = view.ImageModelView;
            /*  self.ProductUpdateImage = view.ImageModelView;*/
            for (var i = 0; i < view.ImageModelView.length; i++) {
                var item = view.ImageModelView[i];
                var html = "";
                /* html = "<div class=\"box-image\" style=\"background-image:url(/product-image/" + item.name + ")\"><span onclick=\"removeImageViewServer(" + item.id + ",this)\" class='remove-image'>X</span></div>";*/
                html = "<div class=\"box-image\" style=\"background-image:url(/product-image/" + item.name + ")\"></div>";

                $(".productimages").append(html);
            }
        }

        CKEDITOR.instances.productdescription.setData(view.description);
        //CKEDITOR.instances.shortdescription.setData(view.shortdescription);
        /*$("#productspecifications").val(view.specifications);*/
        //$("#productendow").val(view.endow);
        //$(".box-image").css({ "background-image": "url('/product-image/" + view.avatar + "')", "display": "block" });  
    }

    self.UploadFileImageProduct = function (productid) {
        var dataImage = new FormData();
        if (self.ProductImages != null && self.ProductImages) {
            for (var i = 0; i < self.ProductImages.length; i++) {
                dataImage.append(productid, self.ProductImages[i]);
            }
        }

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
                self.GetDataPaging(true);
                //if (response.success) {
                //    self.GetDataPaging(true);
                //    $('#_addUpdate').modal('hide');
                //}
            }
        })
    }
    self.removeImageViewServer = function (id, tag) {
        if (self.IsUpdate) {
            if (self.ProductServerImages != null && self.ProductServerImages.length > 0) {
                var indeximage = self.ProductServerImages.find(p => p.id == id);
                if (indeximage != null) {
                    self.ProductUpdateImage.push(indeximage);
                    $(tag).parent().remove();
                }
            }
        }
    }
    self.removeImage = function (nameimage, tag) {
        if (self.ProductImages != null && self.ProductImages.length > 0) {
            var indeximage = self.ProductImages.findIndex(p => p.name == nameimage);
            if (indeximage >= 0) {
                self.ProductImages.splice(indeximage, 1);
                $(tag).parent().remove();
            }
        }
    }


    $(document).ready(function () {

        self.GetDataPaging();

        self.ValidateUser();

        self.GetAllCategories();
        
        CKEDITOR.replace('productdescription', {});
        //CKEDITOR.replace('shortdescription', {});

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


        $('#productimages').on('change', function () {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;
            if (files != null && files.length > 0) {
                var fileExtension = ['jpeg', 'jpg', 'png'];

                for (var i = 0; i < files.length; i++) {
                    var html = "";
                    if ($.inArray(files[i].type.split('/')[1].toLowerCase(), fileExtension) == -1) {
                        alert("Only formats are allowed : " + fileExtension.join(', '));
                    }
                    else {
                        files[i].name = files[i].name.replace(/ /g, "").toLowerCase();
                        self.ProductImages.push(files[i]);
                        console.log(self.ProductImages);
                        var src = URL.createObjectURL(files[i]);

                        //html = "<div class=\"box-image\" style=\"background-image:url(" + src + ")\"><span onclick=\"removeImage('" + files[i].name + "',this)\" class='remove-image'>X</span></div>";

                        //$(".productimages").append(html);
                        html = "<div class=\"box-image\" style=\"background-image:url(" + src + ")\"></div>";

                        $(".productimages").html(html);
                    }
                }
            }

        });

        $(".btn-back").click(function () {
            window.location.reload();
        })
        // Sử dụng cho quản lý số lượng sản phẩm
        self.InitQuantity();
    })
})(jQuery);