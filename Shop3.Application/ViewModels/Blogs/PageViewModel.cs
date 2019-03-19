using Shop3.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shop3.Application.ViewModels.Blogs
{
    public class PageViewModel
    {
        public int Id { set; get; }
        [Required]
        [MaxLength(256)]
        public string Name { set; get; }

        [MaxLength(256)]
        [Required]
        public string Alias { set; get; }

        public string Content { set; get; }
        public Status Status { set; get; }
    }
}
