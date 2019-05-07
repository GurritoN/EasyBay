using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyBay.BusinessLogic;
using EasyBay.DataBase;
using EasyBay.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Storage;

namespace EasyBay.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class LotsController : ControllerBase
    {
        private AuctionFacade facade;

        public LotsController(AuctionContext context)
        {
            facade = new AuctionFacade(context);
        }

        [HttpPost("all")]
        public IEnumerable<Lot> Lots(int page = 0, int pagesize = 10)
        {
            return facade.GetActualLots().Skip(pagesize * page).Take(pagesize);
        }

        [HttpGet("get/{id}")]
        public Lot Lot(int id)
        {
            return facade.GetLot(id);
        }

        [HttpPut("create")]
        public void Create(CreateLotRequest request)
        {
            //auth logic
            facade.CreateNewLot("user", request.Name, request.Description, request.StartingPrice, request.BuyOutPrice, request.TradeFinishTime, request.Tags);
        }

        [HttpPatch("edit")]
        public void Edit(EditLotRequest request)
        {
            //auth logic
            facade.EditLot(request.Id, request.Name, request.Description, request.BuyOutPrice, request.TradeFinishTime, request.Tags);
        }

        [HttpDelete("delete/{id}")]
        public void Delete(int id)
        {
            facade.DeleteLot(id);
        }
    }
}