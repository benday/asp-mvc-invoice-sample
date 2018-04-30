using Benday.DataAccess;
using Benday.DataAccess.SqlServer;
using Microsoft.EntityFrameworkCore;
using System;

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

        protected override DbSet<Invoice> EntityDbSet
        {
            get
            {
                return Context.Invoices;
            }
        }
    }
}
