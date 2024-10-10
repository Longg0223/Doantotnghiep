using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TECH.Service;

namespace TECH.Controllers.Components

{
    [ViewComponent(Name = "CategoryMenuComponent")]
    public class CategoryMenuComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        public CategoryMenuComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {           
            var categoryModel = _categoryService.GetAllMenu();
            return View(categoryModel);
        }
    }
}