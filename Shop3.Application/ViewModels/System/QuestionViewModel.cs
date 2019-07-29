using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Shop3.Data.Enums;

namespace Shop3.Application.ViewModels.System
{
    public class QuestionViewModel
    {
        public int Id { set; get; }
        [StringLength(500)]
        public string Title { get; set; }
        public string Content { get; set; }
        public int DisplayOrder { get; set; }
        public Status Status { set; get; }
    }
}
