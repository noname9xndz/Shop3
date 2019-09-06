using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using Shop3.Application.Interfaces;
using Shop3.Application.ViewModels.System;
using Shop3.Authorization;
using Shop3.Data.Enums;
using Shop3.Extensions;
using Shop3.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop3.Areas.Admin.Controllers
{
    // thư viện usermanger chỉ dùng những phương thức dạng bất đồng bộ
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IHubContext<Shop3Hub> _hubContext; // SignalR

        public UserController(IUserService userService, IAuthorizationService authorizationService, IHubContext<Shop3Hub> hubContext)
        {
            _userService = userService;
            _authorizationService = authorizationService;
            _hubContext = hubContext;
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
            if (userVm.Id == null)
            {

                var check = await _userService.AddAsync(userVm);
                if (check == true)
                {
                    var announcement = new AnnouncementViewModel() // create thông báo
                    {
                        Content = $"User {userVm.UserName} has been created",
                        DateCreated = DateTime.Now,
                        Status = Status.Active,
                        Title = "User created",
                        UserId = User.GetUserId(),
                        Id = Guid.NewGuid().ToString(),

                    };
                    await _hubContext.Clients.All.SendAsync("ReceiveMessage", announcement);
                }
                else
                {
                    return Json(new { status = "error", message = "fail create" });
                }

            }
            else
            {
                await _userService.UpdateAsync(userVm);
            }
            return new OkObjectResult(userVm);
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