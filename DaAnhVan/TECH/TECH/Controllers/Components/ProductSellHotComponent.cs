using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;

namespace TECH.Controllers.Components

{
    public class ProductSellHotComponent : ViewComponent
    {
        private readonly IProductsService _productService;
        private readonly IImagesService _imagesService;
        private readonly IProductsImagesService _productsImagesService;
        private readonly ICategoryService _categoryService;
        public ProductSellHotComponent(IProductsService productsService,
           IImagesService imagesService,
           IProductsImagesService productsImagesService,
           ICategoryService categoryService)
        {
            _productService = productsService;
            _categoryService = categoryService;
            _imagesService = imagesService;
            _productsImagesService = productsImagesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var productModelViewSearch = new ProductViewModelSearch();
            productModelViewSearch.status = 2;
            productModelViewSearch.PageIndex = 1;
            productModelViewSearch.PageSize = 5;
            var categoryModel = _productService.GetAllPaging2(productModelViewSearch);
            var model = new List<ProductModelView>();
            if (categoryModel.Results != null && categoryModel.Results.Count > 0)
            {
                foreach (var item in categoryModel.Results)
                {
                    if (item.category_id.HasValue && item.category_id.Value > 0)
                    {
                        var category = _categoryService.GetByid(item.category_id.Value);
                        var productimages = _productsImagesService.GetImageProduct(item.id);
                        if (productimages != null && productimages.Count > 0)
                        {
                            var lstImages = _imagesService.GetImageName(productimages);
                            if (lstImages != null && lstImages.Count > 0)
                            {
                                item.avatar = lstImages[0].name;
                            }
                        }
                        if (category != null && !string.IsNullOrEmpty(category.name))
                        {
                            item.categorystr = category.name;
                        }
                        else
                        {
                            item.categorystr = "Chờ xử lý";
                        }
                    }
                    else
                    {
                        item.categorystr = "";
                    }

                    var productImage = _productsImagesService.GetImageProduct(item.id);
                    if (productImage != null && productImage.Count > 0)
                    {
                        var image = _imagesService.GetImageName(productImage);
                        if (image != null && image.Count > 0)
                        {
                            item.ImageModelView = image;
                        }
                    }
                    item.trademark = !string.IsNullOrEmpty(item.trademark) ? item.trademark : "";
                    item.price_sell_str = item.price_sell.HasValue && item.price_sell.Value > 0 ? item.price_sell.Value.ToString("#,###") : "";
                    item.price_import_str = item.price_import.HasValue && item.price_import.Value > 0 ? item.price_import.Value.ToString("#,###") : "";
                    item.price_reduced_str = item.price_reduced.HasValue && item.price_reduced.Value > 0 ? item.price_reduced.Value.ToString("#,###") : "";
                }
                model = categoryModel.Results.ToList();
            }
            return View(model);
        }
    }
}