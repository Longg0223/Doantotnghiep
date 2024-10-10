using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.Areas.Admin.Models
{
    public class QuantityProductModelView
    {
        public int Id { get; set; }
        //public ProductModelView Product { get; set; }
        //public AppSizeModelView AppSize { get; set; }
        public int? TotalImported { get; set; }
        public int? TotalSell { get; set; }
        public int? ProductId { get; set; }
        public int? AppSizeId { get; set; }
        public int? ColorId { get; set; }
        //public int? TotalSold { get; set; }
        //public int? TotalStock { get; set; }
        //public DateTime? DateImport { get; set; }
        //public string DateImportStr { get; set; }
        //public string AppSizeStr { get; set; }
        //public bool IsDeleted { get; set; }
    }
}
