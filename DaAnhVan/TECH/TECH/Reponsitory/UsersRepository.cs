using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IUsersRepository : IRepository<Users, int>
    {
       
    }

    public class UsersRepository : EFRepository<Users, int>, IUsersRepository
    {
        public UsersRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
