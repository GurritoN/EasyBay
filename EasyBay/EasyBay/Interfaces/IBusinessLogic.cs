using Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyBay.Interfaces
{
    public interface IBusinessLogic
    {
        void RaisePrice(User user, Lot lot, decimal newPrice);

        void BuyOut(User user, Lot lot);

        void ProcessPurchase(Lot lot);

        void Deposit(User user, decimal amount);
    }
}
