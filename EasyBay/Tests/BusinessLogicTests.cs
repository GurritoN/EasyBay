using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyBay.Interfaces;
using Storage;
using EasyBay.BusinessLogic;
using System;

namespace EasyBay.Tests
{
    [TestClass]
    public class BusinessLogicTests
    {
        private IBusinessLogic bl;

        [TestInitialize]
        public void SetupContext()
        {
            bl = new BusinessLogicInternal();
        }

        [TestMethod]
        public void TestDeposit()
        {
            User user = new User();
            bl.Deposit(user, 100);
            Assert.AreEqual(100, user.Balance);
            Assert.ThrowsException<NotEnoughMoneyException>(() => bl.Deposit(user, -1000));
            user.LockedBalance = 100;
            Assert.ThrowsException<NotEnoughMoneyException>(() => bl.Deposit(user, -1));
            user.LockedBalance = 0;
            user.Balance = 0;
        }

        [TestMethod]
        public void TestProcessPurchase()
        {
            User user = new User();
            Lot lot = new Lot();
            lot.CurrentBuyer = user;
            lot.Owner = new User();
            user.Balance = 1000;
            user.LockedBalance = 100;
            lot.CurrentPrice = 100;
            bl.ProcessPurchase(lot);
            Assert.IsTrue(user.BoughtLots.Contains(lot));
            Assert.AreEqual(900, user.Balance);
            Assert.AreEqual(900, user.FreeBalance);
        }

        [TestMethod]
        public void TestBuyOut()
        {
            User user = new User();
            user.Balance = 900;
            Lot lot = new Lot();
            lot.Owner = new User();
            lot.BuyOutPrice = 1000;
            Assert.ThrowsException<NotEnoughMoneyException>(() => bl.BuyOut(user, lot));
            user.Balance += 100;
            bl.BuyOut(user, lot);
            Assert.IsTrue(user.BoughtLots.Contains(lot));
        }

        [TestMethod]
        public void TestRaisePrice()
        {
            User user = new User();
            user.Balance = 900;
            Lot lot = new Lot();
            lot.Owner = new User();
            lot.CurrentPrice = 900;
            lot.BuyOutPrice = 1000;
            Assert.ThrowsException<ArgumentException>(() => bl.RaisePrice(user, lot, 800));
            Assert.ThrowsException<NotEnoughMoneyException>(() => bl.RaisePrice(user, lot, 950));
            user.Balance += 100;
            bl.RaisePrice(user, lot, 950);
            Assert.IsTrue(lot.CurrentBuyer == user);
            Assert.IsTrue(user.FreeBalance == 50);
            Assert.IsTrue(user.Balance == 1000);
            bl.RaisePrice(user, lot, 1000);
            Assert.IsTrue(user.BoughtLots.Contains(lot));
        }
    }
}
