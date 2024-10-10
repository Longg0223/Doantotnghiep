using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.Areas.Admin.Models.Search
{
    public class ProductViewModelSearch : PageViewModel
    {
        public string? name { get; set; }
        public int? categoryId { get; set; }
        public int differentiate { get; set; }
        public int status { get; set; }
        
    }
}
