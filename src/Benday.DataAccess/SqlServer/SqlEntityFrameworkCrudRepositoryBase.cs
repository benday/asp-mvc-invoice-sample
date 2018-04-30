using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Benday.DataAccess.SqlServer
{
    public abstract class SqlEntityFrameworkCrudRepositoryBase<T> :
        SqlEntityFrameworkRepositoryBase<T>, IRepository<T>
        where T : class, IInt32Identity
    {
        public SqlEntityFrameworkCrudRepositoryBase(
            DbContext context) : base(context)
        {

        }

        protected abstract DbSet<T> EntityDbSet
        {
            get;
        }

        public virtual void Delete(T deleteThis)
        {
            if (deleteThis == null)
                throw new ArgumentNullException("deleteThis", "deleteThis is null.");

            var entry = Context.Entry(deleteThis);

            if (entry.State == EntityState.Detached)
            {
                EntityDbSet.Attach(deleteThis);
            }

            EntityDbSet.Remove(deleteThis);

            Context.SaveChanges();
        }

        public virtual IList<T> GetAll()
        {
            return EntityDbSet.ToList();
        }

        public virtual T GetById(int id)
        {
            return (
                from temp in EntityDbSet
                where temp.Id == id
                select temp
                ).FirstOrDefault();
        }

        public virtual void Save(T saveThis)
        {
            if (saveThis == null)
                throw new ArgumentNullException("saveThis", "saveThis is null.");

            VerifyItemIsAddedOrAttachedToDbSet(
                EntityDbSet, saveThis);

            Context.SaveChanges();
        }
    }
}
