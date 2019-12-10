using System.Linq;
using EasyBay.Interfaces;
using Storage;
using Microsoft.EntityFrameworkCore;

namespace EasyBay.DataBase
{
    public class DBController : IDBController
    {
        private AuctionContext _context;

        public IQueryable<User> Users => _context.Users.Include(
            u => u.TrackedLots).Include(
            u => u.LotsForSale).Include(
            u => u.BoughtLots);

        public IQueryable<Lot> Lots => _context.Lots.Include(
            l => l.Owner).Include(
            l => l.CurrentBuyer).Include(
            l => l.Tags);

        public IQueryable<Tag> Tags => _context.Tags;

        public IQueryable<Transaction> Tansactions => _context.Transactions.Include(
            t => t.Customer).Include(
            t => t.Lot);

        public IQueryable<Log> Logs => _context.Logs.Include(
            l => l.Transaction);

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

        public void AddTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
        }

        public void AddLog(Log log)
        {
            _context.Logs.Add(log);
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
