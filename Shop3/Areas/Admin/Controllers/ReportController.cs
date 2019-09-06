using Microsoft.AspNetCore.Mvc;

namespace Shop3.Areas.Admin.Controllers
{
    public class ReportController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Reader()
        {
            return View();
        }
        public IActionResult Revenues()
        {
            return View();
        }
        public IActionResult Visitor()
        {
            return View();
        }

    }
}