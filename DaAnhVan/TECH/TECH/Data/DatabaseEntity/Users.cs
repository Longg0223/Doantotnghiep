using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;
using static TECH.General.General;
namespace TECH.Data.DatabaseEntity
{

    [Table("users")]
    public class Users : DomainEntity<int>
    {
        [Column(TypeName = "nvarchar(250)")]
        public string? full_name { get; set; }

        [Column(TypeName = "varchar(11)")]
        public string? phone_number { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? email { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string? avatar { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string? code { get; set; }

        public string? address { get; set; }

        public int? role { get; set; }

        [Column(TypeName = "varchar(32)")]
        public string? password { get; set; }

        public int? status { get; set; }

        public DateTime? register_date { get; set; }


    }
}
