using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop3.Application.ViewModels.Blogs;
using Shop3.Application.ViewModels.Products;

namespace Shop3.Models
{
    public class CustomMainMenuViewModel
    {
        public List<ProductCategoryViewModel> CategoryViewModels { set; get; }
        public List<PageViewModel> PageViewModels { set; get; }
    }
}
