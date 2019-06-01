using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Shop3.Application.Interfaces;
using Shop3.Extensions;
using Shop3.SignalR;

namespace Shop3.Areas.Admin.Controllers
{

    public class AnnouncementController : BaseController
    {
        private readonly IAnnouncementService _announcementService;

        public AnnouncementController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllPaging(int page, int pageSize)
        {
            var model = _announcementService.GetAllUnReadPaging(User.GetUserId(), page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllAnnouncementOfUserPaging(int page, int pageSize)
        {
            var model = _announcementService.GetAll(User.GetUserId(), page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult MarkAsRead(string id)
        {
            var result = _announcementService.MarkAsRead(User.GetUserId(), id);
            return new OkObjectResult(result);
        }
        [HttpPost]
        public IActionResult MarkAsReadAll()
        {
            var result = _announcementService.MarkAsReadAll(User.GetUserId());
            return new OkObjectResult(result);
        }
        [HttpPost]
        public IActionResult Delete(string id)
        {
            var result = _announcementService.Delete(User.GetUserId(),id);
            return new OkObjectResult(result);
        }
        [HttpPost]
        public IActionResult DeleteAll(string key)
        {
            var result = _announcementService.DeleteAll(User.GetUserId(), key);
            return new OkObjectResult(result);
        }
        // to do send notifications to all users
    }
}
