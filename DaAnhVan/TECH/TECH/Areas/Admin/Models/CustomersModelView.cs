using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.Areas.Admin.Models
{
    public class CustomersModelView
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? MatKhau { get; set; }
        public string? TenDangNhap { get; set; }
        public string? GioiTinh { get; set; }
        public DateTime? NgaySinh { get; set; }
        public bool IsDeleted { get; set; }
    }
}
