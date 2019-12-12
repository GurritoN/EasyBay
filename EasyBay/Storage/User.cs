using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Storage
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public decimal Balance { get; set; }
        public decimal LockedBalance { get; set; }
        [NotMapped]
        public decimal FreeBalance => Balance - LockedBalance;
        public string Role { get; set; }

        [InverseProperty("Owner")]
        [JsonIgnore]
        public List<Lot> LotsForSale { get; set; }
        [JsonIgnore]
        public List<Lot> TrackedLots { get; set; }
        [JsonIgnore]
        public List<Lot> BoughtLots { get; set; }

        public User()
        {
            LotsForSale = new List<Lot>();
            TrackedLots = new List<Lot>();
            BoughtLots = new List<Lot>();
            Balance = 0;
            LockedBalance = 0;
        }
    }
}
