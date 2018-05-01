using Benday.DataAccess;
using Benday.DataAccess.SqlServer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Benday.InvoiceApp.Api
{
    public class InvoiceRepository : 
        SqlEntityFrameworkCrudRepositoryBase<Invoice, InvoiceDbContext>, IRepository<Invoice>
    {

        public InvoiceRepository(
            InvoiceDbContext context) : 
            base(context)
        {

        }

        public override Invoice GetById(int id)
        {
            return (
                from temp in EntityDbSet.Include(d => d.InvoiceLines)
                where temp.Id == id
                select temp
                ).FirstOrDefault();
        }

        protected override DbSet<Invoice> EntityDbSet
        {
            get
            {
                return Context.Invoices;
            }
        }
    }
}
