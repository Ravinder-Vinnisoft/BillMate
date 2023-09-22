using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BillMate.Data;

namespace BillMate.Models
{
    public class Service : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //public DateTime BillingPeriodStart { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //public DateTime BillingPeriodEnd { get; set; }
        public string CostMetric { get; set; }
        public bool IsFlatFee { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal FeePercentage { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal FeeFlatValue { get; set; }
        public bool IsFeeSingleValue { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal FeeSingleValue { get; set; }
        public string FeeRanges { get; set; }
        public string AssignedClients { get; set; }

        public int UserId { get; set; }
    }
}
