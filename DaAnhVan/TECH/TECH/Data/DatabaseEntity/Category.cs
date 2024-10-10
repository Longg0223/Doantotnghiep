using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;

namespace TECH.Data.DatabaseEntity
{
    [Table("categories")]
    public class Category: DomainEntity<int>
    {
        [Column(TypeName = "nvarchar(250)")]
        public string? name { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string? slug { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string? icon { get; set; }

        public int? status { get; set; }

        public DateTime created_at { get; set; }

        public DateTime? updated_at { get; set; }
        public int? isdetele { get; set; }

        public virtual ICollection<Products> Products { get; set; }
    }
}
