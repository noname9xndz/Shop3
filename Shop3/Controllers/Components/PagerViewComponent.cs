using Microsoft.AspNetCore.Mvc;
using Shop3.Utilities.Dtos;
using System.Threading.Tasks;

namespace Shop3.Controllers.Components
{
    public class PagerViewComponent : ViewComponent
    {
        // https://docs.microsoft.com/en-us/aspnet/core/mvc/views/view-components?view=aspnetcore-2.2
        public async Task<IViewComponentResult> InvokeAsync(PagedResultBase result)
        {
            return await Task.FromResult((IViewComponentResult)View("Default", result));

        }
    }
}
