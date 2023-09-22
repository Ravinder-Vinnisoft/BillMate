using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BillMate.Data;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

namespace BillMate.Models
{
    public class Client : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string PracticeName { get; set; }

        [ForeignKey("AddressDetails")]
        public int AddressId { get; set; }
        public AddressDetails AddressDetails { get; set; }

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string PrimaryContact { get; set; }

        [Required]
        [StringLength(100)]
        public string ClaimsSoftware { get; set; }

        [Required]
        [StringLength(100)]
        public string ReferralSource { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime OnboardDate { get; set; }

        [Required]
        [StringLength(50)]
        public string FormOfPayment { get; set; }

        [Required]
        [StringLength(100)]
        public string TimeZone { get; set; }

        [StringLength(100)]
        public string GoogleDriveLink { get; set; }

        [StringLength(50)]
        public string BillingDepartmentNumber { get; set; }
        public string PracticeLogo { get; set; }

        [ForeignKey("BillingPreferences")]
        public int BillingId { get; set; }
        public BillingPreferences BillingPreferences { get; set; }

        [ForeignKey("NotificationsPreferences")]
        public int NotificationId { get; set; }
        public NotificationsPreferences NotificationsPreferences { get; set; }

        [ForeignKey("User")]
        public int? UserId { get; set; }

        public User User { get; set; }

        [NotMapped]
        public Employee Employee { get; set; }
    }
}
