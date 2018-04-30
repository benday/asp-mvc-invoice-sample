using Benday.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Benday.InvoiceApp.Api
{
    public class Invoice : IInt32Identity
    {
        public Invoice()
        {

        }

        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        [DisplayName("Invoice Number")]
        public string InvoiceNumber { get; set; }

        // [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        [DisplayName("Invoice Date")]
        public DateTime InvoiceDate { get; set; }

        public List<InvoiceLine> InvoiceLines { get; set; }

        [ForeignKey("Client")]
        public int OwnerClientIDFK { get; set; }

        [DisplayName("Client")]
        public Client Client { get; set; }
    }
}
