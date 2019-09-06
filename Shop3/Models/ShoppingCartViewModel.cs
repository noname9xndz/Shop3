using Shop3.Application.ViewModels.Products;

namespace Shop3.Models
{
    public class ShoppingCartViewModel
    {
        public ProductViewModel Product { set; get; }

        public int Quantity { set; get; }

        public decimal Price { set; get; }

        public ColorViewModel Color { get; set; }

        public SizeViewModel Size { get; set; }

        public decimal OrderTotal { set; get; } // tổng tiền
    }
}
