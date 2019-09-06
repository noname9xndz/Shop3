using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shop3.Application.Interfaces;
using Shop3.Application.ViewModels.Common;
using System.Collections.Generic;
using System.Linq;

namespace Shop3.Areas.Admin.Controllers
{
    public class SlideController : BaseController
    {
        private readonly ISlideService _slideService;
        public SlideController(ISlideService slideService)
        {
            _slideService = slideService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllSlidePagingByAlias(string keyword, int page, int pageSize)
        {
            var model = _slideService.GetAllSlidesPagingByGroupAlias(keyword, page, pageSize);
            return new OkObjectResult(model);
        }
        [HttpGet]
        public IActionResult GetAllSlideByAlias(string groupAlias)
        {
            var model = _slideService.GetAllSlidesByGroupAlias(groupAlias);
            return new OkObjectResult(model);
        }
        [HttpGet]
        public IActionResult GetSlideById(int id)
        {
            var model = _slideService.GetSlideById(id);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveSlide(SlideViewModel slide)
        {
            //slide.Status = true;
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            if (slide.Id == 0)
            {
                _slideService.AddSlide(slide);
            }
            else
            {
                _slideService.UpdateSlide(slide);
            }
            _slideService.Save();
            return new OkObjectResult(slide);
        }

        [HttpPost]
        public IActionResult DeleteSlideById(int id)
        {
            _slideService.DeleteSlideById(id);
            _slideService.Save();
            return new OkObjectResult(id);
        }

        [HttpPost]
        public IActionResult DeleteSlideByAlias(string alias)
        {
            _slideService.DeleteSlideByAlias(alias);
            _slideService.Save();
            return new OkObjectResult(alias);
        }
    }
}