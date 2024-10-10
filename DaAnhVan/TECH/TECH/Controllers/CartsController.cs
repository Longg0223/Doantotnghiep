using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Mail;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Data.DatabaseEntity;
using TECH.Models;
using TECH.Service;

namespace TECH.Controllers
{
    public class CartsController : Controller
    {
        private readonly ICartsService _cartsService;
        private readonly IOrdersService _ordersService;
        private readonly IProductsService _productsService;
        public IHttpContextAccessor _httpContextAccessor;
        private readonly IReviewsService _reviewsService;
        private readonly IImagesService _imagesService;
        //private readonly ISizesService _sizesService;
        private readonly IProductsImagesService _productsImagesService;
        private readonly IProductQuantityService _productQuantityService;
        private readonly IVnPayService _vnPayservice;
        public CartsController(ICartsService cartsService,
            IHttpContextAccessor httpContextAccessor,
            IOrdersService ordersService,
            IReviewsService reviewsService,
            IVnPayService vnPayservice,
        IImagesService imagesService,
            IProductQuantityService productQuantityService,
        IProductsImagesService productsImagesService,
        IProductsService productsService)
        {
            _vnPayservice = vnPayservice;
            _cartsService = cartsService;
            _ordersService = ordersService;
            _httpContextAccessor = httpContextAccessor;
            _productsService = productsService;
            _reviewsService = reviewsService;
            _imagesService = imagesService;
            _productsImagesService = productsImagesService;
            _productQuantityService = productQuantityService;
        }

        [HttpPost]
        public JsonResult AddOrder(OrdersModelView OrdersModelView)
        {
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var user = new UserModelView();
            if (userString != null)
            {
                user = JsonConvert.DeserializeObject<UserModelView>(userString);
                OrdersModelView.user_id = user.id;
                OrdersModelView.code = "MDH00" + user.id.ToString() + DateTime.Now.Second.ToString();

                var model = _cartsService.GetAllCart(user.id);
                if (OrdersModelView.payment == 2)
                {
                    _httpContextAccessor.HttpContext.Session.SetString("OrdersModelView", JsonConvert.SerializeObject(OrdersModelView));
                    if (model != null && model.Count > 0)
                    {
                        decimal totalprice = 0;
                        foreach (var item in model)
                        {
                            totalprice += item.price.HasValue && item.price.Value > 0 ? item.price.Value : 0;
                        }
                        var vnPayModel = new VnPaymentRequestModel
                        {
                            Amount = totalprice,
                            CreatedDate = DateTime.Now,
                            Description = $"{user.full_name} {user.phone_number}",
                            FullName = user.full_name,
                            OrderId = OrdersModelView.code
                        };
                        string htmlvpn = _vnPayservice.CreatePaymentUrl(HttpContext, vnPayModel);
                        return Json(new
                        {
                            success = true,
                            vnpay = htmlvpn
                        });
                    }
                }
                else
                {
                    var result = _ordersService.AddOrder(OrdersModelView);

                    if (result > 0)
                    {
                        //var model = _cartsService.GetAllCart(user.id);
                        if (model != null && model.Count > 0)
                        {
                            decimal totalprice = 0;
                            foreach (var item in model)
                            {
                                totalprice += item.price.HasValue && item.price.Value > 0 ? item.price.Value : 0;
                                var ordersDetailModelView = new OrdersDetailModelView();
                                ordersDetailModelView.order_id = result;
                                ordersDetailModelView.product_id = item.product_id;
                                //ordersDetailModelView.color = item.color;
                                //ordersDetailModelView.sizeId = item.sizeId;
                                var product = _productsService.GetByid(item.product_id.Value);
                                ordersDetailModelView.price = Convert.ToInt32(product.price_sell);
                                ordersDetailModelView.quantity = item.quantity;
                                _ordersService.AddOrderDetail(ordersDetailModelView);
                                _cartsService.Deleted(item.id);
                                //_ordersService.Save();
                            }

                            //var vnPayModel = new VnPaymentRequestModel
                            //{
                            //    Amount = totalprice,
                            //    CreatedDate = DateTime.Now,
                            //    Description = $"{user.full_name} {user.phone_number}",
                            //    FullName = user.full_name,
                            //    OrderId = OrdersModelView.code
                            //};
                            //string htmlvpn = "";
                            //if (OrdersModelView.payment == 2)
                            //{
                            //    htmlvpn = _vnPayservice.CreatePaymentUrl(HttpContext, vnPayModel);
                            //}                       
                            return Json(new
                            {
                                success = true,
                            });
                        }
                    }
                }
            }

            return Json(new
            {
                success = false
            }); 
        }

        public void SendMail(string email, string code)
        {

            var html = @"
    <div width='100%' style='margin: 0; padding: 0 !important; background-color: #f1f1f1;'>
        <h3>Xin chào bạn. Cảm ơn bạn đã tin tưởng và đặt hàng.</h3>
        <center style='width: 100%; background-color: #f1f1f1;'>
            <div style='margin: 0 auto;' class='m_431664471943874608email-container'>
                <table align='center' role='presentation' cellspacing='0' cellpadding='0' border='0' width='100%' style='margin: auto;'>
                    <tbody>
                        <tr>
                            <td valign='middle' class='m_431664471943874608hero m_431664471943874608bg_white' style='padding: 2em 0 2em 0;'>
                                <table role='presentation' border='0' cellpadding='0' cellspacing='0' width='100%'>
                                    <tbody>
                                        <tr>
                                            <td style='padding: 0 2.5em; text-align: center;'>
                                                <div class='m_431664471943874608text'>
                                                    <h3>Đơn hàng của bạn</h3>
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table class='m_431664471943874608bg_white' role='presentation' border='0' cellpadding='0' cellspacing='0' width='100%'>
                                    <tbody>
                                        <tr style='border-bottom: 1px solid rgba(0, 0, 0, 0.05); background-color: grey;'>
                                            <th width='20%' style='text-align: center; padding: 0 2.5em; color: #000; padding-bottom: 20px;'>Tên sản phẩm</th>
                                            <th width='45%' style='text-align: center; padding: 0 2.5em; color: #000; padding-bottom: 20px;'>Số lượng</th>
                                            <th width='10%' style='text-align: center; padding: 0 2.5em; color: #000; padding-bottom: 20px;'>Tổng</th>
                                        </tr>
                                        <tr style='border-bottom: 1px solid rgba(0, 0, 0, 0.05);'>
                                            <td valign='middle' style='text-align: left; padding: 0 2.5em;'>
                                                <p style='color: #000; font-size: 15px;'>Ipad Promax | màu null</p>
                                            </td>
                                            <td valign='middle' style='text-align: left; padding: 0 2.5em;'>
                                                <p style='color: #000; font-size: 15px;'>x1</p>
                                            </td>
                                            <td valign='middle' style='text-align: left; padding: 0 2.5em;'>
                                                <p style='color: #000; font-size: 15px;'>30000092</p>
                                            </td>
                                        </tr>
                                        <tr style='border-bottom: 1px solid rgba(0, 0, 0, 0.05);'>
                                            <td colspan='2' style='text-align: right; padding: 0 2.5em;'>
                                                Tạm tính
                                            </td>
                                            <td style='padding: 0 2.5em;'>30000092<i>đ</i></td>
                                        </tr>
                                        <tr style='border-bottom: 1px solid rgba(0, 0, 0, 0.05);'>
                                            <td colspan='2' style='text-align: right; padding: 0 2.5em;'>
                                                Hình thức mua hàng
                                            </td>
                                            <td style='padding: 0 2.5em;'>Mua trực tiếp</td>
                                        </tr>
                                        <tr style='border-bottom: 1px solid rgba(0, 0, 0, 0.05);'>
                                            <td colspan='2' style='text-align: right; padding: 0 2.5em;'>
                                                Tổng phải thanh toán
                                            </td>
                                            <td style='padding: 0 2.5em;'>30000092<i>đ</i></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </center>

        <center style='width: 100%; background-color: #f1f1f1;'>
            <div style='margin: 0 auto;' class='m_431664471943874608email-container'>
                <table align='center' role='presentation' cellspacing='0' cellpadding='0' border='0' width='100%' style='margin: auto;'>
                    <tbody>
                        <tr>
                            <td valign='middle' class='m_431664471943874608hero m_431664471943874608bg_white' style='padding: 2em 0 2em 0;'>
                                <table role='presentation' border='0' cellpadding='0' cellspacing='0' width='100%'>
                                    <tbody>
                                        <tr>
                                            <td style='padding: 0 2.5em; text-align: center;'>
                                                <div class='m_431664471943874608text'>
                                                    <h3>Thông tin vận chuyển</h3>
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table class='m_431664471943874608bg_white' role='presentation' border='0' cellpadding='0' cellspacing='0' width='100%'>
                                    <tbody>
                                        <tr style='border-bottom: 1px solid rgba(0, 0, 0, 0.05);'>
                                            <td valign='middle' style='text-align: left; padding: 0 2.5em;'>
                                                <p style='color: #000; font-size: 15px;'>Họ và tên:</p>
                                            </td>
                                            <td valign='middle' style='text-align: left; padding: 0 2.5em;'>
                                                <p style='color: #000; font-size: 15px;'>nguyễn văn a</p>
                                            </td>
                                        </tr>
                                        <tr style='border-bottom: 1px solid rgba(0, 0, 0, 0.05);'>
                                            <td valign='middle' style='text-align: left; padding: 0 2.5em;'>
                                                <p style='color: #000; font-size: 15px;'>Số điện thoại:</p>
                                            </td>
                                            <td valign='middle' style='text-align: left; padding: 0 2.5em;'>
                                                <p style='color: #000; font-size: 15px;'>0345801983</p>
                                            </td>
                                        </tr>
                                        <tr style='border-bottom: 1px solid rgba(0, 0, 0, 0.05);'>
                                            <td valign='middle' style='text-align: left; padding: 0 2.5em;'>
                                                <p style='color: #000; font-size: 15px;'>Ghi chú:</p>
                                            </td>
                                            <td valign='middle' style='text-align: left; padding: 0 2.5em;'>
                                                <p style='color: #000; font-size: 15px;'></p>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </center>
</div>
";


            MailMessage mail = new MailMessage();
            mail.To.Add(email.Trim());
            mail.From = new MailAddress("emcuahai@gmail.com");
            mail.Subject = "Xác Thực Tài Khoản";
            mail.Body = html;
            mail.IsBodyHtml = true;
            mail.Sender = new MailAddress("emcuahai@gmail.com");
            SmtpClient smtp = new SmtpClient();
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Host = "smtp.gmail.com";
            smtp.Credentials = new System.Net.NetworkCredential("emcuahai@gmail.com", "tnquotkcftugcbve");
            smtp.Send(mail);
        }

        public IActionResult HistoryOrder()
        {
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var user = new UserModelView();
            var model = new List<OrdersModelView>();
            if (userString != null)
            {
                user = JsonConvert.DeserializeObject<UserModelView>(userString);
                if (user != null)
                {
                    var data = _ordersService.GetOrderForUserId(user.id);
                    if (data != null && data.Count > 0)
                    {
                        foreach (var item in data)
                        {
                            if (item != null && item.user_id.HasValue)
                            {
                                if (item.payment == 1)
                                {
                                    item.paymentstr = "Ship Cod";
                                }
                                else if (item.payment == 2)
                                {
                                    item.paymentstr = "VnPay";
                                }
                                else if (item.payment == 3)
                                {
                                    item.paymentstr = "Momo";
                                }
                                else if (item.payment == 0)
                                {
                                    item.paymentstr = "Mua trực tiếp";
                                }
                            }
                            if (item.status == 0)
                            {
                                item.statusstr = "Đang chờ xử lý";
                            }
                            else if (item.status == 1)
                            {
                                item.statusstr = "Đã hoàn thành";
                            }
                            else if (item.status == 2)
                            {
                                item.statusstr = "Đã hủy";
                            }

                        }
                        model = data;
                    }
                    return View(model);
                }
            }
            return Redirect("/home");
        }

        public IActionResult Index()
        {
            var carts = new List<CartsModelView>();
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var user = new UserModelView();
            if (userString != null)
            {
                user = JsonConvert.DeserializeObject<UserModelView>(userString);
                if (user != null)
                {
                    var model = _cartsService.GetAllCart(user.id);
                    if (model != null && model.Count > 0)
                    {
                        foreach (var item in model)
                        {
                            if (item.product_id.HasValue && item.product_id.Value > 0)
                            {
                                var _product = _productsService.GetByid(item.product_id.Value);
                                if (_product != null)
                                {
                                    var productImage = _productsImagesService.GetImageProduct(_product.id);
                                    if (productImage != null && productImage.Count > 0)
                                    {
                                        var image = _imagesService.GetImageName(productImage);
                                        if (image != null && image.Count > 0)
                                        {
                                            _product.ImageModelView = image;
                                        }
                                    }


                                    item.productModelView = _product;
                                }
                            }                          
                        }
                        carts = model;
                    }
                }
                return View(carts);
            }
            return Redirect("/home");

        }
        public IActionResult ReviewOrderProduct(int orderId)
        {
            var data = new List<OrdersModelView>();
            if (orderId > 0)
            {
                var order = _ordersService.GetByid(orderId);
                if (order != null)
                {
                    var orderDetail = _ordersService.GetOrderDetails(orderId);
                    if (orderDetail != null && orderDetail.Count > 0)
                    {
                        foreach (var item in orderDetail)
                        {
                            if (item.product_id.HasValue && item.product_id.Value > 0)
                            {
                                var product = _productsService.GetByid(item.product_id.Value);
                                if (product != null)
                                {
                                    item.ProductModelView = product;
                                }

                            }
                        }
                        order.OrdersDetailModelView = orderDetail;
                    }
                    data.Add(order);
                }                
            }
            return PartialView("ReivewsOrderProduct", data);
        }
        public IActionResult OrderPay()
        {
            var ordersCartDetailModelView = new OrdersCartDetailModelView();
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var user = new UserModelView();
            if (userString != null)
            {
                user = JsonConvert.DeserializeObject<UserModelView>(userString);
                if (user != null)
                {
                    user.address = !string.IsNullOrEmpty(user.address) && user.address !="null" ? user.address : "";
                    ordersCartDetailModelView.UserModelView = user;
                    var model = _cartsService.GetAllCart(user.id);
                    if (model != null && model.Count > 0)
                    {
                        foreach (var item in model)
                        {
                            if (item.product_id.HasValue && item.product_id.Value > 0)
                            {
                                var _product = _productsService.GetByid(item.product_id.Value);
                                if (_product != null)
                                {
                                    var productImage = _productsImagesService.GetImageProduct(_product.id);
                                    if (productImage != null && productImage.Count > 0)
                                    {
                                        var image = _imagesService.GetImageName(productImage);
                                        if (image != null && image.Count > 0)
                                        {
                                            _product.ImageModelView = image;
                                        }
                                    }
                                    item.productModelView = _product;
                                }
                            }
                        }
                        ordersCartDetailModelView.CartsModelView = model;
                        return View(ordersCartDetailModelView);
                    }
                }
            }
            return Redirect("/home");
            
        }

        [HttpPost]
        public JsonResult ReviewsPost(List<ReviewsModelView> reviewsPost)
        {

            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var user = new UserModelView();
            bool status = false;
            if (userString != null)
            {
                if (reviewsPost != null && reviewsPost.Count > 0)
                {
                    foreach (var item in reviewsPost)
                    {
                        item.status = 0;
                        _reviewsService.Add(item);
                        _reviewsService.Save();
                    }
                    _ordersService.UpdateReview(reviewsPost[0].order_id.Value, 1);
                    _ordersService.Save();
                    status = true;
                }
                else
                {
                    status = false;
                }
            }
            else
            {
                status = false;
            }            

            return Json(new
            {
                success = status
            });
        }

        [HttpPost]
        public JsonResult Add(CartsModelView cartsModelView)
        {

            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            bool overstock = false;
            int tongsocon = 0;
            var user = new UserModelView();
            if (userString != null)
            {
               
                user = JsonConvert.DeserializeObject<UserModelView>(userString);
                if (user != null)
                {
                    cartsModelView.user_id = user.id;
                    //var product = _productsService.GetByid(cartsModelView.product_id.Value);
                    // thực hiện trừ đi trong database
                    if (cartsModelView.product_id.HasValue && cartsModelView.product_id.Value > 0)
                    {
                        var productExist = _cartsService.GetProductCart(user.id, cartsModelView.product_id.Value);
                        if (productExist != null && productExist.quantity.HasValue && productExist.quantity.Value > 0)
                        {
                            var qtyServer = productExist.quantity.Value + cartsModelView.quantity.Value;
                            var product = _productsService.GetByid(cartsModelView.product_id.Value);
                            var price = productExist.price;
                            if (product != null && product.price_sell.HasValue && product.price_sell.Value > 0)
                            {
                                price = productExist.price + cartsModelView.quantity.Value * product.price_sell.Value;
                            }
                            productExist.price = price;
                            productExist.quantity = qtyServer;
                            _cartsService.Update(productExist);
                        }
                        else
                        {
                            _cartsService.Add(cartsModelView);
                        }
                        _cartsService.Save();
                        return Json(new
                        {
                            success = true,
                            overstock = overstock,
                            tongsoconlai = 0
                        });
                    }                  
                }
            }
         
            return Json(new
            {
                success = false,
                overstock = overstock,
                tongsoconlai = -1
            });
        }

        
        public IActionResult PaymentCallBack(string vnp_Amount, string vnp_BankCode, string vnp_BankTranNo, string vnp_CardType, string vnp_OrderInfo, string vnp_PayDate, string vnp_ResponseCode, string vnp_TmnCode, string vnp_TransactionNo,
            string vnp_TransactionStatus, string vnp_TxnRef, string vnp_SecureHash)
        {
            var response = _vnPayservice.PaymentExecute(Request.Query);
            string Message = "";
            if (response == null || vnp_ResponseCode != "00")
            {
                //Message = $"Lỗi thanh toán VN Pay: {response.VnPayResponseCode}";
                Message = "Thanh toán không thành công";
                TempData["Message"] = Message;
                return View();
            }
            
            var data = _httpContextAccessor.HttpContext.Session.GetString("OrdersModelView");
            if (data != null)
            {
              var dataConvert = JsonConvert.DeserializeObject<OrdersModelView>(data);
                if (dataConvert != null)
                {
                    var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
                    var user = new UserModelView();
                    var model = new List<CartsModelView>();
                    if (userString != null)
                    {
                        user = JsonConvert.DeserializeObject<UserModelView>(userString);

                        model = _cartsService.GetAllCart(user.id);
                    }

                   var result = _ordersService.AddOrder(dataConvert);

                    if (result > 0)
                    {
                        //var model = _cartsService.GetAllCart(user.id);
                        if (model != null && model.Count > 0)
                        {
                            decimal totalprice = 0;
                            foreach (var item in model)
                            {
                                totalprice += item.price.HasValue && item.price.Value > 0 ? item.price.Value : 0;
                                var ordersDetailModelView = new OrdersDetailModelView();
                                ordersDetailModelView.order_id = result;
                                ordersDetailModelView.product_id = item.product_id;
                                //ordersDetailModelView.color = item.color;
                                //ordersDetailModelView.sizeId = item.sizeId;
                                var product = _productsService.GetByid(item.product_id.Value);
                                ordersDetailModelView.price = Convert.ToInt32(product.price_sell);
                                ordersDetailModelView.quantity = item.quantity;
                                _ordersService.AddOrderDetail(ordersDetailModelView);
                                _cartsService.Deleted(item.id);
                                //_ordersService.Save();
                            }
                            _httpContextAccessor.HttpContext.Session.Remove("OrdersModelView");
                            Message = "Thanh toán thành công";
                        }
                    }
                }
            }
            TempData["Message"] = Message;
            return View();
        }



        [HttpPost]
        public JsonResult Update(CartsModelView cartsModelView)
        {

            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var user = new UserModelView();
            if (userString != null)
            {
                user = JsonConvert.DeserializeObject<UserModelView>(userString);
                if (user != null)
                {
                    cartsModelView.user_id = user.id;
                    if (cartsModelView.product_id.HasValue && cartsModelView.product_id.Value > 0)
                    {
                        var product = _productsService.GetByid(cartsModelView.product_id.Value);
                        if (product != null)
                        {
                            cartsModelView.price = Convert.ToInt32(product.price_sell) * cartsModelView.quantity.Value;
                            cartsModelView.pricestr = cartsModelView.price.HasValue && cartsModelView.price.Value > 0 ? cartsModelView.price.Value.ToString("#,###") : "";
                        }

                    }
                   
                    var result = _cartsService.Update(cartsModelView);
                    _cartsService.Save();
                    return Json(new
                    {
                        success = result,
                        Data= cartsModelView
                    });
                }
            }
          
            return Json(new
            {
                success = false
            });
        }

        [HttpPost]
        public JsonResult Deleted(int id)
        {

            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var user = new UserModelView();
            if (userString != null)
            {
                user = JsonConvert.DeserializeObject<UserModelView>(userString);
                if (user != null)
                {
                    var result = _cartsService.Deleted(id);
                    _cartsService.Save();
                    return Json(new
                    {
                        success = result
                    });
                }
            }
       
            return Json(new
            {
                success = false
            });
        }



       
    }
}