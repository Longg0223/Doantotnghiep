using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.Areas.Admin.Models.Search
{
    public class PostsViewModelSearch : PageViewModel
    {
        public string? name { get; set; }
        public List<int> author_ids { get; set; }
        
    }
}
