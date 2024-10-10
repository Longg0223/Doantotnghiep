using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;

namespace TECH.Data.DatabaseEntity
{
    [Table("carts")]
    public class Carts : DomainEntity<int>
    {
        public int? product_id { get; set; }

        [ForeignKey("product_id")]
        public Products? Products { get; set; }


        //[Column(TypeName = "nvarchar(200)")]
        //public string? color { get; set; }

        public int? user_id { get; set; }
        [Column(TypeName = "decimal(18,0)")]
        public decimal? price { get; set; }
        //public int? sizeId { get; set; }

        public int? quantity { get; set; }
    }
}
