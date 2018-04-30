using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Benday.DataAccess.SqlServer
{
    public abstract class SqlEntityFrameworkRepositoryBase<T> :
        IDisposable where T : class, IInt32Identity
    {
        public SqlEntityFrameworkRepositoryBase(
            DbContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context", "context is null.");

            _Context = context;
        }

        public void Dispose()
        {
            ((IDisposable)_Context).Dispose();
        }

        private DbContext _Context;

        protected DbContext Context
        {
            get
            {
                return _Context;
            }
        }

        protected void VerifyItemIsAddedOrAttachedToDbSet(DbSet<T> dbset, T item)
        {
            if (item == null)
            {
                return;
            }
            else
            {
                if (item.Id == 0)
                {
                    dbset.Add(item);
                }
                else
                {
                    var entry = _Context.Entry(item);

                    if (entry.State == EntityState.Detached)
                    {
                        dbset.Attach(item);
                    }

                    entry.State = EntityState.Modified;
                }
            }
        }
    }
}
