using Microsoft.AspNetCore.Mvc;

namespace Shop3.Areas.Admin.Controllers
{
    public class SettingController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}