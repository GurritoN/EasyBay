using EasyBay.Interfaces;
using Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyBay.BusinessLogic
{
    public class BusinessLogicInternal : IBusinessLogic
    {
        public void RaisePrice(User user, Lot lot, int newPrice)
        {
            if (lot.CurrentPrice >= newPrice)
                throw new ArgumentException("Price must be higher than current");
            if (lot.BuyOutPrice <= newPrice)
            {
                BuyOut(user, lot);
                return;
            }

            if (lot.CurrentBuyer == user)
            {
                var priceDiff = newPrice - lot.CurrentPrice;
                if (user.FreeBalance < priceDiff)
                    throw new NotEnoughMoneyException();
                user.LockedBalance += priceDiff;
            }
            else
            { 
                if (user.FreeBalance < newPrice)
                    throw new NotEnoughMoneyException();
                if (lot.CurrentBuyer != null)
                    lot.CurrentBuyer.LockedBalance -= lot.CurrentPrice;
                user.LockedBalance += newPrice;
                user.TrackedLots.Add(lot);
            }
            lot.CurrentPrice = newPrice;
            lot.CurrentBuyer = user;
        }

        public void BuyOut(User user, Lot lot)
        {
            if (lot.CurrentBuyer == user)
            {
                var priceDiff = lot.BuyOutPrice - lot.CurrentPrice;
                if (user.FreeBalance < priceDiff)
                    throw new NotEnoughMoneyException();
                user.LockedBalance += priceDiff;
            }
            else
            {
                if (user.FreeBalance < lot.BuyOutPrice)
                    throw new NotEnoughMoneyException();
                if (lot.CurrentBuyer != null)
                    lot.CurrentBuyer.LockedBalance -= lot.CurrentPrice;
                user.TrackedLots.Add(lot);
                user.LockedBalance += lot.BuyOutPrice;
            }
            lot.CurrentPrice = lot.BuyOutPrice;
            lot.CurrentBuyer = user;
            ProcessPurchase(lot);
        }

        public void ProcessPurchase(Lot lot)
        {
            User user = lot.CurrentBuyer;
            if (user == null)
            {
                lot.IsActive = false;
                return;
            }
            lot.Owner.Balance += lot.CurrentPrice;
            user.LockedBalance -= lot.CurrentPrice;
            user.Balance -= lot.CurrentPrice;
            user.BoughtLots.Add(lot);
            user.TrackedLots.Remove(lot);
            lot.IsActive = false;
        }

        public void Deposit(User user, int amount)
        {
            if (user.FreeBalance + amount < 0)
                throw new NotEnoughMoneyException();
            user.Balance += amount;
        }
    }
}
