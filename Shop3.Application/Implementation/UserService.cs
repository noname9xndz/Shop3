using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shop3.Application.Interfaces;
using Shop3.Application.ViewModels.System;
using Shop3.Data.EF;
using Shop3.Data.Entities;
using Shop3.Utilities.Dtos;
using Shop3.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop3.Application.Implementation
{
    // thư viện usermanger chỉ dùng những phương thức dạng bất đồng bộ
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserService(UserManager<AppUser> userManager, IMapper mapper,
            RoleManager<AppRole> roleManager,
            AppDbContext context)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<bool> AddAsync(AppUserViewModel userVm)
        {
            var user = new AppUser()
            {
                UserName = userVm.UserName,
                Avatar = userVm.Avatar,
                Email = userVm.Email,
                FullName = userVm.FullName,
                DateCreated = DateTime.Now,
                PhoneNumber = userVm.PhoneNumber,
                Status = userVm.Status
            };

            var result = await _userManager.CreateAsync(user, userVm.Password);
            // if (result.Succeeded && userVm.Roles.Count > 0)
            if (result.Succeeded)
            {
                var appUser = await _userManager.FindByNameAsync(user.UserName);

                if (appUser != null && userVm.Roles.Count > 0)
                {
                    await _userManager.AddToRolesAsync(appUser, userVm.Roles);

                }
                return true;

            }
            else
            {
                return false;
            }

        }

        public async Task DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);
        }

        public async Task<List<AppUserViewModel>> GetAllAsync()
        {
            //return await _userManager.Users.ProjectTo<AppUserViewModel>().ToListAsync();
            return await _mapper.ProjectTo<AppUserViewModel>(_userManager.Users).ToListAsync();
        }

        public PagedResult<AppUserViewModel> GetAllPagingAsync(string keyword, int page, int pageSize)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.FullName.Contains(keyword)
                || x.UserName.Contains(keyword)
                || x.Email.Contains(keyword));

            int totalRow = query.Count();
            query = query.Skip((page - 1) * pageSize)
               .Take(pageSize);

            var data = query.Select(x => new AppUserViewModel()
            {
                UserName = x.UserName,
                Avatar = x.Avatar,
                //BirthDay = x.BirthDay.ToString(),
                BirthDay = x.BirthDay,
                Email = x.Email,
                FullName = x.FullName,
                Id = x.Id,
                PhoneNumber = x.PhoneNumber,
                Status = x.Status,
                DateCreated = x.DateCreated

            }).ToList();
            var paginationSet = new PagedResult<AppUserViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }

        public async Task<AppUserViewModel> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            var userVm = _mapper.Map<AppUser, AppUserViewModel>(user);
            userVm.Roles = roles.ToList();
            return userVm;
        }

        public async Task<bool> ChangePassByUserAsync(AppUserViewModel userVm, string password)
        {
            bool flag = false;
            var user = await _userManager.FindByIdAsync(userVm.Id.ToString());
            if (userVm.Password != null && password != null)
            {
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, password, userVm.Password);
                if (changePasswordResult.Succeeded)
                {
                    await _userManager.UpdateAsync(user);
                    flag = true;
                }
            }

            return flag;

        }

        public async Task<bool> UpdateByUserAsync(AppUserViewModel userVm, string password)
        {
            bool flag = false;
            var passwordValidator = new PasswordValidator<AppUser>();
            var model = _mapper.Map<AppUserViewModel, AppUser>(userVm);

            var result = await passwordValidator.ValidateAsync(_userManager, model, password);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByIdAsync(userVm.Id.ToString());
                user.FullName = userVm.FullName;
                user.Email = userVm.Email;
                user.Address = userVm.Address;
                user.BirthDay = userVm.BirthDay;
                user.Gender = userVm.Gender;
                user.PhoneNumber = userVm.PhoneNumber;
                if (userVm.Avatar != null)
                {
                    user.Avatar = userVm.Avatar;
                }
                await _userManager.UpdateAsync(user);
                flag = true;
            }

            return flag;

        }




        public async Task UpdateAsync(AppUserViewModel userVm)
        {
            var user = await _userManager.FindByIdAsync(userVm.Id.ToString());
            //Remove current roles in db
            var currentRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user,
                userVm.Roles.Except(currentRoles).ToArray());

            if (result.Succeeded)
            {
                string[] needRemoveRoles = currentRoles.Except(userVm.Roles).ToArray();
                // await _userManager.RemoveFromRolesAsync(user, needRemoveRoles);
                await RemoveRolesFromUser(user.Id.ToString(), needRemoveRoles);

                //Update user detail
                user.FullName = userVm.FullName;
                user.Status = userVm.Status;
                user.Email = userVm.Email;
                user.PhoneNumber = userVm.PhoneNumber;
                await _userManager.UpdateAsync(user);
            }

        }
        public async Task RemoveRolesFromUser(string userId, string[] roles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roleIds = _roleManager.Roles.Where(x => roles.Contains(x.Name)).Select(x => x.Id).ToList();
            List<IdentityUserRole<Guid>> userRoles = new List<IdentityUserRole<Guid>>();
            foreach (var roleId in roleIds)
            {
                userRoles.Add(new IdentityUserRole<Guid> { RoleId = roleId, UserId = user.Id });
            }
            _context.UserRoles.RemoveRange(userRoles);
            await _context.SaveChangesAsync();

        }

        public async Task<bool> CheckPasswordUser(Guid? id)
        {
            bool flag = false;
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user.PasswordHash != null)
            {
                flag = true;
            }
            return flag;

        }

        public async Task<AppUserViewModel> ChangePassByUserWithPasswordHashIsNull(AppUserViewModel userVm)
        {

            // var model = _mapper.Map<AppUserViewModel, AppUser>(userVm);
            var model = await _userManager.FindByIdAsync(userVm.Id.ToString());
            var user = await _userManager.AddPasswordAsync(model, userVm.Password);
            return userVm;
        }

        public async Task<bool> FindUserByEmailOrUserName(string userOrName)
        {
            var checkmail = TextHelper.EmailIsValid(userOrName);
            var flag = false;
            if (checkmail)
            {
                var user = await _userManager.FindByEmailAsync(userOrName);
                if (user != null)
                {
                    flag = true;
                }
            }
            else
            {
                var user = await _userManager.FindByNameAsync(userOrName);
                if (user != null)
                {
                    flag = true;
                }
            }

            return flag;
        }
    }
}
