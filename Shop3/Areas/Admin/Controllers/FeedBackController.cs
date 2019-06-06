using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop3.Application.Interfaces;

namespace Shop3.Areas.Admin.Controllers
{
    public class FeedBackController : BaseController
    {
        private readonly IFeedbackService _feedbackService;

        public FeedBackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll(string keyword,int page,int pageSize)
        {
            var model = _feedbackService.GetAllPaging(keyword, page, pageSize);
            if (model.Results.Count == 0)
            {
                return Json(new { status = "error", message = "No records found" });
            }
            return new OkObjectResult(model);
        }
        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _feedbackService.GetById(id);
            return new OkObjectResult(model);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            _feedbackService.Delete(id);
            _feedbackService.SaveChanges();

            return new OkObjectResult(id);
        }
        // todo create send mail to user
        // css client
    }
}