using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;

namespace TECH.Data.DatabaseEntity
{
    [Table("product_images")]
    public class ProductImages : DomainEntity<int>
    {
        public int? product_id { get; set; }
        [ForeignKey("product_id")]
        public Products? Products { get; set; }
        public int? image_id { get; set; }
        [ForeignKey("image_id")]
        public Images? Images { get; set; }
    }
}
