using System;
using System.Collections.Generic;
using BillMate.Data;

namespace BillMate.Models
{
    public class Invoice : BaseEntity
    {
        public int Id { get; set; }
        public long StartTime { get; set; }
        public long EndTime { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public decimal Amount { get; set; }
        public int UserId { get; set; }
        public InvoiceStatus Status { get; set; }
        public DateTime StoredTime { get; set; }

        public ICollection<InvoiceService> Services { get; set; }
    }

    public enum InvoiceStatus
    {
        Draft = -1,
        Approved,
        Sent,
        Settled
    }
}
