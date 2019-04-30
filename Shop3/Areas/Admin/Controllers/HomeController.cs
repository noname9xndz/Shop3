using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop3.Application.Dapper.SqlCommands;
using Shop3.Extensions;

namespace Shop3.Areas.Admin.Controllers
{
    //[Area("Admin")]
    //[Route("admin")]
    public class HomeController : BaseController
    {
        private readonly IReportService _reportService;

        public HomeController(IReportService reportService)
        {
            _reportService = reportService;
        }

        public IActionResult Index()
        {
            var email = User.GetSpecificClaim("Email"); // lấy ra thông tin email để hiển thị lên view
            return View();
        }
        // vẽ biểu đồ
        public async Task<IActionResult> GetRevenue(string fromDate, string toDate)
        {
            return new OkObjectResult(await _reportService.GetReportAsync(fromDate, toDate));
        }
    }
}