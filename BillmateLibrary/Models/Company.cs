using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BillMate.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [ForeignKey("AddressDetails")]
        public int AddressId { get; set; }
        public AddressDetails AddressDetails { get; set; }

        [Required]
        [StringLength(100)]
        public string TimeZone { get; set; }

        public string ImageUrl { get; set; }
    }
}
