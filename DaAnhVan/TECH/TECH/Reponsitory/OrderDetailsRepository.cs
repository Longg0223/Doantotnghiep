using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IOrderDetailsRepository : IRepository<OrdersDetails, int>
    {
       
    }

    public class OrderDetailsRepository : EFRepository<OrdersDetails, int>, IOrderDetailsRepository
    {
        public OrderDetailsRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
