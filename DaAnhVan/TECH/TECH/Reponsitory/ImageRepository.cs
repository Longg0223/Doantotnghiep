using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IImageRepository : IRepository<Images, int>
    {
       
    }

    public class ImageRepository : EFRepository<Images, int>, IImageRepository
    {
        public ImageRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
