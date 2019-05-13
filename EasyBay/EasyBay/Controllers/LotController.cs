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
using Storage;

namespace EasyBay.Controllers
{
    public class LotController : Controller
    {
        private readonly IAuctionFacade facade;

        public LotController(AuctionContext context)
        {
            facade = new AuctionFacade(context);
            facade.CreateNewUser("admin", "sacha0147", "email@mail.ru");
            facade.CreateNewLot("admin", "TestLot", "", 100, 1000, DateTime.MaxValue, null);
        }


        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}