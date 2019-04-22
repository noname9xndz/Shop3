using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop3.Controllers.Components
{
    public class MobileMenuViewComponent : ViewComponent
    {
        // https://docs.microsoft.com/en-us/aspnet/core/mvc/views/view-components?view=aspnetcore-2.2
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
