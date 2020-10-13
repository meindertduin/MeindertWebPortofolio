using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var loginForm = new LoginModel()
            {
                ReturnUrl = returnUrl,
            };

            return View(loginForm);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {


            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(loginModel.UserName);

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, false);

                    if (result.Succeeded)
                    {
                        if (string.IsNullOrEmpty(loginModel.ReturnUrl))
                        {
                            return Redirect("/");
                        }

                        return Redirect(loginModel.ReturnUrl);
                    }
                }
            }
            ModelState.AddModelError("", "invalid login");

            return View(loginModel);
        }
    }
}