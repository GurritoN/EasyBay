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
using System.IO;

namespace EasyBay.Controllers
{
    public class LotController : Controller
    {
        private readonly IAuctionFacade facade;
        public LotController(AuctionContext context)
        {
            facade = new AuctionFacade(context);
        }


        [HttpGet("Lot/{username}")]
        [Authorize]
        public IActionResult Index(string username)
        {
            ViewBag.username = username;
            ViewBag.facade = facade;
            return View(facade.GetActualLots().ToList());
        }

        [HttpGet("Lot/User/{username}")]
        [Authorize]
        public IActionResult User(string username)
        {
            ViewBag.username = username;
            return View(facade.GetBoughtLots(username).ToList());
        }

        [HttpGet("Lot/Track/{username}")]
        [Authorize]
        public IActionResult Track(string username)
        {
            ViewBag.username = username;
            return View(facade.GetTrackedLots(username).ToList());
        }

        [HttpGet("Lot/Details/{Id}/{username}")]
        [Authorize]
        public IActionResult Details(string username, int Id)
        {
            ViewBag.facade = facade;
            ViewBag.username = username;
            Lot lot = facade.GetLot(Id);
            return View(lot);
        }


        [HttpGet("Lot/Raise/{Id}/{username}")]
        [Authorize]
        public IActionResult Raise(string username, int Id)
        {
            return View(facade.GetLot(Id));
        }

        [HttpPost("Lot/Raise/{Id}/{username}")]
        [Authorize]
        public IActionResult Raise(int Id, string username, RaisePriceModel model)
        {
            if (ModelState.IsValid)
            {
                var user = facade.GetUser(username);
                try
                {
                    ViewBag.errors = null;
                    facade.RaisePrice(username, Id, decimal.Parse(model.Price));
                }
                catch
                {
                    ViewBag.errors = "NotEnoughMoneyException";
                    return View(facade.GetLot(Id));
                }

                return RedirectToAction("Index", "Home");
            }
            return View(facade.GetLot(Id));
        }


        [Authorize]
        public IActionResult BuyOut(int Id, string username)
        {
            var user = facade.GetUser(username);
            try
            {
                ViewBag.errors = null;
                facade.BuyOut(username, Id);
            }
            catch
            {
                ViewBag.errors = "NotEnoughMoneyException";
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");

        }
    }
}