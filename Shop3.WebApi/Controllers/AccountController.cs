using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Shop3.Data.Entities;
using Shop3.Data.Enums;
using Shop3.WebApi.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace Shop3.WebApi.Controllers
{
    // https://www.ecanarys.com/Blogs/ArticleID/308/Token-Based-Authentication-for-Web-APIs
    // http://jasonwatmore.com/post/2018/08/14/aspnet-core-21-jwt-authentication-tutorial-with-example-api
    // https://fullstackmark.com/post/19/jwt-authentication-flow-with-refresh-tokens-in-aspnet-core-web-api
    public class AccountController : ApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger _logger;
        private readonly IConfiguration _config;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILoggerFactory loggerFactory, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _config = config;
        }

        [HttpPost]
        [AllowAnonymous] // cho phép năc danh
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(model); //400
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, true);

                if (!result.Succeeded)
                {
                    return new BadRequestObjectResult(result.ToString());
                }

                var roles = await _userManager.GetRolesAsync(user); // get role
                var claims = new[]
                {
                 /*
                    * Claims là mẩu thông tin được lưu gắn với userlogin ngoài các thông tin có sẵn,bản chất claim giống session nó
                    * sẽ mã hóa thông tin và lưu trong cookies ,dùng claims có thể nhúng thêm thông tin mà không cần check database mỗi
                    * lần login
                   Claims được lưu dưới dang key(text) - value
                 */
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim("fullName", user.FullName),
                    new Claim("avatar", string.IsNullOrEmpty(user.Avatar)? string.Empty:user.Avatar),
                    new Claim("roles", string.Join(";",roles)),
                    new Claim("permissions",""),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }; // list claims

                _logger.LogError(_config["Tokens"]);
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // mã hóa token kèm theo claims
                var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                    _config["Tokens:Issuer"],
                    claims, // đẩy list claims vào token
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: creds);
                _logger.LogInformation(1, "User logged in.");

                return new OkObjectResult(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            return new BadRequestObjectResult("Login failure");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(model);
            }

            var user = new AppUser
            {
                FullName = model.FullName,
                UserName = model.Email,
                Email = model.Email,
                Avatar = string.Empty,
                BirthDay = model.BirthDay,
                PhoneNumber = model.PhoneNumber,
                Status = Status.Active

            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // User claim for write customers data
                await _userManager.AddClaimAsync(user, new Claim("Customers", "Write"));

                await _signInManager.SignInAsync(user, false);
                _logger.LogInformation(3, "User created a new account with password.");
                return new OkObjectResult(model);
            }

            return new BadRequestObjectResult(model);
        }
    }
}