using Microsoft.AspNetCore.Mvc;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;

namespace TECH.Areas.Admin.Controllers
{
    public class ProductQuantityController : BaseController
    {
        private readonly IProductQuantityService _productQuantityService;
        public ProductQuantityController(IProductQuantityService productQuantityService)
        {
            _productQuantityService = productQuantityService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetProductQuantityForProductId(int productId)
        {
            if (productId > 0)
            {
                var data = _productQuantityService.GetProductQuantity(productId);
                return Json(new
                {
                    Data = data,
                    Success = true
                });
            }
            return Json(new
            {                
                Success = false
            });

        }
        [HttpPost]
        public JsonResult Add(List<QuantityProductModelView> quantities)
        {
            try
            {
                if (quantities == null || quantities.Count == 0)
                {
                    return Json(new
                    {
                        success = false
                    });
                }
                _productQuantityService.Add(quantities);
                _productQuantityService.Save();
                return Json(new
                {
                    success = true
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false
                });
            }
           
        }
        [HttpPost]
        public JsonResult Update(List<QuantityProductModelView> quantities)
        {
            try
            {
                if (quantities == null || quantities.Count == 0)
                {
                    return Json(new
                    {
                        success = false
                    });
                }
                _productQuantityService.Update(quantities);
                _productQuantityService.Save();
                return Json(new
                {
                    success = true
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false
                });
            }
        }

        [HttpPost]
        public JsonResult Deleted(List<int> ids)
        {
            if (ids != null && ids.Count() > 0)
            {
                var result = _productQuantityService.Deleted(ids);
                _productQuantityService.Save();
                return Json(new
                {
                    success = result
                });
            }
            return Json(new
            {
                success = false
            }); ;
        }

    }
}
