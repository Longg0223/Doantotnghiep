(function ($) {
    var self = this;
    self.Data = [];
    self.Cities = [];
    self.Districts = [];
    self.Wards = [];
    self.PostImages = {};
    self.IsUpdate = false;    
    self.Fees = {
        id: null,
        city_id: null,
        district_id: null,
        fee: null,
    }
    



    self.FeesSearch = {
        name: "",
        PageIndex: tedu.configs.pageIndex,
        PageSize: tedu.configs.pageSize
    }


    self.RenderTableHtml = function (data) {
        self.Data = data;
        var html = "";
        if (data != "" && data.length > 0) {
            var index = 0;
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                html += "<tr>";
                html += "<td>" + (++index) + "</td>";
                html += "<td>" + item.CityModelView.name + "</td>";
                html += "<td>" + item.DistrictsModelView.name + "</td>";
                html += "<td>" + item.WardsModelView.name + "</td>";
                html += "<td>" + item.feestr + "</td>";         
                html += "<td style=\"text-align: center;\">" +
                    "<button  class=\"btn btn-primary custom-button\" onClick=Update(" + item.id + ")><i  class=\"bi bi-pencil-square\"></i></button>" +
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
            $("#titleModal").text("Cập nhật phí vận chuyển");
            $(".btn-submit-format").text("Cập nhật");
            self.GetById(id, self.RenderHtmlByObject);
            self.Fees.id = id;

            $('#feeModal').modal('show');

            //$(".product-add-update").show();
            //$(".product-list").hide();
            self.IsUpdate = true;
        }
    }

    self.GetAllCities = function () {
        $.ajax({
            url: '/Admin/Fee/GetAllCity',
            type: 'GET',
            dataType: 'json',
            beforeSend: function () {
                Loading('show');
            },
            complete: function () {
                Loading('hiden');
            },
            success: function (response) {
                var html = "<option value =\"\">Chọn thành phố</option>";                
                if (response.Data != null && response.Data.length > 0) {
                    self.Cities = response.Data;
                    for (var i = 0; i < response.Data.length; i++) {
                        var item = response.Data[i];
                        html += "<option value =" + item.id + ">" + item.name + "</option>";
                        
                    }
                }
                $("#cities").html(html);
            }
        })
    }
    self.GetAllDistrict = function () {
        $.ajax({
            url: '/Admin/Fee/GetAllDistricts',
            type: 'GET',
            dataType: 'json',
            beforeSend: function () {
                Loading('show');
            },
            complete: function () {
                Loading('hiden');
            },
            success: function (response) {
                var html = "<option value =\"\">Chọn quận/huyện </option>";
                if (response.Data != null && response.Data.length > 0) {
                    self.Districts = response.Data;
                    for (var i = 0; i < response.Data.length; i++) {
                        var item = response.Data[i];
                        html += "<option value =" + item.id + ">" + item.name + "</option>";

                    }
                }
                $("#districts").html(html);
            }
        })
    }

    self.GetDistrictForCityId = function (tag) {
        var cityId = $(tag).val();
        /*self.GetDistrictCityId(cityId);*/
        $.ajax({
            url: '/Admin/Fee/GetDistrictsForCityId',
            type: 'GET',
            data: {
                cityId: cityId
            },
            dataType: 'json',
            beforeSend: function () {
                Loading('show');
            },
            complete: function () {
                Loading('hiden');
            },
            success: function (response) {
                
                var html = "<option value=\"\">Chọn quận/huyện</option>";
                if (response.Data != null && response.Data.length > 0) {
                    $("#district").removeAttr("disabled");
                    self.Districts = response.Data;
                    for (var i = 0; i < response.Data.length; i++) {
                        var item = response.Data[i];
                        html += "<option value =" + item.id + ">" + item.name + "</option>";
                    }
                }
                else {
                    //$("#wards").attr("disabled","disabled");
                    //$("#district").attr("disabled", "disabled");
                }
                $("#district").html(html);
            }
        })
    }
    self.GetDistrictCityId = function (cityId,districtId) {
        $.ajax({
            url: '/Admin/Fee/GetDistrictsForCityId',
            type: 'GET',
            data: {
                cityId: cityId
            },
            dataType: 'json',
            beforeSend: function () {
                Loading('show');
            },
            complete: function () {
                Loading('hiden');
            },
            success: function (response) {

                var html = "<option value=\"\">Chọn quận/huyện</option>";
                if (response.Data != null && response.Data.length > 0) {
                    $("#district").removeAttr("disabled");
                    self.Districts = response.Data;
                    for (var i = 0; i < response.Data.length; i++) {
                        var item = response.Data[i];
                        if (item.id == districtId) {
                            html += "<option value =" + item.id + " selected>" + item.name + "</option>";
                        }
                        else {
                            html += "<option value =" + item.id + ">" + item.name + "</option>";
                        }
                        
                    }
                }
                else {
                    //$("#wards").attr("disabled","disabled");
                    //$("#district").attr("disabled", "disabled");
                }
                $("#district").html(html);
            }
        })
    }


    self.GetWardsForDistrictId = function (tag) {
        var districtId = $(tag).val();        
        $.ajax({
            url: '/Admin/Fee/GetWardsForDistrictId',
            type: 'GET',
            data: {
                districtId: districtId
            },
            dataType: 'json',
            beforeSend: function () {
                Loading('show');
            },
            complete: function () {
                Loading('hiden');
            },
            success: function (response) {
                var html = "<option value=\"\">Chọn phường/xã</option>";
                if (response.Data != null && response.Data.length > 0) {
                  /*  $("#wards").removeAttr("disabled");*/
                    for (var i = 0; i < response.Data.length; i++) {
                        var item = response.Data[i];
                        html += "<option value =" + item.id + ">" + item.name + "</option>";

                    }
                }
                else {
                  /*  $("#wards").attr("disabled", "disabled");*/
                }
                $("#wards").html(html);
            }
        })
    }

    self.GetWardsDistrictId = function (districtId,wardsId) {
        $.ajax({
            url: '/Admin/Fee/GetWardsForDistrictId',
            type: 'GET',
            data: {
                districtId: districtId
            },
            dataType: 'json',
            beforeSend: function () {
                Loading('show');
            },
            complete: function () {
                Loading('hiden');
            },
            success: function (response) {
                var html = "<option value=\"\">Chọn phường/xã</option>";
                if (response.Data != null && response.Data.length > 0) {
                    /*  $("#wards").removeAttr("disabled");*/
                    for (var i = 0; i < response.Data.length; i++) {
                        var item = response.Data[i];
                        if (item.id == wardsId) {
                            html += "<option value =" + item.id + " selected>" + item.name + "</option>";
                        }
                        else {
                            html += "<option value =" + item.id + ">" + item.name + "</option>";
                        }
                      

                    }
                }
                else {
                    /*  $("#wards").attr("disabled", "disabled");*/
                }
                $("#wards").html(html);
            }
        })
    }


    self.GetAllWards = function () {
        $.ajax({
            url: '/Admin/Fee/GetAllWards',
            type: 'GET',
            dataType: 'json',
            beforeSend: function () {
                Loading('show');
            },
            complete: function () {
                Loading('hiden');
            },
            success: function (response) {
                var html = "<option value =\"\">Chọn phường/xã </option>";
                if (response.Data != null && response.Data.length > 0) {
                    self.Wards = response.Data;
                    for (var i = 0; i < response.Data.length; i++) {
                        var item = response.Data[i];
                        html += "<option value =" + item.id + ">" + item.name + "</option>";

                    }
                }
                $("#wards").html(html);
            }
        })
    }




    self.GetById = function (id, renderCallBack) {
        //self.PostData = {};
        if (id != null && id != "") {
            $.ajax({
                url: '/Admin/Fee/GetById',
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
            tedu.confirm('Bạn có chắc muốn phí vận chuyển này?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Fee/Delete",
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

        self.FeesSearch.PageIndex = tedu.configs.pageIndex;
        self.FeesSearch.PageSize = tedu.configs.pageSize;

        $.ajax({
            url: '/Admin/Fee/GetAllPaging',
            type: 'GET',
            data: self.FeesSearch,
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
   
    self.AddUser = function (userView) {
        $.ajax({
            url: '/Admin/Fee/Add',
            type: 'POST',
            dataType: 'json',
            data: {
                FeeModelView: userView
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
                    $('#feeModal').modal('hide');
                }                
            }
        })
    }

    self.UpdateUser = function (userView) {
        $.ajax({
            url: '/Admin/Fee/Update',
            type: 'POST',
            dataType: 'json',
            data: {
                FeeModelView: userView
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
                    $('#feeModal').modal('hide');
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
                cities: {
                    required: true,
                },   
                district: {
                    required: true
                },
                wards: {
                    required: true
                },
                fees: {
                    required: true
                }
                
            },
            messages:
            {
                cities: {
                    required: "Bạn chưa chọn thành phố",
                },               
                district: {
                    required: "Bạn chưa chọn quận huyện"
                },
                wards: {
                    required: "Bạn chưa chọn phường xã"
                },
                fees: {
                    required: "Bạn chưa nhập phí vận chuyển"
                }
            },
            submitHandler: function (form) {               
                self.GetValue();
                debugger;
                if (self.IsUpdate) {
                    self.UpdateUser(self.Fees);
                }
                else {                    
                    self.AddUser(self.Fees);
                }

            }
        });
    }

    self.GetValue = function () {
        self.Fees.city_id = $("#cities").val();     
        self.Fees.district_id = $("#district").val();     
        self.Fees.ward_id = $("#wards").val();     
        self.Fees.fee = $("#fees").val();           
    }

    self.RenderHtmlByObject = function (view) {
        $("#cities").val(view.city_id);
        self.GetDistrictCityId(view.city_id, view.district_id);
        $("#district").val(view.district_id);
        self.GetWardsDistrictId(view.district_id, view.ward_id);
        /*$("#wards").val(view.ward_id);*/
        $("#fees").val(view.fee);
    }

    


    $(document).ready(function () {

        self.ValidateUser();
        //self.GetAllDistrict();
        //self.GetAllWards();
        self.GetAllCities();

        self.GetDataPaging();

        $(".modal").on("hidden.bs.modal", function () {
            $(this).find('form').trigger('reset');
            $("form").validate().resetForm();
            $("#district,#wards").html("");
            $("label.error").hide();
            $(".error").removeClass("error");
        });

        $(".btn-addorupdate").click(function () {
            $(".custom-format").removeAttr("disabled");
            $("#titleModal").text("Thêm mới phí vận chuyển");            
            $(".btn-submit-format").text("Thêm mới");
            self.IsUpdate = false;
            $('#feeModal').modal('show');
        })
        //$('#select-right').on('change', function () {
        //    $('input.form-search').val("");
        //    self.FeesSearch.name = null;
        //    self.FeesSearch.star = $(this).val();
        //    self.GetDataPaging(true);
        //});

        $('#ddlShowPage').on('change', function () {
            tedu.configs.pageSize = $(this).val();
            tedu.configs.pageIndex = 1;
            self.GetDataPaging(true);
        });

        $('input.form-search').on('input', function (e) {
            self.FeesSearch.name = $(this).val();
            self.GetDataPaging(true);
        });        
       
    })
})(jQuery);