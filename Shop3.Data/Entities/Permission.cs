using Shop3.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Shop3.Data.Entities
{
    [Table("Permissions")] // bảng phân quyền(chỉ ra role này với funtion này có quyền gì)
    public class Permission : DomainEntity<int>
    {

        public Permission()
        {

        }

        public Permission(Guid roleId, string functionId, bool canCreate, bool canRead, bool canUpdate, bool canDelete)
        {
            RoleId = roleId;
            FunctionId = functionId;
            CanCreate = canCreate;
            CanRead = canRead;
            CanUpdate = canUpdate;
            CanDelete = canDelete;
        }


        [Required]
        public Guid RoleId { get; set; }

       // [StringLength(128)] 
        [Required]
        public string FunctionId { get; set; }
        [ForeignKey("FunctionId")]
        public virtual Function Function { get; set; }

        // có thể gom các quyền lại thành 1 nhóm để trỏ qua bảng này
        public bool CanCreate { set; get; }
        public bool CanRead { set; get; }

        public bool CanUpdate { set; get; }
        public bool CanDelete { set; get; }


        [ForeignKey("RoleId")]
        public virtual AppRole AppRole { get; set; }

        
    }
}
