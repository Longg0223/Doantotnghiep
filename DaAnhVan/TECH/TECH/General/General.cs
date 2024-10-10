using Microsoft.CodeAnalysis.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace TECH.General
{
   public class General
    {
        public enum StaffStatus
        {
            Active = 1, // đang làm việc
            InActive = 2 // nghỉ làm việc
        }
        public enum OrdersStatus
        {
            Delivered = 1, // đã giao hàng
            Cancel = 2 // Trả lại hàng
        }
        public enum ProductStatus
        {
            Show = 1, // Sản phẩm hót
            Hide = 2, // Trả lại hàng
            Wait = 3
        }
    }
    public static class Common
    {
        public static string GetTrademark(int trademark)
        {
            if (trademark == 1)
            {
                return "Adidas";
            }
            if (trademark == 2)
            {
                return "Nike";
            }
            if (trademark == 3)
            {
                return "MLB Korea";
            }
            if (trademark == 4)
            {
                return "New Balance";
            }
            if (trademark == 5)
            {
                return "McQueen";
            }
            if (trademark == 6)
            {
                return "Converse";
            }
            return "Thương Hiệu khác";
        }
        public static string GetStatusStr(int staus)
        {
            if (staus == 1)
            {
                return "Hàng mới";
            }
            if (staus == 2)
            {
                return "Nổi bật";
            }
            return "";
        }
    }
}
