using Shop3.Data.Enums;
using Shop3.Data.Interfaces;
using Shop3.Infrastructure.SharedKernel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop3.Data.Entities
{
    [Table("SystemConfigs")]
    public class SystemConfig : DomainEntity<string>, ISwitchable
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        public string StringValue { get; set; }
        public int? IntValue { get; set; }

        public bool? BoolValue { get; set; }

        public DateTime? DateTimeValue { get; set; }

        public decimal? DecimalValue { get; set; }
        public Status Status { get; set; }
    }
}