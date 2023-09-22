using System;
namespace BillMate.Models
{
    public class ClientDetail
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }

        public string PracticeName { get; set; }
        public string PracticeAddress { get; set; }
        public string PracticeWebsite { get; set; }
        public string PracticePhoneNumber { get; set; }
        public string ReplyTo { get; set; }
        public string EmailName { get; set; }

        public bool? AutomaticPostPayments { get; set; }
        public bool? ExcludeBilling { get; set; }

        public string LogoURL { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
