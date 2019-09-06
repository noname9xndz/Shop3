using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Shop3.WebApi.Controllers
{
    [Route("api/[controller]")] // router
    [Produces("application/json")] // dữ liệu trả về luon là json
                                   // ajax chỉ cung cấp get và post => resful cung cấp CRUD
    public class ApiControllerBase : Controller
    {

    }
}

