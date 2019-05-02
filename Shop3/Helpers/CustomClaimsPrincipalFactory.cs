using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shop3.Data.Entities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Shop3.Helpers
{
    public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppUser, AppRole>
    {
        /*
         * Claims là mẩu thông tin được lưu gắn với userlogin ngoài các thông tin có sẵn,bản chất claim giống session nó
         * sẽ mã hóa thông tin và lưu trong cookies ,dùng claims có thể nhúng thêm thông tin mà không cần check database mỗi
         * lần login
         Claims được lưu dưới dang key(text) - value
             */
        private UserManager<AppUser> _userManger;

        public CustomClaimsPrincipalFactory(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IOptions<IdentityOptions> options)
                           : base(userManager, roleManager, options)
        {
            _userManger = userManager;
        }

        // ghi đè phương thức ClaimsPrincipal gắn thêm thông tin khi user đăng nhập ,nhớ rigter trong startup
        public async override Task<ClaimsPrincipal> CreateAsync(AppUser user)
        {
            var principal = await base.CreateAsync(user); // lấy ra thông tin của user
            var roles = await _userManger.GetRolesAsync(user); // lấy roles trong user

            ((ClaimsIdentity)principal.Identity).AddClaims(new[]
            {
                // add thông tin cần thêm vào claim
                new Claim(ClaimTypes.NameIdentifier,user.UserName),

                new Claim("Email",user.Email),
                new Claim("FullName",user.FullName),
                new Claim("Avatar",user.Avatar??string.Empty),
                new Claim("Roles",string.Join(";",roles)),
                new Claim("UserId",user.Id.ToString())
            });

            return principal;
        }
    }
}