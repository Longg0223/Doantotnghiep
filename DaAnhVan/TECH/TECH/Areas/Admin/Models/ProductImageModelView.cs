using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.Areas.Admin.Models
{
    public class ProductImageModelView
    {
        public int id { get; set; }
        public int? product_id { get; set; }
        public int? images_id { get; set; }
    }
}
