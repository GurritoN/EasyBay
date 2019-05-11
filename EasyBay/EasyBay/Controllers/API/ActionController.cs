using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyBay.BusinessLogic;
using EasyBay.DataBase;
using EasyBay.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyBay.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActionController : ControllerBase
    {
        private IAuctionFacade facade;

        public ActionController(AuctionContext context)
        {
            facade = new AuctionFacade(context);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public void RaisePrice([FromForm]int lotID, [FromForm]decimal newPrice)
        {
            facade.RaisePrice(User.Identity.Name, lotID, newPrice);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public void BuyOut([FromForm]int lotID)
        {
            facade.BuyOut(User.Identity.Name, lotID);
        }
    }
}