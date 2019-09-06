using Microsoft.AspNetCore.Mvc;
using Shop3.Application.Dapper.SqlCommands;
using Shop3.Application.Interfaces;
using Shop3.Data.Enums;
using Shop3.Extensions;
using System.Threading.Tasks;

namespace Shop3.Areas.Admin.Controllers
{
    //[Area("Admin")]
    //[Route("admin")]
    public class HomeController : BaseController
    {
        private readonly IReportService _reportService;
        private readonly IProductService _productService;

        public HomeController(IReportService reportService, IProductService productService)
        {
            _reportService = reportService;
            _productService = productService;
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

        [HttpGet]
        public IActionResult GetTopSellProduct()
        {
            var model = _productService.GetProductsByStatusBill(5, BillStatus.Completed);
            return new OkObjectResult(model);

        }
    }
}