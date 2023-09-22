using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BillMate.Models
{
    public class EmployeeTaskLogs
    {
		[Key]
		public int Id { get; set; }

		[Required]
		[StringLength(100)]
		public string TaskName { get; set; }

		[Required]
		public string Message { get; set; }

		[Required]
		public string AssignedTo { get; set; }

		[Required]
		[StringLength(100)]
		public string createdBy { get; set; }

		[Required]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime createdDate { get; set; }

	}
}
