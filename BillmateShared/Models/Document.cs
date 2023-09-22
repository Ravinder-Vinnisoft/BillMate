using System;
using System.ComponentModel.DataAnnotations.Schema;
using BillMate.Data;

namespace BillMate.Models
{
    public class Document : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Tag { get; set; }
        public string FileName { get; set; }
        public byte[] File { get; set; }
        public string FileType { get; set; }
        public DateTime StoredTime { get; set; }
        public int ClientId { get; set; }
        public int? UploadedByUserId { get; set; }
    }
}
