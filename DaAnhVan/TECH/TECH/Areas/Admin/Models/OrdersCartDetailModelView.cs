using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.Areas.Admin.Models
{
    public class OrdersCartDetailModelView
    {
       public UserModelView UserModelView { get; set; }
        public List<CartsModelView> CartsModelView { get; set; }
    }
}
