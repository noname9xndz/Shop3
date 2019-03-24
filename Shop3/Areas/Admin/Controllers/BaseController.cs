using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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