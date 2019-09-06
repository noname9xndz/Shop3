using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Shop3.Application.Interfaces;
using Shop3.Application.ViewModels.Common;
using Shop3.Services;
using System.Threading.Tasks;
using Shop3.Application.Shared;

namespace Shop3.Areas.Admin.Controllers
{
    public class FeedBackController : BaseController
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly IViewRenderService _viewRenderService;

        public FeedBackController(IFeedbackService feedbackService,
            IViewRenderService viewRenderService,
            IConfiguration configuration,
            IEmailSender emailSender)
        {
            _feedbackService = feedbackService;
            _emailSender = emailSender;
            _configuration = configuration;
            _viewRenderService = viewRenderService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll(string keyword, int page, int pageSize)
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

        [HttpPost]
        public async Task<IActionResult> SendMailToUser(FeedbackViewModel FeedbackVM, string subject)
        {// todo error upload img with ckeditor
         // todo get mail send by admin
         // todo error return view to string 
            if (ModelState.IsValid)
            {
                _feedbackService.Add(FeedbackVM);
                _feedbackService.SaveChanges();
                var viewName = "../Areas/Admin/Views/FeedBack/_ContactMailToUser";
                var content = await _viewRenderService.RenderToStringAsync(viewName, FeedbackVM);
                await _emailSender.SendEmailAsync(FeedbackVM.Email, subject, content);


            }

            return new OkResult();
        }
    }
}