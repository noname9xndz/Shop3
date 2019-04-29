using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Shop3.Application.Interfaces;
using Shop3.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop3.Controllers.Components
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        // https://docs.microsoft.com/en-us/aspnet/core/mvc/views/view-components?view=aspnetcore-2.2
        // https://docs.microsoft.com/en-us/aspnet/core/performance/caching/memory?view=aspnetcore-2.2
        // thảm khảo thêm  : https://viblo.asia/p/lam-viec-voi-distributed-cache-trong-aspnet-core-m68Z0O69KkG

        private IProductCategoryService _productCategoryService;
        private IMemoryCache _memoryCache; // phù hợp với các ứng dụng vừa và nhỏ sử dụng 1 server

        public CategoryMenuViewComponent(IProductCategoryService productCategoryService, IMemoryCache memoryCache)
        {
            _productCategoryService = productCategoryService;
            _memoryCache = memoryCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // có thể dùng get set
            var categories =  _memoryCache.GetOrCreate(CacheKeys.ProductCategories, entry => {
                entry.SlidingExpiration = TimeSpan.FromHours(2);
                return _productCategoryService.GetAll();
            });

            return View(categories);
        }
    }
}
