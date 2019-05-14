using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBay.Terminal.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public decimal Balance { get; set; }
        public decimal LockedBalance { get; set; }
        public decimal FreeBalance => Balance - LockedBalance;
        public string Role { get; set; }

        public List<int> LotsForSale { get; set; }
        public List<int> TrackedLots { get; set; }
        public List<int> BoughtLots { get; set; }
    }
}
