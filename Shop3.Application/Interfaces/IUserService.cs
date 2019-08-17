using Shop3.Application.ViewModels.System;
using Shop3.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shop3.Application.ViewModels.Custom;

namespace Shop3.Application.Interfaces
{
    public interface IUserService
    {
        // thư viện usermanger chỉ dùng những phương thức dạng bất đồng bộ
        Task<bool> AddAsync(AppUserViewModel userVm);

        Task DeleteAsync(string id);

        Task<List<AppUserViewModel>> GetAllAsync();

        PagedResult<AppUserViewModel> GetAllPagingAsync(string keyword, int page, int pageSize);

        Task<AppUserViewModel> GetById(string id);

        Task<bool> ChangePassByUserAsync(AppUserViewModel userVm, string password);

        Task<bool> UpdateByUserAsync(AppUserViewModel userVm, string password);


        Task UpdateAsync(AppUserViewModel userVm);

        Task RemoveRolesFromUser(string userId, string[] roles);

        Task<bool> CheckPasswordUser(Guid? id);

        Task<AppUserViewModel> ChangePassByUserWithPasswordHashIsNull(AppUserViewModel userVm);

        Task<bool> FindUserByEmailOrUserName(string userOrName);
    }
}
