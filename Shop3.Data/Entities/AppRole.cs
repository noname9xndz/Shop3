using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop3.Data.Entities
{
    [Table("AppRoles")]
    public class AppRole : IdentityRole<Guid>
    {
        public AppRole() : base()
        {

        }
        // contructor của identity chỉ có name => viết lại contructor để add thêm name vào
        public AppRole(string name, string description) : base(name)
        {
            this.Description = description;
        }

        [StringLength(250)]
        public string Description { get; set; }
    }
}