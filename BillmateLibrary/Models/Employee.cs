using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BillMate.Data;

namespace BillMate.Models
{
    public class Employee : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Photo { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(200)]
        public string Name { get => $"{FirstName} {LastName}"; }

        [ForeignKey("AddressEmployee")]
        public int AddressId { get; set; }
        public AddressEmployee AddressEmployee { get; set; }

        [Required]
        [StringLength(100)]
        public string TimeZone { get; set; }

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string JobTitle { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        [Required]
        [StringLength(100)]
        public string WorkEmailAddress { get; set; }

        [Required]
        [StringLength(100)]
        public string JobCompensation { get; set; }

        [Required]
        [StringLength(100)]
        public string JobRole { get; set; }

        [StringLength(100)]
        public string AssignedManager { get; set; }

        [StringLength(100)]
        public string GoogleDriveLink { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EmploymentStartDate { get; set; }

        [StringLength(50)]
        public string ReviewsScheduled { get; set; }
        public DateTime CreatedDateTime { get; set; }

        //[Required]
        public string AssignOffices { get; set; }

        [NotMapped]
        public List<Duty> Duties { get; set; }

        [ForeignKey("User")]
        public int? UserId { get; set; }

        public User User { get; set; }

        public List<EmployeeOffices> EmployeeOffices { get; set; }

        //public string AssignedAdmin { get; set; }

    }
}
