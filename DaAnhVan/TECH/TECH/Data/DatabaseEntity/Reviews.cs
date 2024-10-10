using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;

namespace TECH.Data.DatabaseEntity
{
    [Table("reviews")]
    public class Reviews : DomainEntity<int>
    {
        public int? order_id { get; set; }
        [ForeignKey("order_id")]
        public Orders? Orders { get; set; }

        public int? product_id { get; set; }
        [ForeignKey("product_id")]
        public Products? Products { get; set; }
        
        public DateTime? created_at { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? comment { get; set; }

        public int? status { get; set; }

        public int? star { get; set; }
    }
}
