using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyBay.Interfaces;
using Microsoft.EntityFrameworkCore;
using EasyBay.DataBase;
using Storage;
using System.Linq;
using System;
using EasyBay.BusinessLogic;
using System.Collections.Generic;

namespace EasyBay.Tests
{
    [TestClass]
    public class FacadeTests
    {
        private IAuctionFacade facade;

        [TestInitialize]
        public void SetupContext()
        {
            var options = new DbContextOptionsBuilder<AuctionContext>().UseSqlite("Data Source=test.db").Options;
            facade = new AuctionFacade(new AuctionContext(options));
        }

        [TestMethod]
        public void TestDML()
        {
            User user = new User();
            user.Username = "testuser";
            user.Password = "111";
            user.Email = "test@mail.ru";

            facade.CreateNewUser("testuser", "111", "test@mail.ru");
            facade.CreateNewLot("testuser", "lot", null, 100, 100,
                DateTime.Now + TimeSpan.FromDays(7), new List<string>());

            Assert.IsTrue(facade.GetUser("testuser").Password == user.Password);

            facade.EditUser("testuser", "1110", "test@mail.ru");
            Assert.IsTrue(facade.GetUser("testuser").Password == "1110");

            IEnumerable<Lot> lots = facade.GetOwnedLots("testuser");
            bool is_find = false;
            int lotID = 0;
            foreach (var lot in lots) {
                if (lot.Name == "lot")
                    is_find = true;
                lotID = lot.Id;
            }
            Assert.IsTrue(is_find);
        }

        [TestMethod]
        public void TestBL()
        {
            IEnumerable<Lot> lots = facade.GetOwnedLots("testuser");
            int lotID = 0;
            foreach (var lot in lots)
            {
                if (lot.Name == "lot")
                    lotID = lot.Id;
            }
            facade.CreateNewUser("buyer", "1111", "mail");
            var user = facade.GetUser("buyer");
            facade.Deposit("buyer", 10000000);
            facade.BuyOut("buyer", lotID);
            Assert.IsTrue(user.Balance + 100 == 10000000);

            lots = facade.GetOwnedLots("testuser");
            bool is_active = false;
            foreach (var lot in lots)
            {
                if (lot.Name == "lot" && lot.IsActive)
                    is_active = true;
            }
            Assert.IsFalse(is_active);
        }
    }
}
