using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IProductsImageRepository : IRepository<ProductImages, int>
    {
       
    }

    public class ProductsImageRepository : EFRepository<ProductImages, int>, IProductsImageRepository
    {
        public ProductsImageRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
