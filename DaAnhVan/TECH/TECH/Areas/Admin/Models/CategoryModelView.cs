using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.Areas.Admin.Models
{
    public class CategoryModelView
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? slug { get; set; }
        public string? icon { get; set; }
        public int? status { get; set; }
        public string? status_str { get; set; }
        public DateTime created_at { get; set; }
        public string? created_str { get; set; }
        public DateTime? updated_at { get; set; }
        public string? updated_str { get; set; }
    }
    public class CategoryMenuCountModelView
    {
        public int id { get; set; }
        public string? category_name { get; set; }
        public int count_product { get; set; }
        public int product_id { get; set; }
    }
}
