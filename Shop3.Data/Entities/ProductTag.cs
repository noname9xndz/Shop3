using Shop3.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Shop3.Data.Entities
{
    public class ProductTag : DomainEntity<int>
    {
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { set; get; }

        [StringLength(50)]
        public string TagId { set; get; }
        [ForeignKey("TagId")]
        public virtual Tag Tag { set; get; }
    }
}
