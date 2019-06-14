using Shop3.Data.Interfaces;
using Shop3.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Shop3.Data.Entities
{
    [Table("WishProducts")]
    public class WishProduct : DomainEntity<int>
    {
        public WishProduct()
        {

        }
        public WishProduct(int id,int productId,Guid customerId)
        {
            Id = id;
            ProductId = productId;
            CustomerId = customerId ;
        }
        public int ProductId { set; get; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public Guid CustomerId { set; get; }
        [ForeignKey("CustomerId")]
        public virtual AppUser User { set; get; }
    }
}
