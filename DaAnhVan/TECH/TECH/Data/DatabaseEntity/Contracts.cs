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

    [Table("contacts")]
    public class Contracts : DomainEntity<int>
    {
        //[Key]
        //public int Id { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        public string? full_name { get; set; }

        [Column(TypeName = "varchar(11)")]
        public string? phone_number { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? note { get; set; }

        public int? status { get; set; }

        public DateTime? created_at { get; set; }


    }
}
