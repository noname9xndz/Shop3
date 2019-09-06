using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shop3.Application.Interfaces;
using Shop3.Application.ViewModels.Blogs;
using Shop3.Utilities.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Shop3.Areas.Admin.Controllers
{
    public class BlogController : BaseController
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            //todo infinity loop
            var model = _blogService.GetAllPaging(keyword, page, pageSize);
            return new OkObjectResult(model);
        }
        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _blogService.GetById(id);

            return new OkObjectResult(model);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                _blogService.Delete(id);
                _blogService.Save();

                return new OkObjectResult(id);
            }
        }
        [HttpPost]
        public IActionResult SaveEntity(BlogViewModel blogVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                blogVm.SeoAlias = TextHelper.ToUnsignString(blogVm.Name);
                if (blogVm.Id == 0)
                {
                    _blogService.Add(blogVm);
                }
                else
                {
                    _blogService.Update(blogVm);
                }
                _blogService.Save();
                return new OkObjectResult(blogVm);
            }
        }
    }
}