using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TECH.Areas.Admin.Models;
using TECH.Service;

namespace TECH.Controllers.Components

{
    [ViewComponent(Name = "RelatedProductsComponent")]
    public class RelatedProductsComponent : ViewComponent
    {
        private readonly IProductsService _productService;
        private readonly IProductsImagesService _productsImagesService;
        public RelatedProductsComponent(IProductsService productService, IProductsImagesService productsImagesService)
        {
            _productService = productService;
            _productsImagesService = productsImagesService;
    }

        public async Task<IViewComponentResult> InvokeAsync(int? categoryId,int productId)
        {
            var model = new List<ProductModelView>();
            var lstdata = new List<ProductModelView>();
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                var productRelate = _productService.GetProductReLated(categoryId.Value, productId);
                if (productRelate != null && productRelate.Count > 0)
                {
                    lstdata = productRelate.ToList();
                }
            }
            return View(lstdata);
        }
    }
}