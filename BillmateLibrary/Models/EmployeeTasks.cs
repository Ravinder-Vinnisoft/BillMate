using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BillMate.Models
{
    public class EmployeeTasks
    {
		public int Id { get; set; }

		public bool ScheduleCall { get; set; }
		
		public bool GeneralSoftware { get; set; }

		public bool PatientAccount { get; set; }

		public bool Insurance { get; set; }

		public bool Preferences { get; set; }

		public bool SoftwareTraining { get; set; }

		public bool NewChat { get; set; }

	}
}
