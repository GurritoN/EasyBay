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
        public List<Lot> LotsForSale { get; set; }
        public List<Lot> TrackedLots { get; set; }
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

    public static class Role
    {
        public static string Admin = "Admin";
        public static string User = "User";
    }
}
