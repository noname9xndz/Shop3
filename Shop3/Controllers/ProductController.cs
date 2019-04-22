using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Shop3.Controllers
{
    public class ProductController : Controller
    {
        //https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/routing?view=aspnetcore-2.2
        // khi view bất kỳ nào đó use route kiểu này thì view đó sẽ mất route mặc định trong startup
        [Route("products.html")] // điều hướng cho controller
        public IActionResult Index()
        {
            return View();
        }

        [Route("{alias}-c.{id}.html")]
        // danh muc sp
        public IActionResult Catalog(int id, string keyword, int? pageSize, string sortBy, int page = 1)
        {
            return View();
        }

    }
}