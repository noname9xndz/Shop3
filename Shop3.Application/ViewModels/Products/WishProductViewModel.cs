﻿using System;

namespace Shop3.Application.ViewModels.Products
{
    public class WishProductViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Guid CustomerId { set; get; }
    }
}
