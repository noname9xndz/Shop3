using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shop3.Application.Interfaces;
using Shop3.Application.ViewModels.System;
using Shop3.Authorization;

namespace Shop3.Areas.Admin.Controllers
{
    // thư viện usermanger chỉ dùng những phương thức dạng bất đồng bộ
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;

        public UserController(IUserService userService,IAuthorizationService authorizationService)
        {
            _userService = userService;
            _authorizationService = authorizationService;
    }
        public async Task<IActionResult> Index()
        {
            // function user : url : admin/user/index
            var result = await _authorizationService.AuthorizeAsync(User, "USER", Operations.Read);

            if (result.Succeeded == false)
                return new RedirectResult("/Admin/Login/Index");

            return View();
        }
        public IActionResult GetAll()
        {
            var model = _userService.GetAllAsync();

            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var model = await _userService.GetById(id);

            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var model = _userService.GetAllPagingAsync(keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(AppUserViewModel userVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                if (userVm.Id == null)
                {
                    if (await _userService.AddAsync(userVm) == true)
                    {
                        return new OkObjectResult(userVm);
                    }
                    else
                    {
                        return Json(new { status = "error", message = "fail create" });
                    }
                   
                }
                else
                {
                    await _userService.UpdateAsync(userVm);
                    return new OkObjectResult(userVm);
                }
                
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                await _userService.DeleteAsync(id);

                return new OkObjectResult(id);
            }
        }
    }
}