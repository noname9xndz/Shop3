using Shop3.Application.ViewModels.Products;
using Shop3.Utilities.Dtos;

namespace Shop3.Models.BillViewModels
{
    public class MyBillViewModel
    {
        // public PagedResult<BillViewModel> ListBill { set; get; }
        // public List<BlogViewModel>
        public PagedResult<BillViewModel> ListBill { set; get; }
        public decimal OrderTotal { set; get; }
    }
}
