using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BillMate.Data;

namespace BillMate.Models
{
    public class DentalTotalClaims : BaseEntity
    {
        [Key]
        public int BillmateReportId { get; set; }
        public decimal PercentageOfClaims { get; set; }
        public decimal ClaimFees { get; set; }
        public decimal InsPayEst { get; set; }

        public decimal InsPaidAmt { get; set; }

        public string CarrierName { get; set; }

        public int NumberOfClaims { get; set; }

        public string Phone { get; set; }

        public int ClientId { get; set; }


        [ForeignKey("ClientId")]
        public Client Client { get; set; }
    }
}
