using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace TECH.Areas.Admin.Controllers
{
    public class ReviewsController : BaseController
    {
        private readonly IReviewsService _reviewsService;
        private readonly IProductsService _productService;
        private readonly IOrdersService _ordersService;
        private readonly IAppUserService _appUserService;
        public ReviewsController(
            IReviewsService reviewsService,
            IProductsService productService,
            IOrdersService ordersService,  
            IAppUserService appUserService
            )
        {
            _reviewsService = reviewsService;
            _productService = productService;
            _appUserService = appUserService;
            _ordersService = ordersService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddView()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetById(int id)
        {
            var model = new ReviewsModelView();
            if (id > 0)
            {
                model = _reviewsService.GetByid(id);
                if (model.order_id.HasValue && model.order_id.Value > 0)
                {
                    var dataOrders = _ordersService.GetByid(model.order_id.Value);
                    if (dataOrders != null)
                    {
                        model.ordersModelView = dataOrders;

                        if (dataOrders.id > 0 && dataOrders.user_id.HasValue && dataOrders.user_id.Value > 0)
                        {
                            var dataUser = _appUserService.GetByid(dataOrders.user_id.Value);
                            if (dataUser != null)
                            {
                                model.userModelView = dataUser;
                            }
                        }
                    }
                }
            }
            return Json(new
            {
                Data = model
            });
        }

        [HttpGet]
        public IActionResult AddOrUpdate()
        {
            return View();
        }
       
        [HttpPost]
        public JsonResult Add(ReviewsModelView ReviewsModelView)
        {
            var result = _reviewsService.Add(ReviewsModelView);
            _reviewsService.Save();
            return Json(new
            {
                success = result
            });

        }

        [HttpGet]
        public JsonResult UpdateStatus(int id,int status)
        {
            if (id > 0)
            {
               var  model = _reviewsService.UpdateStatus(id, status);
                _reviewsService.Save();
                return Json(new
                {
                    success = model
                });
            }
            return Json(new
            {
                success = false
            });

        }

        [HttpPost]
        public JsonResult Update(ReviewsModelView ReviewsModelView)
        {
            var result = _reviewsService.Update(ReviewsModelView);
            _reviewsService.Save();
            return Json(new
            {
                success = result
            });

        }        

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _reviewsService.Deleted(id);
            _reviewsService.Save();
            return Json(new
            {
                success = result
            });
        }

        [HttpGet]
        public JsonResult GetAllPaging(ReviewsViewModelSearch reviewsViewModelSearch)
        {
            if (reviewsViewModelSearch != null)
            {
                if (!string.IsNullOrEmpty(reviewsViewModelSearch.name))
                {
                    string textSeach = reviewsViewModelSearch.name.Trim();
                    OrdersViewModelSearch search = new OrdersViewModelSearch();
                    search.PageIndex = 1;
                    search.PageSize = 250;
                    search.name = textSeach;
                    var _order = _ordersService.GetAllPaging(search);
                    if (_order != null && _order.Results != null && _order.Results.Count > 0)
                    {
                        var order_ids = _order.Results.Select(o => o.id).ToList();
                        if (order_ids != null && order_ids.Count > 0)
                        {
                            reviewsViewModelSearch.order_ids = order_ids;
                        }
                    }
                    else
                    {

                        var _user = _appUserService.GetUserSearch(textSeach);
                        if (_user != null && _user.Count > 0)
                        {
                            var user_ids = _user.Select(u => u.id).ToList();
                            if (user_ids != null && user_ids.Count > 0)
                            {
                                search.name = "";
                                search.user_ids = user_ids;
                                var order_ids = _ordersService.GetAllPaging(search);
                                if (order_ids != null && order_ids.Results != null && order_ids.Results.Count > 0)
                                {
                                    reviewsViewModelSearch.order_ids = order_ids.Results.Select(o=>o.id).ToList();
                                }
                                
                            }
                        }
                    }
                }

            }
            var data = _reviewsService.GetAllPaging(reviewsViewModelSearch);
            foreach (var item in data.Results)
            {
                if (item.order_id.HasValue && item.order_id.Value > 0)
                {
                    var dataOrders = _ordersService.GetByid(item.order_id.Value);
                    if (dataOrders != null)
                    {
                        item.ordersModelView = dataOrders;

                        if (dataOrders.id > 0 && dataOrders.user_id.HasValue && dataOrders.user_id.Value > 0)
                        {
                            var dataUser = _appUserService.GetByid(dataOrders.user_id.Value);
                            if (dataUser != null)
                            {
                                item.userModelView = dataUser;
                            }
                        }
                    }
                }

            }
            return Json(new { data = data });
        }
    }
}
