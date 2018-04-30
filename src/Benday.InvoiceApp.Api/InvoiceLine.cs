using Benday.DataAccess;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Benday.InvoiceApp.Api
{
    public class InvoiceLine : IInt32Identity
    {
        public InvoiceLine()
        {

        }

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [DisplayName("Item")]
        public string ItemName { get; set; }

        [DisplayName("Description")]
        public string ItemDescription { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        // [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal Value { get; set; }

        [ForeignKey("ParentInvoice")]
        public int InvoiceID { get; set; }

        public Invoice ParentInvoice { get; set; }
    }
}
