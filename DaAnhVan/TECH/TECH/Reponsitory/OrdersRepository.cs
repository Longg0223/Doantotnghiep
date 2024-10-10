using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IOrdersRepository : IRepository<Orders, int>
    {
       
    }

    public class OrdersRepository : EFRepository<Orders, int>, IOrdersRepository
    {
        public OrdersRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
