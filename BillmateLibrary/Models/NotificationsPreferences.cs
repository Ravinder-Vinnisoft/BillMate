using System;
using System.ComponentModel.DataAnnotations;
using BillMate.Data;

namespace BillMate.Models
{
    public class NotificationsPreferences : BaseEntity
    {
		public int Id { get; set; }

		[Required]
		[StringLength(3)]
		public string NotifyInvoices { get; set; }

		[Required]
		[StringLength(3)]
		public string NotifyHelpRequest { get; set; }

		[Required]
		[StringLength(3)]
		public string NotifyAnnouncements { get; set; }

	}
}
