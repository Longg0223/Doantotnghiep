using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.Areas.Admin.Models
{
    public class CartsModelView
    {
        public int id { get; set; }
        public int? product_id { get; set; }
        public string? color { get; set; }
        public int? user_id { get; set; }
        public int? sizeId { get; set; }
        public string? sizeStr { get; set; }
        public decimal? price { get; set; }
        public string? pricestr { get; set; }
        public int? quantity { get; set; }

        public ProductModelView productModelView { get; set; }
        public UserModelView UserModelView { get; set; }
    }
}
