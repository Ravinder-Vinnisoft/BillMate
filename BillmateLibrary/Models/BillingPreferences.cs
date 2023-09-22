using System;
using System.ComponentModel.DataAnnotations;
using BillMate.Data;

namespace BillMate.Models
{
    public class BillingPreferences : BaseEntity
    {
		public int Id { get; set; }

		[Required]
		[StringLength(3)]
		public string IsFeesSchedule { get; set; }

		[Required]
		[StringLength(3)]
		public string ApplyPositiveAdjusts { get; set; }

		[Required]
		[StringLength(3)]
		public string AcceptsInsurancePayments { get; set; }

		[StringLength(200)]
		public string InterestPaymentsHandle { get; set; }

		[StringLength(200)]
		public string InactiveInsuranceHandle { get; set; }

		public string BillingPreferenceText { get; set; }
	}
}
