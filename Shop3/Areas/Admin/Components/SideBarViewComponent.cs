using Microsoft.AspNetCore.Mvc;
using Shop3.Application.Interfaces;
using Shop3.Application.ViewModels.System;
using Shop3.Extensions;
using Shop3.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Shop3.Areas.Admin.Components
{
    // https://docs.microsoft.com/en-us/aspnet/core/mvc/views/view-components?view=aspnetcore-2.2

   // [ViewComponent(Name ="SideBar")]
    public class SideBarViewComponent : ViewComponent
    {
        private IFunctionService _functionService;

        public SideBarViewComponent(IFunctionService functionService)
        {
            _functionService = functionService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        { //ép kiểu User để lấy ra Roles nếu ko gọi được extentions từ User
            var roles = ((ClaimsPrincipal)User).GetSpecificClaim(CommonConstants.UserClaims.Roles);
            List<FunctionViewModel> functions;
            if (roles.Split(";").Contains(CommonConstants.AppRole.AdminRole))
            {
                functions = await _functionService.GetAll(string.Empty);
            }
            else
            {
                var rolesArr = roles.Split(";");
                functions = await _functionService.GetAllFuncByRoles(string.Empty, rolesArr);
            }
            return View(functions);
        }
    }
}