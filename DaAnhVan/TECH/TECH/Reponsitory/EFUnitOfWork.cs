using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Call save change from db context
        /// </summary>
        void Commit();
    }
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly DataBaseEntityContext _context;
        public EFUnitOfWork(DataBaseEntityContext context)
        {
            _context = context;
        }
        public void Commit()
        {
            _context.SaveChanges();
        }        

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
