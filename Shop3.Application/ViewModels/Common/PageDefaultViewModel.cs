using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Shop3.Data.Enums;

namespace Shop3.Application.ViewModels.Common
{
   public class PageDefaultViewModel
    {
        public string Id { set; get; }

        [Required]
        [MaxLength(256)]
        public string Title { set; get; }

        [Required]
        public string Content { set; get; }

        public Status Status { set; get; }
    }
}
