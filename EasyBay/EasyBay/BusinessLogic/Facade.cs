using EasyBay.DataBase;
using EasyBay.Interfaces;
using Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EasyBay.BusinessLogic
{
    public class AuctionFacade : IAuctionFacade
    {
        private DBController db;
        private BusinessLogicInternal logic;
        private Random rand = new Random();

        public AuctionFacade(AuctionContext dbContext)
        {
            db = new DBController(dbContext);
            logic = new BusinessLogicInternal();
        }

        public void BuyOut(string username, int lotID)
        {
            User user = GetUser(username);
            Lot lot = GetLot(lotID);

            logic.BuyOut(user, lot);

            db.SaveChanges();
        }

        public void ClearUnusedTags(string name)
        {
            foreach (var tag in db.Tags)
                if (!db.Lots.Any(l => l.Tags.Contains(tag)))
                    db.DeleteTag(tag);
            db.SaveChanges();
        }

        public Lot CreateNewLot(string username, string name, string description, decimal startingPrice, decimal buyOutPrice, DateTime tradeFinishTime, List<string> tags)
        {
            if (startingPrice < 0 || startingPrice <= 0)
                throw new ArgumentException("Prices can't be negative");
            User user = GetUser(username);
            Lot lot = new Lot();
            lot.Owner = user;
            lot.Name = name;
            lot.Description = description;
            lot.CurrentPrice = startingPrice;
            lot.BuyOutPrice = buyOutPrice;
            lot.TradeFinishTime = tradeFinishTime;
            List<Tag> taglist = new List<Tag>();
            foreach (var tagName in tags)
            {
                var tag = db.Tags.FirstOrDefault(t => t.Name == tagName);
                if (tag != null)
                    taglist.Add(tag);
                else
                {
                    tag = new Tag();
                    tag.Name = tagName;
                    db.AddTag(tag);
                    taglist.Add(tag);
                }
            }
            lot.Tags = taglist;
            db.AddLot(lot);
            return lot;
        }

        public void CreateNewUser(string username, string password, string email)
        {
            User user = new User();
            user.Username = username;
            user.Password = password;
            user.Email = email;
            if (user.Username == "admin")
                user.Role = Role.Admin;
            else
                user.Role = Role.User;
            db.AddUser(user);
        }

        public void DeleteLot(int lotID)
        {
            Lot lot = GetLot(lotID);
            if (lot.CurrentBuyer != null)
            {
                lot.CurrentBuyer.LockedBalance -= lot.CurrentPrice;
                db.SaveChanges();
            }
            db.DeleteLot(GetLot(lotID));
        }

        public void DeleteUser(string username)
        {
            User user = GetUser(username);
            foreach (var lot in user.LotsForSale)
                DeleteLot(lot.Id);
            db.DeleteUser(user);
        }

        public void Deposit(string username, decimal amount)
        {
            User user = GetUser(username);
            logic.Deposit(user, amount);
            db.SaveChanges();
        }

        public void EditLot(int lotID, string name, string description, decimal? buyOutPrice, DateTime? tradeFinishTime, List<string> tags)
        {
            Lot lot = GetLot(lotID);
            if (name != null)
                lot.Name = name;
            if (description != null)
                lot.Description = description;
            if (buyOutPrice.HasValue)
            {
                if (buyOutPrice.Value <= lot.CurrentPrice)
                    throw new ArgumentException("New buyout price must be more than current price");
                lot.BuyOutPrice = buyOutPrice.Value;
            }
            if (tradeFinishTime.HasValue)
                lot.TradeFinishTime = tradeFinishTime.Value;
            List<Tag> taglist = new List<Tag>();
            foreach (var tagName in tags)
            {
                var tag = db.Tags.FirstOrDefault(t => t.Name == tagName);
                if (tag != null)
                    taglist.Add(tag);
                else
                {
                    tag = new Tag();
                    tag.Name = tagName;
                    db.AddTag(tag);
                    taglist.Add(tag);
                }
            }
            lot.Tags = taglist;

            db.SaveChanges();
        }

        public void EditUser(string username, string password, string email)
        {
            User user = GetUser(username);
            if (password != null)
                user.Password = password;
            if (email != null)
                user.Email = email;

            db.SaveChanges();
        }

        public IEnumerable<Lot> GetElapsedLots()
        {
            return db.Lots.Where(l => l.TradeFinishTime < DateTime.Now);
        }

        public IEnumerable<Lot> GetActualLots()
        {
            return db.Lots.Where(l => l.IsActive);
        }

        public Lot GetLot(int lotID)
        {
            return db.Lots.FirstOrDefault(l => l.Id == lotID);
        }

        public IEnumerable<Lot> GetOwnedLots(string username)
        {
            return GetUser(username).LotsForSale;
        }

        public Lot GetRandomLot()
        {
            return db.Lots.ElementAt(rand.Next(db.Lots.Count()));
        }

        public IEnumerable<Tag> GetTagsByPrefix(string prefix)
        {
            return db.Tags.Where(t => t.Name.StartsWith(prefix));
        }

        public User GetUser(string username)
        {
            return db.Users.FirstOrDefault(u => u.Username == username);
        }

        public void ProcessPurchase(int lotID)
        {
            Lot lot = GetLot(lotID);

            logic.ProcessPurchase(lot);

            db.SaveChanges();
        }

        public void RaisePrice(string username, int lotID, decimal newPrice)
        {
            User user = GetUser(username);
            Lot lot = GetLot(lotID);

            logic.RaisePrice(user, lot, newPrice);

            db.SaveChanges();
        }

        public bool ValidateCredentials(string username, string password)
        {
            return db.Users.Any(u => u.Username == username && u.Password == password);
        }

        public void AddImageToLot(int lotId, Stream image)
        {
            string path = $"images/lotimages/{lotId}.jpg";
            using (FileStream fs = File.Exists(path) ? File.OpenWrite(path) : File.Create(path))
            {
                image.CopyTo(fs);
            }
        }

        public Stream GetLotImage(int lotId)
        {
            string path = $"images/lotimages/{lotId}.jpg";
            return File.Exists(path) ? File.OpenWrite(path) : null;
        }
    }
}