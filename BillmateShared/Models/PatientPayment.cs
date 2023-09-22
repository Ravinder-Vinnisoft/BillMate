using System;
namespace BillMate.Models
{
    public class PatientPayment
    {
        public int PatientPaymentId { get; set; }
        public int  ClientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EstInsuranceDue { get; set; }
        public string EstPatientDue { get; set; }
        public string TotalDue { get; set; }
        public string AgeOfBalance { get; set; }
        public string BillingType { get; set; }
        public string Hold { get; set; }
        public string OpenClaimsFamily { get; set; }
        public int PatientId { get; set; }
    }
}
