using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BillMate.Models
{
	public class PaymentMade
	{
        public int Id { get; set; }
        public DateTime DepositDaeTime { get; set; }
        public decimal Amount { get; set; }
        public int NumberOfAttempts { get; set; }
        public string TransactionID { get; set; }
        public string PaymentMethod { get; set; }
        public string BillingType { get; set; }
        public int PatientId { get; set; }

        [NotMapped]
        public DentalPatient Patient { get; set; }
        public int ClientId { get; set; }
    }
}

