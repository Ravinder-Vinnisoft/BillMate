using System;
namespace BillMate.Models
{
    public class SchedulerConfiguration
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public int AttemptNumber { get; set; }
        public string AttemptName { get; set; }
        public int? SendAfterDays { get; set; }
        public bool? IsTextNotificationEnabled { get; set; }
        public bool? IsEmailNotificationEnabled { get; set; }
        public string TextMessage { get; set; }
        public string EmailMessage { get; set; }
        public DateTime StoredTime { get; set; }
    }
}
