using Shop3.Data.Enums;
using Shop3.Data.Interfaces;
using Shop3.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Shop3.Data.Entities
{
    [Table("ProductCategories")] // tên bản khi render ra
    // kế thừa các thuộc tính dùng chung khai báo trong Shop3.Data.Interfaces  và Shop3.Infrastructure.SharedKernel để dùng lại
    public class ProductCategory : DomainEntity<int>, IHasSeoMetaData, ISwitchable, ISortable, IDateTracking 
    {

        public ProductCategory()
        {// contructor giúp tránh trường hợp null
            Products = new List<Product>();
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? ParentId { get; set; }

        public int? HomeOrder { get; set; }

        public string Image { get; set; }

        public bool? HomeFlag { get; set; }

        public string SeoPageTitle { set; get; }
        public string SeoAlias { set; get; }
        public string SeoKeywords { set; get; }
        public string SeoDescription { set; get; }
        public Status Status { set; get; }
        public int SortOrder { set; get; }
        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }

        public virtual ICollection<Product> Products { set; get; } // chỉ ra quan hệ (cha) với bảng product
    }
}
