using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface ICategoryRepository : IRepository<Category, int>
    {
       
    }

    public class CategoryRepository : EFRepository<Category, int>, ICategoryRepository
    {
        public CategoryRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
