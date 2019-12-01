using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Todo.Server.Controllers
{
    public class AccountController : Controller
    {
        private UnderlineContext _ctx;

        public AccountController(UnderlineContext ctx)
        {
            _ctx = ctx;
        }

        public IActionResult Register()
        {
            RegisterModel registerModel = new RegisterModel();

            Role role = roleManager.Get(EnumRole.user);

            registerModel = new RegisterModel
            {
                ConfirmPassword = "111111",
                Password = "111111",
                UserName = "a@a.com"
            };

            EnumAccount Result = accountManager.Register(registerModel, role);

            return View(registerModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Register(RegisterModel registerModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Role role = roleManager.Get(EnumRole.user);

                    EnumAccount Result = accountManager.Register(registerModel, role);

                    TempData["sucess"] = Result.GetDisplayName();

                    return Redirect("/");
                }
                else
                {
                    TempData["error"] = EnumErrors.notvalidinputs.GetDisplayName();

                    return View(registerModel);
                }
            }
            catch (Exception e)
            {
                var error = e.Message; // log
                TempData["error"] = EnumErrors.error.GetDisplayName();
                return View(registerModel);
            }
        }

        [HttpGet("/login")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost("/login")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AuthModel Result = accountManager.Login(loginModel);

                    if (Result.EnumAccount == EnumAccount.success)
                    {
                        await HttpContext.SignInAsync(
                                CookieAuthenticationDefaults.AuthenticationScheme,
                                new ClaimsPrincipal(AuthenticationHelper.get_claimsIdentityAsync(loginModel.UserName, Result.UserID.ToString(), (int)EnumRole.admin)),
                                AuthenticationHelper.get_authProperties(loginModel.RememberMe));

                        TempData["success"] = Result.EnumAccount.GetDisplayName();

                        return Redirect("/");
                    }

                    TempData["error"] = Result.EnumAccount.GetDisplayName();

                    return View(loginModel);
                }
                else
                {
                    TempData["error"] = EnumErrors.notvalidinputs.GetDisplayName();

                    return View(loginModel);
                }
            }
            catch (Exception e)
            {
                var error = e.Message; // log
                TempData["error"] = EnumErrors.error.GetDisplayName();
                return View(loginModel);
            }
        }

        [HttpGet("/logout")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/login");
        }
    }
}