using System.ComponentModel.DataAnnotations;

namespace Shop3.Models.AccountViewModels
{
    public class LoginViewModel
    {


        //[Required(ErrorMessage = "Required")]
        //[Display(Name = "UserName")]
        // public string UserName { get; set; }

        [Required(ErrorMessage = "The email address  or username is required")]
        [Display(Name = "Email or UserName")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]

        public string Password { get; set; }

        [Display(Name = "Remember")]
        public bool RememberMe { get; set; }
    }
}
