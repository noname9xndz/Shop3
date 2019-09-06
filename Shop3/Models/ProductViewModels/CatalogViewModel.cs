using Microsoft.AspNetCore.Mvc.Rendering;
using Shop3.Application.ViewModels.Products;
using Shop3.Utilities.Dtos;
using System.Collections.Generic;

namespace Shop3.Models.ProductViewModels
{
    public class CatalogViewModel
    {
        public PagedResult<ProductViewModel> Data { get; set; }

        public ProductCategoryViewModel Category { set; get; }

        public string SortType { set; get; }

        public int? PageSize { set; get; }


        //public List<SelectListItem> SortTypes { get; } = new List<SelectListItem>
        //{
        //    new SelectListItem(){Value = "lastest",Text = "Lastest"},
        //    new SelectListItem(){Value = "price",Text = "Price"},
        //    new SelectListItem(){Value = "name",Text = "Name"},
        //};

        public List<SelectListItem> SortTypes { get; } = new List<SelectListItem>
        {
            new SelectListItem(){Value = "Lastest",Text = "Lastest"},
            new SelectListItem(){Value = "Pricelowtohigh",Text = "Price low to high"},
            new SelectListItem(){Value = "Pricehightolow",Text = "Price high to low"},
            new SelectListItem(){Value = "name_az",Text = "Name A -> Z"},
            new SelectListItem(){Value = "name_za",Text = "Name Z -> A"},
        };

        public List<SelectListItem> PageSizes { get; } = new List<SelectListItem>
        {
            new SelectListItem(){Value = "12",Text = "12"},
            new SelectListItem(){Value = "24",Text = "24"},
            new SelectListItem(){Value = "48",Text = "48"},
        };
    }
}

