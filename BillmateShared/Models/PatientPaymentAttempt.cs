using System;
using System.ComponentModel.DataAnnotations;

namespace BillMate.Models
{
    public class PatientPaymentAttempt
    {
        public int PatientPaymentAttemptId { get; set; }
      
        // Foreign key to the unique identifier for a client
        public int ClientId { get; set; }

        // Foreign key to the unique identifier for a patient
        public int PatientId { get; set; }

        // The number of the attempt 
        public int AttemptNumber { get; set; }

        // The date/time of the attempt
        public DateTime DateAttempted { get; set; }

        public DateTime StoredTime { get; set; }

        // Other possible useful fields:
        // The type of notification (e.g., email, text)
        public string NotificationType { get; set; }

        // The message sent in the attempt
        public string Message { get; set; }

        // The status or result of the attempt
        public string Status { get; set; }
    }
}
