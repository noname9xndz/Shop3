using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shop3.Application.Interfaces;
using Shop3.Application.ViewModels.Common;
using Shop3.Data.Enums;

namespace Shop3.Areas.Admin.Controllers
{
    public class PageDefaultController : BaseController
    {
        private readonly IPageDefaultService _pageDefaultService;

        public PageDefaultController(IPageDefaultService pageDefaultService)
        {
            _pageDefaultService = pageDefaultService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAllPageDefault()
        {
           var model =  _pageDefaultService.GetAllPageDefault();
           return new  OkObjectResult(model);
        }

        public IActionResult GetPageDefaultById(string id)
        {
            var model = _pageDefaultService.GetPageDefault(id);
            return new OkObjectResult(model);
        }

        public IActionResult UpdatePageDefault(PageDefaultViewModel pageDefault)
        {
            pageDefault.Status = Status.Active;
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                _pageDefaultService.Update(pageDefault);
                _pageDefaultService.SaveChange();
                return new OkObjectResult(pageDefault);
            }
            
        }
    }
}