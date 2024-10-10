using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;

namespace TECH.Data.DatabaseEntity
{
    [Table("posts")]
    public class Posts : DomainEntity<int>
    {
        [Column(TypeName = "nvarchar(250)")]
        public string? title { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string? slug { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string? avatar { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? content { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? short_content { get; set; }
        
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }

        public int? status { get; set; }

        public int? author { get; set; }
    }
}
