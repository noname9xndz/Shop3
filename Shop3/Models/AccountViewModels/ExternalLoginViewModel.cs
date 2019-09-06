using System.ComponentModel.DataAnnotations;

namespace Shop3.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        //[Required]
        //[EmailAddress]
        //public string Email { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string DOB { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        //public string Adress { get; set; } todo



    }
}
