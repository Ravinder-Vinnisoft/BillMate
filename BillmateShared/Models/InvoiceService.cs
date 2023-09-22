using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BillMate.Models
{
    public class InvoiceService
    {
        [Key, Column(Order = 0)]
        public int InvoiceId { get; set; }

        [Key, Column(Order = 1)]
        public int ServiceId { get; set; }

        [ForeignKey("InvoiceId")]
        public Invoice Invoice { get; set; }
        [ForeignKey("ServiceId")]
        public Service Service { get; set; }

        public decimal Amount { get; set; }
    }
}
