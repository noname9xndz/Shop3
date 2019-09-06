using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shop3.Application.Interfaces;
using Shop3.Application.ViewModels.Common;
using System.Collections.Generic;
using System.Linq;

namespace Shop3.Areas.Admin.Controllers
{
    public class ContactController : BaseController
    {
        private readonly IContactService _contactService;
        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _contactService.GetAll();
            return new OkObjectResult(model);
        }
        [HttpGet]
        public IActionResult GetById(string id)
        {
            var model = _contactService.GetById(id);
            return new OkObjectResult(model);
        }
        [HttpPost]
        public IActionResult Delete(string id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            _contactService.Delete(id);
            _contactService.SaveChanges();

            return new OkObjectResult(id);

        }
        [HttpPost]
        public IActionResult SaveEntity(ContactViewModel contactViewModel)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            if (contactViewModel.Id == "0")
            {

                _contactService.Add(contactViewModel);
            }
            else
            {
                _contactService.Update(contactViewModel);
            }
            _contactService.SaveChanges();
            return new OkObjectResult(contactViewModel);
        }
    }
}