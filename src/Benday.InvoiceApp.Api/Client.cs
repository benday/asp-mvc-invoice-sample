using Benday.DataAccess;

namespace Benday.InvoiceApp.Api
{
    public class Client : IInt32Identity
    {
        public Client()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
