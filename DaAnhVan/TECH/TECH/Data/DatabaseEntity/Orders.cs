using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;

namespace TECH.Data.DatabaseEntity
{
    [Table("orders")]
    public class Orders : DomainEntity<int>
    {
        [Column(TypeName = "nvarchar(250)")]
        public string? code { get; set; }

        public int? user_id { get; set; }
        [ForeignKey("user_id")]
        public Users? Users { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? note { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        public string? full_name { get; set; }
        [Column(TypeName = "varchar(150)")]
        public string? phone_number { get; set; }

        public int? review { get; set; }

        public int payment { get; set; }

        public int? status { get; set; }

        public int? total { get; set; }

        public int? fee_ship { get; set; }

        public DateTime? created_at { get; set; }
    }
}
