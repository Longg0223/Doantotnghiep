using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IProductsRepository : IRepository<Products, int>
    {
       
    }

    public class ProductsRepository : EFRepository<Products, int>, IProductsRepository
    {
        public ProductsRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
