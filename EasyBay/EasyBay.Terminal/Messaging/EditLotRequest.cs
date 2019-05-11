﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyBay.Messaging
{
    public class EditLotRequest
    {
        public DateTime? TradeFinishTime { get; set; }
        public decimal? BuyOutPrice { get; set; }
        public int Id { get; set; }
        public bool? IsActive { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Tags { get; set; }
    }
}
