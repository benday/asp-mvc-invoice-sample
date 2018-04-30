using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Benday.DataAccess.SqlServer
{
    public abstract class SqlEntityFrameworkRepositoryBase<TEntity, TDbContext> :
        IDisposable where TEntity : class, IInt32Identity
        where TDbContext : DbContext
    {
        public SqlEntityFrameworkRepositoryBase(
            TDbContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context", "context is null.");

            _Context = context;
        }

        public void Dispose()
        {
            ((IDisposable)_Context).Dispose();
        }

        private TDbContext _Context;

        protected TDbContext Context
        {
            get
            {
                return _Context;
            }
        }

        protected void VerifyItemIsAddedOrAttachedToDbSet(DbSet<TEntity> dbset, TEntity item)
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
                    var entry = _Context.Entry<TEntity>(item);

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
