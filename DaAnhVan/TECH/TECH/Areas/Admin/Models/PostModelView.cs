using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.Areas.Admin.Models
{
    public class PostModelView
    {
        public int id { get; set; }
        public string? title { get; set; }
        public string? slug { get; set; }
        public string? avatar { get; set; }
        public string? content { get; set; }
        public string? author_name { get; set; }

        public string? short_content { get; set; }

        public string? endow { get; set; }

        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }

        public int? status { get; set; }

        public int? author { get; set; }
        public string? create_atstr { get; set; }
    }
   
}
