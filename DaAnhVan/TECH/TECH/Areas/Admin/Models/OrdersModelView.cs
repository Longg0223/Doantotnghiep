using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.Areas.Admin.Models
{
    public class OrdersModelView
    {
        public int id { get; set; }
        public string? code { get; set; }

        public int? user_id { get; set; }

        public string? note { get; set; }

        public int? review { get; set; }

        public string? ip_address { get; set; }

        public int payment { get; set; }

        public string? paymentstr { get; set; }

        public int? status { get; set; }
        public string? statusstr { get; set; }

        public int? total { get; set; }

        public string? totalstr { get; set; }
        public int? totalOrderDetail { get; set; }
        public string? totalOrderDetailStr { get; set; }

        public int? fee_ship { get; set; }
        public string? fee_shipstr { get; set; }

        public string? customerStr { get; set; }

        public string? created_atstr { get; set; }

        public DateTime? created_at { get; set; }

        public string? full_name { get; set; }
        public string? phone_number { get; set; }

        public UserModelView UserModelView { get; set; }

        public List<OrdersDetailModelView> OrdersDetailModelView { get; set; }
    }

    public class OrderStatistical
    {
        public int TotalWaitingProgressing { get; set; }
        public int TotalAccomplished { get; set; }
        public int Totalcancel { get; set; }
    }
}
