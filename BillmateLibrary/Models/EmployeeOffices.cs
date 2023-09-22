using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

namespace BillMate.Models
{
    public class EmployeeOffices
    {
        [Key, Column(Order = 0)]
        public int EmployeeId { get; set; }

     
        [Key, Column(Order = 1)]
        public int ClientId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
        [ForeignKey("ClientId")]
        public Client Client { get; set; }
    }
}
