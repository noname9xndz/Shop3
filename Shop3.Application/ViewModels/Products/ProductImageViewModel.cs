using System;
using System.Collections.Generic;
using System.Text;

namespace Shop3.Application.ViewModels.Products
{
    public class ProductImageViewModel
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public ProductViewModel Product { get; set; }

        public string Path { get; set; }

        public string Caption { get; set; }
    }
}
