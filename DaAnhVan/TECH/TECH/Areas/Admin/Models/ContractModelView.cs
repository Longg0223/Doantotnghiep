using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.Areas.Admin.Models
{
    public class ContractModelView
    {
        public int id { get; set; }
        public string? full_name { get; set; }
        public string? phone_number { get; set; }
        public string? note { get; set; }
        public int? status { get; set; }
        public DateTime? created_at { get; set; }
        public string? datestr { get; set; }
    }
}
