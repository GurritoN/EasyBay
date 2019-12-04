using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Storage
{
    public class Lot
    {
        public int Id { get; set; }
        public DateTime TradeFinishTime { get; set; }
        public decimal BuyOutPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhotoPath { get; set; }

        public int? CurrentBuyerId { get; set; }
        public virtual User CurrentBuyer { get; set; }

        public int OwnerId { get; set; }
        [InverseProperty("LotsForSale")]
        [JsonIgnore]
        public virtual User Owner { get; set; }
        public List<Tag> Tags { get; set; }
        public Lot()
        {
            Tags = new List<Tag>();
        }
    }
}
