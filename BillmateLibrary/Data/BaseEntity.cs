using System;
using BillMate.Models;

namespace BillMate.Data
{
    public abstract class BaseEntity
    {
        public int? CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
