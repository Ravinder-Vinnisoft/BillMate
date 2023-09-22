using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BillMate.Data;

namespace BillMate.Models
{
    public class Task : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TaskName { get; set; }

        [Required]
        public string TaskType { get; set; }

        [Required]
        [StringLength(20)]
        public string TaskNature { get; set; }

        [Required]
        [ForeignKey("Employee")]
        public int AssignedEmployee { get; set; }
        public Employee Employee { get; set; }

        [Required]
        [ForeignKey("Client")]
        public int AssignedClient { get; set; }
        public Client Client { get; set; }

        [StringLength(20)]
        public string TaskTag { get; set; }

        [StringLength(20)]
        public string TaskStatus { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }

        [Required]
        public string Comments { get; set; }

        public bool IsSelfCreated { get; set; }
        
        [Column("StoredTime")]
        public DateTime StoredTime { get; set; }
        
    }
}
