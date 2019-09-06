using System;
using System.ComponentModel.DataAnnotations;

namespace Shop3.Models.AccountViewModels
{
    public class RegisterViewModel
    {


        [Required(ErrorMessage = "Full name required", AllowEmptyStrings = false)]
        [Display(Name = "Full name")]
        public string FullName { set; get; }

        [Display(Name = "DOB")]
        public DateTime? BirthDay { set; get; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        // [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")] // compare : bằng password
        public string ConfirmPassword { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Phone number")]
        [Required(ErrorMessage = "Phone Number Required!")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
            ErrorMessage = "Entered phone format is not valid.")]
        public string PhoneNumber { set; get; }

        [Display(Name = "Avatar")]
        public string Avatar { get; set; }


    }
}
