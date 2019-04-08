using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyBay.Interfaces;
using EasyBay.DataBase;
using Storage;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace EasyBay.Tests
{
    [TestClass]
    public class DBTests
    {
        private IDBController db;

        [TestInitialize]
        public void SetupContext()
        {
            var options = new DbContextOptionsBuilder<AuctionContext>().UseSqlite("Data Source=test.db").Options;
            db = new DBController(new AuctionContext(options));
        }

        [TestMethod]
        public void AddUserTest()
        {
            // Arrange
            User user = new User();
            user.Username = "test";
            user.Password = "111";
            user.Email = "a@a.ru";

            // Act
            db.AddUser(user);

            // Assert
            Assert.IsTrue(db.Users.Any(u => u.Username == "test"));
            User testUser = db.Users.Where(u => u.Username == "test").ToList()[0];
            Assert.IsTrue(testUser.Password == user.Password && testUser.Email == user.Email);
            db.DeleteUser(user);

        }

        public void DeleteUserTest()
        {
            // Arrange
            User user = new User();
            user.Username = "test1";
            user.Password = "111";
            user.Email = "a@a.ru";

            // Act
            db.DeleteUser(user);

            // Assert
            Assert.IsFalse(db.Users.Any(u => u.Username == "test1"));
        }

        public void AddLotTest()
        {
            // Arrange
            Lot lot = new Lot();
            lot.Name = "lot";
            lot.Description = "some kitty";
            lot.TradeFinishTime = DateTime.UtcNow + TimeSpan.FromDays(1);
            lot.CurrentPrice = 0;

            // Act
            db.AddLot(lot);

            // Assert
            Assert.IsTrue(db.Lots.Any(u => u.Name == "lot"));
            Lot testLot = db.Lots.Where(u => u.Name == "lot").ToList()[0];
            Assert.IsTrue(testLot.Description == lot.Description
                && testLot.TradeFinishTime == lot.TradeFinishTime
                && testLot.CurrentPrice == lot.CurrentPrice);
            db.DeleteLot(lot);
        }

        public void DeleteLotTest()
        {
            // Arrange
            Lot lot = new Lot();
            lot.Name = "lot1";
            lot.Description = "some kitty";
            lot.TradeFinishTime = DateTime.UtcNow + TimeSpan.FromDays(1);
            lot.CurrentPrice = 0;

            // Act
            db.DeleteLot(lot);

            // Assert
            Assert.IsFalse(db.Lots.Any(u => u.Name == "lot1"));
        }

        public void AddTagTest()
        {
            // Arrange
            Tag tag = new Tag();
            tag.Name = "puppy";

            // Act
            db.AddTag(tag);

            // Assert
            Assert.IsTrue(db.Tags.Any(u => u.Name == "puppy"));
        }

        public void SaveChangesTest()
        {
            // Arrange
            Tag tag = new Tag();
            tag.Name = "kitty";
            db.AddTag(tag);
            tag.Name = "sonya";

            // Act
            db.SaveChanges();

            // Assert
            Assert.IsTrue(db.Tags.Any(u => u.Name == "sonya"));
        }
    }
}
