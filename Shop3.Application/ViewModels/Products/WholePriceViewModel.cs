using System;
using System.Collections.Generic;
using System.Text;

namespace Shop3.Application.ViewModels.Products
{
    // giá bán sỉ sp với số lượng x -> số lượng y áp dụng với giá nào
    public class WholePriceViewModel
    {
        public int ProductId { get; set; }

        public int FromQuantity { get; set; }

        public int ToQuantity { get; set; }

        public decimal Price { get; set; }
    }
}
