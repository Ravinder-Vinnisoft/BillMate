using System;
namespace BillMate.Models
{
	public class DentalPatient
	{
        public int ClientId { get; set; }
        public int PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public decimal? EstBalance { get; set; }
        public decimal? Bal_0_30 { get; set; }
        public decimal? Bal_31_60 { get; set; }
        public decimal? Bal_61_90 { get; set; }
        public decimal? BalOver90 { get; set; }
        public decimal? InsEst { get; set; }
        public decimal? BalTotal { get; set; }
        public DateTime? DateFirstVisit { get; set; }
        public int? PatStatus { get; set; }
        public int? GuarantorId { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }

        public string WirelessPhone { get; set; }
        public string Email { get; set; }

        public int BillingTypeId { get; set; }

        public string BillingType { get; set; }

        public int? NumberOfOpenClaims { get; set; }

        public DateTime? DateFutureVisit { get; set; }
    }
}

