//using Microsoft.AspNetCore.Mvc;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using TECH.Areas.Admin.Models;
//using TECH.Areas.Admin.Models.Search;
//using TECH.Service;

//namespace TECH.Controllers.Components

//{
//    //[ViewComponent(Name = "CategoryMenuComponent")]
//    public class ProductAllComponent : ViewComponent
//    {
//        private readonly IProductsService _productService;
//        private readonly IReviewsService _reviewsService;
//        public ProductAllComponent(IProductsService productService,
//            IReviewsService reviewsService)
//        {
//            _productService = productService;
//            _reviewsService = reviewsService;
//        }

//        public async Task<IViewComponentResult> InvokeAsync(int differentiate)
//        {
//            var colorViewModelSearch = new ProductViewModelSearch();
//            colorViewModelSearch.PageIndex = 1;
//            colorViewModelSearch.PageSize = 8;
//            colorViewModelSearch.differentiate = differentiate;
//            var categoryModel = _productService.GetAllPaging(colorViewModelSearch);
//            var model = new List<ProductModelView>();
//            if (categoryModel.Results != null && categoryModel.Results.Count > 0)
//            {
//                foreach (var item in categoryModel.Results)
//                {
//                    var review = _reviewsService.GetReviewForProduct(item.id);
//                    if (review != null && review.star > 0 && review.review_count > 0)
//                    {
//                        item.ProductViews = review;
//                    }
//                    else
//                    {
//                        item.ProductViews = null;
//                    }
//                }

//                model = categoryModel.Results.Where(p=>p.status != 1).ToList();
//            }
//            return View(model);
//        }
//    }
//}