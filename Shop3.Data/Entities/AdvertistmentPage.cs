using Shop3.Infrastructure.SharedKernel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop3.Data.Entities
{
    [Table("AdvertistmentPages")] // trang chứa quảng cáo
    public class AdvertistmentPage : DomainEntity<string>
    {
        public string Name { get; set; }

        public virtual ICollection<AdvertistmentPosition> AdvertistmentPositions { get; set; }
    }
}
