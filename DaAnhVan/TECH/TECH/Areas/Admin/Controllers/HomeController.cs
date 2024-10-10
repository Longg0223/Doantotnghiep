using Microsoft.AspNetCore.Mvc;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;

namespace TECH.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IProductsService _productsService;
        private readonly ICategoryService _categoryService;
        private readonly IPostsService _postsService;
        private readonly IAppUserService _appUserService;
        private readonly IOrdersService _ordersService;
        public HomeController(
                IProductsService productsService,
                ICategoryService categoryService,
                IPostsService postsService,
                IOrdersService ordersService,
        IAppUserService appUserService
            )
        {
            _productsService = productsService;
            _categoryService = categoryService;
            _postsService = postsService;
            _appUserService = appUserService;
            _ordersService = ordersService;
        }

        public IActionResult Index()
        {
            
            var home = new HomeModelView();
            home.PostCount = _postsService.GetCount();
            home.ProductCount = _productsService.GetCount();
            home.CategoryCount = _categoryService.GetCount();
            home.AppUserCount = _appUserService.GetCount();
            return View(home);
        }

        [HttpGet]
        public JsonResult GetOrderStatistical()
        {
            var model = _ordersService.GetOrderStatistical();            
            return Json(new
            {
                Data = model
            });
        }

    }
}
