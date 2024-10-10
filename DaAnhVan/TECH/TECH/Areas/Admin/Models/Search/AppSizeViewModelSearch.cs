using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECH.Areas.Admin.Models.Search;

namespace TECH.Areas.Admin.Models
{
    public class AppSizeViewModelSearch : PageViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
