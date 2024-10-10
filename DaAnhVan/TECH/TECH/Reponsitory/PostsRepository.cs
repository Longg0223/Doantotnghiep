using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IPostsRepository : IRepository<Posts, int>
    {
       
    }

    public class PostsRepository : EFRepository<Posts, int>, IPostsRepository
    {
        public PostsRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
