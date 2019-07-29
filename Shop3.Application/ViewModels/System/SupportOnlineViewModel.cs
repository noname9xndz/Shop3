using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Shop3.Data.Enums;

namespace Shop3.Application.ViewModels.System
{
    public class SupportOnlineViewModel
    {
        public int Id { set; get; }
        [StringLength(128)]
        public string Name { set; get; }
        [StringLength(128)]
        public string Skype { get; set; }
        [StringLength(128)]
        public string FaceBook { get; set; }
        [StringLength(128)]
        public string Yahoo { get; set; }
        [StringLength(128)]
        public string Pinterest { get; set; }
        public string Twitter { get; set; }
        [StringLength(128)]
        public string Google { get; set; }
        [StringLength(128)]
        public string Mobile { get; set; }
        [StringLength(128)]
        public string Email { get; set; }
        [StringLength(128)]
        public string Instagram { get; set; }
        [StringLength(128)]
        public string Youtube { get; set; }
        [StringLength(128)]
        public string Linkedin { get; set; }
        [StringLength(128)]
        public string Zalo { get; set; }
        [StringLength(128)]
        public string TimeOpenWindow { get; set; }
        [StringLength(500)]
        public string Other { get; set; }
        public int DisplayOrder { get; set; }
        public Status Status { set; get; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
