using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.Areas.Admin.Models.Search
{
    public class OrdersViewModelSearch : PageViewModel
    {
        public string? name { get; set; }        
        public int? user_id { get; set; }
        public List<int> user_ids { get; set; }
        public int? payment { get; set; }
        public int? status { get; set; }

    }
}
