using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BillMate.Models
{
	public class PaymentRequested
	{
        public int Id { get; set; }
        public DateTime SentDateTime { get; set; }
        public decimal TotalDue { get; set; }
        public decimal? AmountPaid { get; set; }
        public int NumberOfAttempts { get; set; }
        public DateTime? FutureVisitDate { get; set; }
        public string TransactionID { get; set; }
        public int PatientId { get; set; }

        [NotMapped]
        public DentalPatient Patient { get; set; }
        public int ClientId { get; set; }

        public int? CurrentScheduleId { get; set; }
        public int? NextScheduleId { get; set; }
    }
}

