using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyBay.BusinessLogic;
using EasyBay.DataBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyBay.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActionController : ControllerBase
    {
        private AuctionFacade facade;

        public ActionController(AuctionContext context)
        {
            facade = new AuctionFacade(context);
        }

        [HttpPost("raise")]
        public void RaisePrice(int lotID, decimal newPrice)
        {
            facade.RaisePrice(User.Identity.Name, lotID, newPrice);
        }

        [HttpPost("buyout")]
        public void RaisePrice(int lotID)
        {
            facade.BuyOut(User.Identity.Name, lotID);
        }
    }
}