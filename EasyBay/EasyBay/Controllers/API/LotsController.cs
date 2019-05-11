using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyBay.BusinessLogic;
using EasyBay.DataBase;
using EasyBay.Interfaces;
using EasyBay.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Storage;

namespace EasyBay.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class LotsController : ControllerBase
    {
        private IAuctionFacade facade;

        public LotsController(AuctionContext context)
        {
            facade = new AuctionFacade(context);
        }

        [HttpPost]
        public IEnumerable<Lot> All([FromForm]int page = 0, [FromForm]int pagesize = 10)
        {
            return facade.GetActualLots().Skip(pagesize * page).Take(pagesize);
        }

        [HttpGet("{id}")]
        public Lot Get(int id)
        {
            return facade.GetLot(id);
        }

        [Authorize]
        [HttpPut]
        public void Create([FromBody]CreateLotRequest request)
        {
            facade.CreateNewLot(User.Identity.Name, request.Name, request.Description, request.StartingPrice, request.BuyOutPrice, request.TradeFinishTime, request.Tags);
        }

        [Authorize]
        [HttpPatch]
        public void Edit([FromBody]EditLotRequest request)
        {
            if (User.IsInRole(Role.Admin) || facade.GetOwnedLots(User.Identity.Name).Any(l => l.Id == request.Id))
                facade.EditLot(request.Id, request.Name, request.Description, request.BuyOutPrice, request.TradeFinishTime, request.Tags);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public void Delete([FromForm]int id)
        {
            if (User.IsInRole(Role.Admin) || facade.GetOwnedLots(User.Identity.Name).Any(l => l.Id == id))
                facade.DeleteLot(id);
        }
    }
}