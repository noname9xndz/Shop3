using Shop3.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Shop3.Application.ViewModels.Common
{
    public class FeedbackViewModel
    {
        public int Id { set; get; }
        [StringLength(250)]
        [Required]
        public string Name { set; get; }

        [StringLength(250)]
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { set; get; }

        [StringLength(500)]
        [Required(ErrorMessage = "The Message is required")]
        [Display(Name = "Message")]
        public string Message { set; get; }

        public Status Status { set; get; }
        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }
    }
}
