using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BillMate.Data;

namespace BillMate.Models
{
    public class Duty : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Label { get; set; }
        public int TitleId { get; set; }

    }

}
