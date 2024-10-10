(function ($) {
    var self = this;
    self.Data = [];
    self.IsUpdate = false;
    self.Category = {
        id: null,
        name: "",
    }
    self.CategorySearch = {
        name: "",
        status: null,
        PageIndex: tedu.configs.pageIndex,
        PageSize: tedu.configs.pageSize
    }

    self.RenderTableHtml = function (data) {
        var html = "";
        if (data != "" && data.length > 0) {
            var index = 0;
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                html += "<tr>";
                html += "<td>" + (++index) + "</td>";
                html += "<td>" + item.name + "</td>";
                html += "<td style=\"text-align: center;\">" +

                    (item.status == 0 ? "<button  class=\"btn btn-dark custom-button\" onClick=UpdateStatus(" + item.id + ",1)><i class=\"bi bi-eye\"></i></button>" : "<button  class=\"btn btn-secondary custom-button\" onClick=UpdateStatus(" + item.id + ",0)><i class=\"bi bi-eye-slash\"></i></button>") +
                    "<button  class=\"btn btn-primary custom-button\" onClick=Update(" + item.id + ")><i  class=\"bi bi-pencil-square\"></i></button>" +
                    "<button  class=\"btn btn-danger custom-button\" onClick=\"Deleted(" + item.id + ")\"><i  class=\"bi bi-trash\"></i></button>" +
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
            $("#titleModal").text("Cập nhật danh mục");
            $(".btn-submit-format").text("Cập nhật");
            $(".custom-format").attr("disabled", "disabled");
            self.GetById(id, self.RenderHtmlByObject);   
            self.Category.id = id;
            //self.RenderHtmlByUser(user);
            $('#categoryModal').modal('show');
            //$(".content-infor").hide();
            //$(".box-content-add").show();

            self.IsUpdate = true;
        }
    }

    self.GetById = function (id, renderCallBack) {
        //self.userData = {};
        if (id != null && id != "") {
            $.ajax({
                url: '/Admin/Category/GetById',
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

    self.UpdateStatus = function (id, status) {
        $.ajax({
            url: '/Admin/Category/UpdateStatus',
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
            tedu.confirm('Bạn có chắc muốn xóa danh mục này?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Category/Delete",
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

        self.CategorySearch.PageIndex = tedu.configs.pageIndex;
        self.CategorySearch.PageSize = tedu.configs.pageSize;

        $.ajax({
            url: '/Admin/Category/GetAllPaging',
            type: 'GET',
            data: self.CategorySearch,
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


   

    self.Add = function (userView) {
        $.ajax({
            url: '/Admin/Category/Add',
            type: 'POST',
            dataType: 'json',
            data: {
                CategoryModelView: userView
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
                    $('#categoryModal').modal('hide');
                }
                else if (response.isCategoryNameExist) {
                    $(".categoryname-exist").show().text("Tên danh mục đã tồn tại");
                }
               
            }
        })
    }

   

    self.UpdateObject = function (userView) {
        $.ajax({
            url: '/Admin/Category/Update',
            type: 'POST',
            dataType: 'json',
            data: {
                CategoryModelView: userView
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
                    $('#categoryModal').modal('hide');
                }

            }
        })
    }

    self.ValidateUser = function () {
        $(".categoryname-exist").hide();
        $("#form-submit").validate({
            rules:
            {
                name: {
                    required: true,
                }
                            
            },
            messages:
            {
                name: {
                    required: "Tên danh mục không được để trống",
                }                
            },
            submitHandler: function (form) {
                self.GetValue();               
                if (self.IsUpdate) {
                    self.UpdateObject(self.Category);
                }
                else {
                    self.Add(self.Category);
                }
            }
        });
    }

    self.GetValue = function () {
        self.Category.name = $(".txtname").val();
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
        $(".txtname").val(view.name);
    }

   
    $(document).ready(function () {
        self.GetDataPaging();
        
        self.ValidateUser();
        
        $(".modal").on("hidden.bs.modal", function () {
            $(this).find('form').trigger('reset');
            $("form").validate().resetForm();
            $("label.error").hide();
            $(".error").removeClass("error");
        });

        $(".btn-addorupdate").click(function () {
            $("#titleModal").text("Thêm mới danh mục");
            $(".btn-submit-format").text("Thêm mới");
            self.IsUpdate = false;
            $('#categoryModal').modal('show');
        })
        $('#select-right').on('change', function () {
            $('input.form-search').val("");
            self.CategorySearch.name = null;
            self.CategorySearch.status = $(this).val();
            self.GetDataPaging(true);
        });

        $('#ddlShowPage').on('change', function () {
            tedu.configs.pageSize = $(this).val();
            tedu.configs.pageIndex = 1;
            self.GetDataPaging(true);
        });

        $('input.form-search').on('input', function (e) {
            self.CategorySearch.name = $(this).val();
            self.GetDataPaging(true);
        });

    })
})(jQuery);