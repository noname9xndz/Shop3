using Shop3.Infrastructure.SharedKernel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop3.Data.Entities
{
    [Table("AdvertistmentPositions")] // vị trí quảng cáo
    public class AdvertistmentPosition : DomainEntity<string>
    {

        // [StringLength(20)]
        public string PageId { get; set; }
        [ForeignKey("PageId")]
        public virtual AdvertistmentPage AdvertistmentPage { get; set; }

        [StringLength(250)]
        public string Name { get; set; }
        public virtual ICollection<Advertistment> Advertistments { get; set; }
    }
}
