using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;

namespace TECH.Data.DatabaseEntity
{
    [Table("product_quantity")]
    public class ProductQuantity : DomainEntity<int>
    {
        public int? product_id { get; set; }
        [ForeignKey("product_id")]
        public Products? Products { get; set; }

        public int? color_id { get; set; }
        [ForeignKey("color_id")]
        //public Colors? Colors { get; set; }

        public int? size_id { get; set; }
        [ForeignKey("size_id")]
        //public Size? size { get; set; }
        public int? totalimport { get; set; }
        public int? totalsell { get; set; }
        public int? totalinventory { get; set; }
        public int status { get; set; }
    }
}
