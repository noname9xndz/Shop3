using Shop3.Data.Interfaces;
using Shop3.Infrastructure.SharedKernel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Shop3.Data.Entities
{
    public class VisitorStatistic : DomainEntity<int>, IDateTracking
    {
        [Required]
        [MaxLength(128)]
        public string IPAddress { get; set; }

        [Required]
        public DateTime? VisitedDate { set; get; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
