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
    //[ViewComponent(Name = "CategoryMenuComponent")]
    public class ProductForCategoryComponent : ViewComponent
    {
        private readonly IProductsService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IImagesService _imagesService;
        private readonly IProductsImagesService _productsImagesService;
        public ProductForCategoryComponent(IProductsService productService,
            ICategoryService categoryService,
            IImagesService imagesService,
            IProductsImagesService productsImagesService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _imagesService = imagesService;
            _productsImagesService = productsImagesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categoryServer = _categoryService.GetAll();
            var productForCategory = new List<ProductForCategoryModelView>();
            if (categoryServer != null && categoryServer.Count > 0)
            {
                var productViewModelSearch = new ProductViewModelSearch();
                productViewModelSearch.PageSize = 10;
                productViewModelSearch.PageIndex = 1;
                foreach (var item in categoryServer)
                {
                    var productForCategoryModel = new ProductForCategoryModelView();
                    productForCategoryModel.CategoryModelView = item;
                    productViewModelSearch.categoryId = item.id;
                    var productModel = _productService.GetAllPaging(productViewModelSearch);
                    if (productModel != null && productModel.Results != null && productModel.Results.Count > 0)
                    {
                        productForCategoryModel.productModelViews = productModel.Results.Where(p=>p.ishidden != 1).ToList();

                        foreach (var itemProduct in productForCategoryModel.productModelViews)
                        {
                            if (itemProduct.category_id.HasValue && itemProduct.category_id.Value > 0)
                            {
                                var category = _categoryService.GetByid(itemProduct.category_id.Value);
                                var productimages = _productsImagesService.GetImageProduct(itemProduct.id);
                                if (productimages != null && productimages.Count > 0)
                                {
                                    var lstImages = _imagesService.GetImageName(productimages);
                                    if (lstImages != null && lstImages.Count > 0)
                                    {
                                        itemProduct.avatar = lstImages[0].name;
                                    }
                                }
                                if (category != null && !string.IsNullOrEmpty(category.name))
                                {
                                    itemProduct.categorystr = category.name;
                                }
                                else
                                {
                                    itemProduct.categorystr = "Chờ xử lý";
                                }

                            }
                            else
                            {
                                itemProduct.categorystr = "";
                            }
                            var productImage = _productsImagesService.GetImageProduct(itemProduct.id);
                            if (productImage != null && productImage.Count > 0)
                            {
                                var image = _imagesService.GetImageName(productImage);
                                if (image != null && image.Count > 0)
                                {
                                    itemProduct.ImageModelView = image;
                                }
                            }
                            itemProduct.trademark = !string.IsNullOrEmpty(itemProduct.trademark) ? itemProduct.trademark : "";
                            itemProduct.price_sell_str = itemProduct.price_sell.HasValue && itemProduct.price_sell.Value > 0 ? itemProduct.price_sell.Value.ToString("#,###") : "";
                            itemProduct.price_import_str = itemProduct.price_import.HasValue && itemProduct.price_import.Value > 0 ? itemProduct.price_import.Value.ToString("#,###") : "";
                            itemProduct.price_reduced_str = itemProduct.price_reduced.HasValue && itemProduct.price_reduced.Value > 0 ? itemProduct.price_reduced.Value.ToString("#,###") : "";
                            //item.total_product = 10;

                        }


                        productForCategory.Add(productForCategoryModel);
                    }
                }
            }                     
            return View(productForCategory);
        }
    }
}