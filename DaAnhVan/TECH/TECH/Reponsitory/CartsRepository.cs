using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface ICartsRepository : IRepository<Carts, int>
    {
       
    }

    public class CartsRepository : EFRepository<Carts, int>, ICartsRepository
    {
        public CartsRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
