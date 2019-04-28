using Shop3.Application.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop3.Models.ProductViewModels
{
    public class SearchResultViewModel : CatalogViewModel 
    {
        public string Keyword { get; set; }
        public List<ProductCategoryViewModel> Categories { set; get; }
    }
}
