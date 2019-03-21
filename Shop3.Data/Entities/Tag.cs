using Shop3.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shop3.Data.Entities
{
    public class Tag : DomainEntity<string> 
    {
        // mặc định id hiện tại ở dạng nvarchar(MAX) phải cấu hình lại cho giống TagId bên ProductTag (ko được sửa trong DomainEntity)
        // => sử dụng : confident api giúp ta config thuộc tính của entity (TagConfiguration )
        // xem  TagConfiguration và ModelBuilderExtensions trong Shop3.Data.EF

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [MaxLength(50)]
        [Required]
        public string Type { get; set; }
    }
}
