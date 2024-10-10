using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IProductQuantityRepository : IRepository<ProductQuantity, int>
    {
       
    }

    public class ProductQuantityRepository : EFRepository<ProductQuantity, int>, IProductQuantityRepository
    {
        public ProductQuantityRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
