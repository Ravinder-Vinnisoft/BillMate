using System;
using System.ComponentModel.DataAnnotations;
using BillMate.Data;

namespace BillMate.Models
{
    public class AddressDetails : BaseEntity
    {
		public int Id { get; set; }

		[Required]
		[StringLength(100)]
		public string Address1 { get; set; }

		[StringLength(100)]
		public string Address2 { get; set; }

		[Required]
		[StringLength(50)]
		public string City { get; set; }

		[Required]
		[StringLength(50)]
		public string Province { get; set; }

		[Required]
		[StringLength(20)]
		public string PostalCode { get; set; }

		[Required]
		[StringLength(20)]
		public string Country { get; set; }
		public string Address { get => $"{Address1}, {Address2}, {City}, {Province}, {PostalCode}, {Country}"; }
	}
}
