using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IContractsRepository : IRepository<Contracts, int>
    {
       
    }

    public class ContractsRepository : EFRepository<Contracts, int>, IContractsRepository
    {
        public ContractsRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
