using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.SharedKernel
{
    public abstract class DomainEntity<T>
    {
        public T id { get; set; }

        /// <summary>
        /// True if domain entity has an identity
        /// </summary>
        /// <returns></returns>
        public bool IsTransient()
        {
            return id.Equals(default(T));
        }
    }
}
