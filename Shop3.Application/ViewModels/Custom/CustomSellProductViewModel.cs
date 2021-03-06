﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Shop3.Application.ViewModels.Products;
using Shop3.Data.Enums;

namespace Shop3.Application.ViewModels.Custom
{
    public class CustomSellProductViewModel
    {
        public int Id { get; set; }

     
        public string Name { get; set; }

      
        public string Image { get; set; }

      
        public decimal Price { get; set; }
        

      
        public string Unit { get; set; }

        public int Total { get; set; }

    }
}
