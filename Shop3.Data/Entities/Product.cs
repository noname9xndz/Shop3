using Shop3.Data.Enums;
using Shop3.Data.Interfaces;
using Shop3.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Shop3.Data.Entities
{
    [Table("Products")] // tên bản khi render ra
    public class Product : DomainEntity<int>, IHasSeoMetaData, ISwitchable, IDateTracking 
    {
        [StringLength(255)] // nếu ko cấu hình mặc định là nvarchar(Max)
        [Required] // bắt buộc phải nhập
        public string Name { get; set; }

        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")] // liên kế khóa ngoại 
        public virtual ProductCategory ProductCategory { set; get; } 
        // vitural là 1 thuộc tính ảo giúp lazyloading khi load bảng con có thể trỏ đến bảng có quan hệ với bảng chính  (cha)

        [StringLength(255)]
        public string Image { get; set; }

        [Required]
        [DefaultValue(0)] // mặc định giá trị là 0
        public decimal Price { get; set; }

        public decimal? PromotionPrice { get; set; } // ? có thể null

        [Required]
        public decimal OriginalPrice { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public string Content { get; set; }

        public bool? HomeFlag { get; set; }

        public bool? HotFlag { get; set; }

        public int? ViewCount { get; set; }

        [StringLength(255)]
        public string Tags { get; set; }

        public virtual ICollection<ProductTag> ProductTags { set; get; }

        [StringLength(255)]
        public string Unit { get; set; }
        


        public string SeoPageTitle { set; get; }

        [Column(TypeName ="varchar")] // chỉ ra kiểu dữ liệu thuộc tính mặc định là nvarchar(Max)
        [StringLength(255)]
        public string SeoAlias { set; get; }

        [StringLength(255)]
        public string SeoKeywords { set; get; }

        [StringLength(255)]
        public string SeoDescription { set; get; }

        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }
        public Status Status { set; get; }
    }
}
