using Benday.DataAccess;
using Benday.DataAccess.SqlServer;
using Microsoft.EntityFrameworkCore;
using System;

namespace Benday.InvoiceApp.Api
{
    public class ClientRepository :
        SqlEntityFrameworkCrudRepositoryBase<Client, InvoiceDbContext>, IRepository<Client>
    {

        public ClientRepository(
            InvoiceDbContext context) :
            base(context)
        {

        }

        protected override DbSet<Client> EntityDbSet
        {
            get
            {
                return Context.Clients;
            }
        }
    }
}
