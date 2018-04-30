using Benday.DataAccess.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Benday.InvoiceApp.Api
{
    public class InvoiceDbContext : DbContext
    {
        public InvoiceDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Client> Clients { get; set; }
    }
}
