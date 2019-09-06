using Microsoft.AspNetCore.Mvc;
using Shop3.Application.Interfaces;
using System.Threading.Tasks;

namespace Shop3.Controllers.Components
{
    public class MobileMenuViewComponent : ViewComponent
    {
        // https://docs.microsoft.com/en-us/aspnet/core/mvc/views/view-components?view=aspnetcore-2.2

        private IProductCategoryService _productCategoryService;

        public MobileMenuViewComponent(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(_productCategoryService.GetAll());
        }
    }
}
