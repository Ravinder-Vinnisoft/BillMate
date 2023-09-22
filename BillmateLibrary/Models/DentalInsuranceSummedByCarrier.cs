using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BillMate.Data;

namespace BillMate.Models
{
    public class DentalInsuranceSummedByCarrier : BaseEntity
    {
        [Key]
        public int BillmateReportId { get; set; }
        public decimal PaymentEntered { get; set; }
        public string CarrierName { get; set; }      

       
        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        public Client Client { get; set; }
    }
}
