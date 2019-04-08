using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyBay.Interfaces;
using Storage;
using Microsoft.EntityFrameworkCore;

namespace EasyBay.DataBase
{
    public class DBController : IDBController
    {
        private AuctionContext _context;

        public IQueryable<User> Users => _context.Users.Include(u => u.TrackedLots).Include(u => u.LotsForSale).Include(u => u.BoughtLots);

        public IQueryable<Lot> Lots => _context.Lots.Include(l => l.Owner).Include(l => l.CurrentBuyer).Include(l => l.Tags);

        public IQueryable<Tag> Tags => _context.Tags;

        public DBController(AuctionContext context)
        {
            _context = context;
        }

        public void AddLot(Lot lot)
        {
            _context.Lots.Add(lot);
            _context.SaveChanges();
        }

        public void AddTag(Tag tag)
        {
            _context.Tags.Add(tag);
            _context.SaveChanges();
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void DeleteLot(Lot lot)
        {
            _context.Lots.Remove(lot);
            _context.SaveChanges();
        }

        public void DeleteTag(Tag tag)
        {
            _context.Tags.Remove(tag);
            _context.SaveChanges();
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
