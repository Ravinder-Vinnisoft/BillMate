using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BillMate.Data;

namespace BillMate.Models
{
    public class DentalWriteOffs : BaseEntity
    {
        [Key]
        public int BillmateReportId { get; set; }
        public int PatNum { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }

        public string Name { get => $"{FName} {LName}"; }

        public decimal Amount { get; set; }

        public string Abbr { get; set; }

        public string ClaimDate { get; set; }

        public string CarrierName { get; set; }

        public string ClaimNum { get; set; }

        public int ClientId { get; set; }


        [ForeignKey("ClientId")]
        public Client Client { get; set; }
    }
}
