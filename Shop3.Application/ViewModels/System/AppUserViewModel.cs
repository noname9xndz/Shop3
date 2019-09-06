using Shop3.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shop3.Application.ViewModels.System
{
    public class AppUserViewModel
    {
        public AppUserViewModel()
        {
            Roles = new List<string>();
        }
        public Guid? Id { set; get; }

        [StringLength(70, MinimumLength = 3)]
        [Required]
        public string FullName { set; get; }
        [DataType(DataType.Date)]
        public DateTime? BirthDay { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
        public string UserName { set; get; }
        public string Address { get; set; }

        [StringLength(15, MinimumLength = 9)]
        public string PhoneNumber { set; get; }
        public string Avatar { get; set; }
        public Status Status { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateCreated { get; set; }
        public List<string> Roles { get; set; }
    }
}
