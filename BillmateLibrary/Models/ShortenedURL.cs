using System;
using System.ComponentModel.DataAnnotations;

namespace BillMate.Models
{
    public class ShortenedURL
    {
        [Key]
        public int ShortenUrlId { get; set; }

        public string Url { get; set; }

        public string ShortUrl { get; set; }
        public string Token { get; set; }

        public int ClientId { get; set; }

        public int PatientId { get; set; }

        public DateTime StoredTime { get; set; }

        public DateTime? DateExpires { get; set; }
    }
}
