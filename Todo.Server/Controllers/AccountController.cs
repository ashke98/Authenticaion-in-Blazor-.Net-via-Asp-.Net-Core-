using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Todo.Data;
using Todo.Server.Methods;

namespace Todo.Server.Controllers
{
    public class AccountController : Controller
    {
        private TodotaskContext _ctx;

        public AccountController(TodotaskContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet("/register")]
        public IActionResult Register()
        {
            User user = new User();
            return View(user);
        }

        [HttpPost("/register")]
        [AutoValidateAntiforgeryToken]
        public IActionResult Register(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (user.Id == 0)
                    {
                        _ctx.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                        _ctx.SaveChanges();
                    }

                    return Redirect("/");
                }
                else
                {
                    return View(user);
                }
            }
            catch (Exception e)
            {
                var error = e.Message; // log
                return View(user);
            }
        }

        [HttpGet("/login")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost("/login")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(string username, string password, string rememberme)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = _ctx.Users.FirstOrDefault(m => m.UserName == username && m.Password == password);

                    if(user != null)
                    {
                        await HttpContext.SignInAsync(
                             CookieAuthenticationDefaults.AuthenticationScheme,
                             new ClaimsPrincipal(AuthMethod.get_claimsIdentityAsync(username, user.Id.ToString(), user.RoleName)),
                             AuthMethod.get_authProperties(rememberme == "on"));

                        return Redirect("/");
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    return View();
                }
            }
            catch (Exception e)
            {
                var error = e.Message; // log
                return View();
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