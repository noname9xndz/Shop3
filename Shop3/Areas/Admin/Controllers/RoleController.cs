using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using Shop3.Application.Interfaces;
using Shop3.Application.ViewModels.System;
using Shop3.Extensions;
using Shop3.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop3.Areas.Admin.Controllers
{
    // thư viện usermanger chỉ dùng những phương thức dạng bất đồng bộ
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;
        private readonly IHubContext<Shop3Hub> _hubContext; // SignalR

        public RoleController(IRoleService roleService, IHubContext<Shop3Hub> hubContext)
        {
            _roleService = roleService;
            _hubContext = hubContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAll()
        {
            var model = await _roleService.GetAllAsync();

            return new OkObjectResult(model);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            var model = await _roleService.GetById(id);

            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var model = _roleService.GetAllPagingAsync(keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(AppRoleViewModel roleVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            if (!roleVm.Id.HasValue)
            {
                var notificationId = Guid.NewGuid().ToString();
                var announcement = new AnnouncementViewModel()
                {
                    Title = "Role created",
                    DateCreated = DateTime.Now,
                    Content = $"Role {roleVm.Name} has been created",
                    Id = notificationId,
                    UserId = User.GetUserId()
                };
                var announcementUsers = new List<AnnouncementUserViewModel>()
                {
                    new AnnouncementUserViewModel(){AnnouncementId = notificationId,HasRead = false,UserId = User.GetUserId()}
                };
                await _roleService.AddAsync(announcement, announcementUsers, roleVm);

                await _hubContext.Clients.All.SendAsync("ReceiveMessage", announcement); // gửi cho tất cả vừa gửi vào db và client
                // _hubContext.Clients. ....
            }
            else
            {
                await _roleService.UpdateAsync(roleVm);
            }
            return new OkObjectResult(roleVm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            await _roleService.DeleteAsync(id);
            return new OkObjectResult(id);
        }



        [HttpPost]
        public IActionResult ListAllFunction(Guid roleId)
        {
            var functions = _roleService.GetListFunctionWithRole(roleId);
            return new OkObjectResult(functions);
        }


        [HttpPost]
        public IActionResult SavePermission(List<PermissionViewModel> listPermmission, Guid roleId)
        {
            _roleService.SavePermission(listPermmission, roleId);
            return new OkResult();
        }



    }
}