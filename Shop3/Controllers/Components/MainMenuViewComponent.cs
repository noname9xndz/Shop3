using Microsoft.AspNetCore.Mvc;
using Shop3.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop3.Models;

namespace Shop3.Controllers.Components
{
    public class MainMenuViewComponent : ViewComponent
    {
        // https://docs.microsoft.com/en-us/aspnet/core/mvc/views/view-components?view=aspnetcore-2.2
        private IProductCategoryService _productCategoryService;
        private IPageService _pageService;

        public MainMenuViewComponent(IProductCategoryService productCategoryService, IPageService pageService)
        {
            _productCategoryService = productCategoryService;
            _pageService = pageService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            CustomMainMenuViewModel customMainMenu = new CustomMainMenuViewModel();
            customMainMenu.CategoryViewModels = _productCategoryService.GetAll();
            customMainMenu.PageViewModels = _pageService.GetAll();
            return View(customMainMenu);
            //CustomMainMenuViewModel
        }
    }
}
