using Shop3.Application.ViewModels.Blogs;
using Shop3.Application.ViewModels.Products;
using System.Collections.Generic;

namespace Shop3.Models
{
    public class CustomMainMenuViewModel
    {
        public List<ProductCategoryViewModel> CategoryViewModels { set; get; }
        public List<PageViewModel> PageViewModels { set; get; }
    }
}
