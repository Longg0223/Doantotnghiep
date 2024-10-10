using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TECH.Areas.Admin.Models;
using TECH.Service;

namespace TECH.Controllers.Components

{
    [ViewComponent(Name = "OrderDetailComponent")]
    public class OrderDetailComponent : ViewComponent
    {
        private readonly IOrdersService _ordersService;
        private readonly IAppUserService _appUserService;
        private readonly IProductsService _productsService;
        public OrderDetailComponent(IOrdersService ordersService,
            IAppUserService appUserService,
            IProductsService productsService)
        {
            _ordersService = ordersService;
            _appUserService = appUserService;
            _productsService = productsService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int orderId)
        {
            var model = new OrdersModelView();
            if (orderId > 0)
            {

                model = _ordersService.GetByid(orderId);

                if (model != null && model.user_id.HasValue)
                {
                    model.totalstr = model.total.HasValue && model.total.Value > 0 ? model.total.Value.ToString("#,###") : "";

                    if (model.payment == 1)
                    {
                        model.paymentstr = "Ship Cod";
                    }
                    else if (model.payment == 2)
                    {
                        model.paymentstr = "VnPay";
                    }
                    else if (model.payment == 0)
                    {
                        model.paymentstr = "Mua trực tiếp";
                    }

                    var appuser = _appUserService.GetByid(model.user_id.Value);
                    if (appuser != null)
                    {
                        model.UserModelView = appuser;
                    }

                    var orderDetails = _ordersService.GetOrderDetails(model.id);
                    if (orderDetails != null && orderDetails.Count > 0)
                    {
                        foreach (var item in orderDetails)
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
                        model.OrdersDetailModelView = orderDetails;
                    }

                }

            }
            return View(model);
        }
    }
}