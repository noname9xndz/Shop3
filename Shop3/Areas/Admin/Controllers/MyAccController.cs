using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shop3.Application.Interfaces;
using Shop3.Application.ViewModels.System;
using Shop3.Extensions;
using Shop3.Utilities.Constants;

namespace Shop3.Areas.Admin.Controllers
{
    public class MyAccController : BaseController
    {
        private readonly IUserService _userService;


        public MyAccController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetAccount()
        {
            if (User.Identity.IsAuthenticated == true)
            {
                string id = User.GetSpecificClaim(CommonConstants.UserClaims.UserId);
                var model = await _userService.GetById(id);

                return new OkObjectResult(model);
            }
            return new OkResult();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserByUser(AppUserViewModel userVm, string oldPassword, string confirmPassword)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {

                if (User.Identity.IsAuthenticated == true && userVm.Id == Guid.Parse(User.GetSpecificClaim(CommonConstants.UserClaims.UserId)))
                {


                    if (!await _userService.CheckPasswordUser(userVm.Id) && oldPassword == null)
                    {
                        await _userService.ChangePassByUserWithPasswordHashIsNull(userVm);
                        return new OkObjectResult(userVm);
                    }
                    else if (oldPassword == null && confirmPassword == null)
                    {
                        return new BadRequestResult();
                    }
                    else if (confirmPassword != null)
                    {
                        if (await _userService.ChangePassByUserAsync(userVm, oldPassword))
                            return new OkObjectResult(userVm);
                    }
                    else
                    {
                        if (await _userService.UpdateByUserAsync(userVm, oldPassword))
                            return new OkObjectResult(userVm);
                    }
                }


            }
            return new BadRequestResult();

        }

    }
}