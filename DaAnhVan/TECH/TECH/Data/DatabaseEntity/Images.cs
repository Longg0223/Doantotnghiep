using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;

namespace TECH.Data.DatabaseEntity
{
    [Table("images")]
    public class Images : DomainEntity<int>
    {
        [Column(TypeName = "varchar(200)")]
        public string? name { get; set; }
        [Column(TypeName = "nvarchar(400)")]
        public string? alt { get; set; }
        public int? status { get; set; }
    }
}
