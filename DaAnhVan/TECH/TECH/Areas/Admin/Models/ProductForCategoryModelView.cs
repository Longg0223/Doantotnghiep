using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.Areas.Admin.Models
{
    public class ProductForCategoryModelView
    {

        public CategoryModelView CategoryModelView { get; set; }
        public List<ProductModelView> productModelViews {get;set;}
    }  
}
