using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IReviewsRepository : IRepository<Reviews, int>
    {
       
    }

    public class ReviewsRepository : EFRepository<Reviews, int>, IReviewsRepository
    {
        public ReviewsRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
