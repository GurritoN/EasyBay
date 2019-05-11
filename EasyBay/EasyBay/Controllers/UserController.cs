using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using EasyBay.Interfaces;
using EasyBay.DataBase;
using EasyBay.BusinessLogic;
using EasyBay.ViewModels;

namespace EasyBay.Controllers
{
    public class UserController : Controller
    {
        private readonly IAuctionFacade facade;

        public UserController(AuctionContext context)
        {
            facade = new AuctionFacade(context);
        }

        [HttpGet]
        [Authorize]
        [Route("{username}")]
        public IActionResult Index(string username)
        {
            var user = facade.GetUser(username);
            if (user == null)
                return NotFound();
            return View(user);
        }
        /*
        [HttpPost]
        [Authorize]
        [Route("{username}")]
        //Форма из users/index/username
       
        public IActionResult Index(string username, int newBalance, string role)
        {
            facade.EditUser(username, password, email);
            return View(user);
        }
        */
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (facade.ValidateCredentials(model.Username, model.Password))
                {
                    var user = facade.GetUser(model.Username);
                    Authenticate(user.Username, user.Role);

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (facade.GetUser(model.Username) == null)
                {
                    facade.CreateNewUser(model.Username, model.Password, model.Email);
                    var user = facade.GetUser(model.Username);

                    Authenticate(user.Username, user.Role);

                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Пользователь уже существует");
            }
            return View(model);
        }

        private void Authenticate(string username, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, username),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Lots()
        {
            return View();
        }
    }
}