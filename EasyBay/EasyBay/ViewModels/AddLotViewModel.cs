using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyBay.ViewModels
{
    public class AddLotViewModel
    {
        public DateTime TradeFinishTime { get; set; }
        public decimal BuyOutPrice { get; set; }
        public decimal StartingPrice { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
