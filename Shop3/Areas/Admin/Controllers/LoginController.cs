using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shop3.Application.Interfaces;
using Shop3.Data.Entities;
using Shop3.Extensions;
using Shop3.Models.AccountViewModels;
using Shop3.Utilities.Constants;
using Shop3.Utilities.Dtos;
using System.Threading.Tasks;

namespace Shop3.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Route("admin")]
    public class LoginController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;
        private readonly ILogger _logger;


        public LoginController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager
            , ILogger<LoginController> logger, IRoleService roleService, IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _roleService = roleService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated == true)
            {
                string id = User.GetSpecificClaim(CommonConstants.UserClaims.UserId);
                string email = User.GetSpecificClaim(CommonConstants.UserClaims.Email);

                var check = await _roleService.CheckRoleByUser(id);
                var checkPermistion = await _roleService.CheckAccount(email);
                if (check == true && checkPermistion == true)
                {
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    return RedirectToAction(nameof(Forbidden));
                }

            }
            return View();



        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Authen(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                var checkPermistion = await _roleService.CheckAccount(model.Email);
                var checkUser = await _userService.FindUserByEmailOrUserName(model.Email);

                if (checkUser == false)
                {
                    return new ObjectResult(new GenericResult(false, "User does not exist"));
                }
                if (checkPermistion == false)
                {
                    _logger.LogWarning("User account locked out.");
                    return new ObjectResult(new GenericResult(false, "User account locked out"));
                }
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return new OkObjectResult(new GenericResult(true));

                }
                else
                {

                    _logger.LogWarning("User login in fail.");
                    return new ObjectResult(new GenericResult(false, "User Name or password Invalid"));
                }

            }
            return new ObjectResult(new GenericResult(false, model));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Forbidden()
        {
            return View();
        }




    }
}