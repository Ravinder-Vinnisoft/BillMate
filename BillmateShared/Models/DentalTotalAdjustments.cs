using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BillMate.Data;

namespace BillMate.Models
{
    public class DentalTotalAdjustments : BaseEntity
    {
        [Key]
        public int BillmateReportId { get; set; }
        public int PatNum { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }

        public string Name { get => $"{FName} {LName}"; }

        public decimal AdjAmt { get; set; }       

        public string AdjDate { get; set; }

        public string Abbr { get; set; }

        public string ItemName { get; set; }

        public string AdjNote { get; set; }

        public int ClientId { get; set; }


        [ForeignKey("ClientId")]
        public Client Client { get; set; }
    }
}
