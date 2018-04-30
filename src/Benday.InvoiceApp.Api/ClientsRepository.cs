using Benday.DataAccess;
using Benday.DataAccess.SqlServer;
using Microsoft.EntityFrameworkCore;
using System;

namespace Benday.InvoiceApp.Api
{
    public class ClientsRepository :
        SqlEntityFrameworkCrudRepositoryBase<Client, InvoiceDbContext>, IRepository<Client>
    {

        public ClientsRepository(
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
