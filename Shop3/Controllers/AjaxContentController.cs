using Microsoft.AspNetCore.Mvc;

namespace Shop3.Controllers
{
    public class AjaxContentController : Controller
    {
        public IActionResult HeaderCart()
        {
            return ViewComponent("HeaderCart"); // load lại view component để cập nhật danh sách sp add vào giỏ hàng
        }
    }
}