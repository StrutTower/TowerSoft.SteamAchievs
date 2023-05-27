using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TowerSoft.SteamAchievs.Website.Infrastructure;
using TowerSoft.SteamAchievs.Website.ViewModels;

namespace TowerSoft.SteamAchievs.Website.Controllers {
    public class AccountController : CustomController {
        //private readonly UnitOfWork uow;

        public AccountController(/*UnitOfWork uow*/) {
            //this.uow = uow;
        }


        [HttpGet]
        public IActionResult Login() {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> Login(LoginViewModel model) {
        //    throw new NotImplementedException();
        //if (ModelState.IsValid) {
        //User user;
        //user = uow.GetRepo<UserRepository>().GetByUsername(model.UserName);

        //if (user != null) {
        //    if (new Hasher().Validate(user.Password, model.Password, user.Salt)) {
        //        List<Claim> claims = new List<Claim> {
        //            new Claim(ClaimTypes.Name, user.Username),
        //            new Claim(ClaimTypes.Email, user.EmailAddress),
        //            new Claim(ClaimTypes.NameIdentifier, user.ID.ToString())
        //        };

        //        if (user.Username.Equals("StrutTower", StringComparison.InvariantCultureIgnoreCase)) {
        //            claims.Add(new Claim(ClaimTypes.Role, RoleTypes.Admin));
        //        }

        //        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        //        AuthenticationProperties authProps = new AuthenticationProperties {
        //            IsPersistent = true,
        //            AllowRefresh = true
        //        };

        //        await HttpContext.SignInAsync(
        //            CookieAuthenticationDefaults.AuthenticationScheme,
        //            new ClaimsPrincipal(identity),
        //            authProps);
        //        return RedirectToAction("Index", "Home");
        //    }
        //}
        //ModelState.AddModelError("Password", "The username or password are incorrect.");
        //}
        //return View(model);
        //}

        [Authorize]
        public async Task<IActionResult> Logout() {
            await HttpContext.SignOutAsync();
            TempData["message"] = "You have been logged out";
            return RedirectToAction("Index", "Home");
        }
    }
}
