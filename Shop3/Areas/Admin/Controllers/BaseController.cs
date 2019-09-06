using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Shop3.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Route("admin")]
    [Authorize]
    public class BaseController : Controller
    {

    }
}