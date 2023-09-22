using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BillMate.Data;

namespace BillMate.Models
{
    public class DentalTotalCollection : BaseEntity
    {
        [Key]
        public int BillmateReportId { get; set; }

        public decimal PaymentAmt { get; set; }
        
        public string PaymentType { get; set; }

        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        public Client Client { get; set; }
    }
}
