using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.Areas.Admin.Models
{
    public class ReviewsModelView
    {
        public int id { get; set; }

        public int? order_id { get; set; }

        public int? product_id { get; set; }

        public DateTime? created_at { get; set; }

        public string? comment { get; set; }

        public int? status { get; set; }

        public int? star { get; set; }
        public string? create_atstr { get; set; }

        public UserModelView? userModelView { get; set; }

        public OrdersModelView? ordersModelView { get; set; }
    }
    public class ReviewStarModel
    {
        public int star { get; set; }
        public int review_count { get; set; }
    }


}
