using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BillMate.Data;

namespace BillMate.Models
{
    public class DentalOutstandingClaims : BaseEntity
    {
        [Key]
        public int BillmateReportId { get; set; }
        public int PatNum { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }

        public string Name { get => $"{FName} {LName}"; }

        public DateTime? DateService { get; set; }

        public DateTime? DateSent { get; set; }

        public string CarrierName { get; set; }

        public string Phone { get; set; }

       
        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        public Client Client { get; set; }
    }
}
