using Shop3.Infrastructure.SharedKernel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop3.Data.Entities
{
    [Table("Sizes")]
    public class Size : DomainEntity<int>
    {

        [StringLength(250)]
        public string Name
        {
            get; set;
        }
    }
}
