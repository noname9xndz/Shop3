using Shop3.Infrastructure.SharedKernel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop3.Data.Entities
{
    [Table("WholePrices")] // giá bán sỉ (1 sp giá 1000 nhưng 10sp giá 7000) 
    public class WholePrice : DomainEntity<int>
    {

        public int ProductId { get; set; }

        public int FromQuantity { get; set; }

        public int ToQuantity { get; set; }

        public decimal Price { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
