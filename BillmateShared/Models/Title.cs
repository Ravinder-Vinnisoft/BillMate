using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BillMate.Data;

namespace BillMate.Models
{
    public class Title : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TitleName { get; set; }
        public int DepartmentId { get; set; }
        public ICollection<Duty> JobDuties { get; set; }


    }

}
