using Microsoft.AspNetCore.Mvc;
using Shop3.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop3.Controllers.Components
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        // https://docs.microsoft.com/en-us/aspnet/core/mvc/views/view-components?view=aspnetcore-2.2

        private IProductCategoryService _productCategoryService;
        public CategoryMenuViewComponent(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()

        {
            return View(_productCategoryService.GetAll());
        }
    }
}
