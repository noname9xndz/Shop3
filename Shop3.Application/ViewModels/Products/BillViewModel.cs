using Shop3.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shop3.Application.ViewModels.Products
{
    public class BillViewModel
    {
        public int Id { get; set; }

        
        [MaxLength(256)]
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        public string CustomerName { set; get; }

        
        [MaxLength(256)]
        [Required(ErrorMessage = "Address is required")]
        [Display(Name = "Address")]
        public string CustomerAddress { set; get; }

        
        [MaxLength(50)]
        [Display(Name = "Phone number")]
        [Required(ErrorMessage = "Phone Number Required!")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
            ErrorMessage = "Entered phone format is not valid.")]
        public string CustomerMobile { set; get; }

        [MaxLength(50)]
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string CustomerEmail { set; get; }

        [Required]
        [MaxLength(256)]
        public string CustomerMessage { set; get; }

        public PaymentMethod PaymentMethod { set; get; }

        public BillStatus BillStatus { set; get; }

        public DateTime DateCreated { set; get; }

        public DateTime DateModified { set; get; }

        public Status Status { set; get; }

        public Guid? CustomerId { set; get; }

        public List<BillDetailViewModel> BillDetails { set; get; }

        public decimal OrderTotal { set; get; }

        [MaxLength(500)]
        public string ReOrderMesssage { set; get; }
    }
}
