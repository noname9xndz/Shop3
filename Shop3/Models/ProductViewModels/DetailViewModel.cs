using Microsoft.AspNetCore.Mvc.Rendering;
using Shop3.Application.ViewModels.Common;
using Shop3.Application.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop3.Application.ViewModels.Custom;

namespace Shop3.Models.ProductViewModels
{
    public class DetailViewModel
    {
        public ProductViewModel Product { get; set; }
        
        public List<ProductViewModel> RelatedProducts { get; set; }

        public ProductCategoryViewModel Category { get; set; }

        public List<ProductImageViewModel> ProductImages { set; get; }

        public List<ProductViewModel> UpsellProducts { get; set; }

        public List<ProductViewModel> LastestProducts { get; set; }

        public List<ProductViewModel> HotProducts { set; get; }

        public List<TagViewModel> Tags { set; get; }


        public List<SelectListItem> Colors { set; get; }


        public List<SelectListItem> Sizes { set; get; }

        public bool Available { set; get; } // còn hàng hoặc hết hàng

        public  List<CustomProductTagViewModel> ListCustomProductTag {set; get; }
    }
}
