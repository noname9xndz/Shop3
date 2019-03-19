using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shop3.Application.ViewModels.Products
{
    public class ColorViewModel
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string Name
        {
            get; set;
        }

        [StringLength(250)]
        public string Code { get; set; }
    }
}
