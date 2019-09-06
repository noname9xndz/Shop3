using Microsoft.AspNetCore.Mvc;

namespace Shop3.Areas.Admin.Controllers
{
    public class ErrorController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}