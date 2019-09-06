using Shop3.Infrastructure.SharedKernel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop3.Data.Entities
{
    [Table("Colors")] // màu của sp
    public class Color : DomainEntity<int>
    {

        [StringLength(250)]
        public string Name
        {
            get; set;
        }

        [StringLength(250)]
        public string Code { get; set; }
    }
}
