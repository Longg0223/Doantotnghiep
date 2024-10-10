using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;

namespace TECH.Data.DatabaseEntity
{
    [Table("order_details")]
    public class OrdersDetails : DomainEntity<int>
    {
        public int? order_id { get; set; }
        [ForeignKey("order_id")]
        public Orders? Orders { get; set; }

        public int? product_id { get; set; }
        [ForeignKey("product_id")]
        public Products? Products { get; set; }

        //[Column(TypeName = "nvarchar(500)")]
        //public string? color { get; set; }
        [Column(TypeName = "decimal(18,0)")]
        public decimal? price { get; set; }
        //public int? sizeId { get; set; }
        public int? quantity { get; set; }
    }
}
