using Benday.InvoiceApp.Api;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Benday.InvoiceApp.WebUi.Models
{
    public class InvoiceViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Clients")]
        public List<SelectListItem> Clients { get; set; }

        [Required]
        [Display(Name = "Client Id")]
        public string ClientId { get; set; }

        public Invoice Invoice { get; set; }
    }
}
