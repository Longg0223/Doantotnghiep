using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.Areas.Admin.Models
{
    public class ProductModelView
    {
        public int id { get; set; }
        public int? category_id { get; set; }
        public string? categorystr { get; set; }
        public string? name { get; set; }

        public string? slug { get; set; }

        public string? avatar { get; set; }

        public decimal? price { get; set; }

        public string? color { get; set; }

        public int quantity { get; set; }

        public string? short_desc { get; set; }

        public string? description { get; set; }

        public string? specifications { get; set; }

        public string? shortdescription { get; set; }

        public string? endow { get; set; }

        public int? status { get; set; }
        public string? statusstr { get; set; }

        public int? type { get; set; }
        public int? differentiate { get; set; } // hàng mới hay cũ

        public int total_product { get; set; }
        public ProductViews ProductViews { get; set; }


        public decimal? price_sell { get; set; }

        public decimal? price_reduced { get; set; }
        public decimal? price_import { get; set; }


        public string? price_sell_str { get; set; }

        public string? price_reduced_str { get; set; }
        public string? price_import_str { get; set; }

        public List<ImageModelView> ImageModelView { get; set; }
 
        public string? trademark { get; set; }
        public int? trademarkId { get; set; }
        public string? trademarkStr { get; set; }
        public int? ishidden { get; set; }
        public int? number_import { get; set; }
        public int? cover_type { get; set; }
        public string? cover_type_str { get; set; }


    }
    public class ProductViews
    {
        public int star { get; set; }
        public int review_count { get; set; }
    }

    public class ProductViewsComponent
    {
        public int status { get; set; }
        public List<ProductModelView> Products { get; set; }
    }


}
