using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BillMate.Models
{
    public class DentalTotalProduction
    {
        [Key]
        public int BillmateReportId { get; set; }
        public decimal Adjustments { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public decimal PatIncome { get; set; }

        public string Production { get; set; }

        public decimal TotIncome { get; set; }

        public decimal TotProd { get; set; }
        public decimal WriteOff { get; set; }
        public string PaymentType { get; set; }

        public int ClientId { get; set; }


        [ForeignKey("ClientId")]
        public Client Client { get; set; }
    }
}
