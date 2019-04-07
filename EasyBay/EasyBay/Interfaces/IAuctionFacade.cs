using Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyBay.Interfaces
{
    public interface IAuctionFacade
    {
        void CreateNewUser(string username, string password, string email);
        void EditUser(string password, string email);
        bool ValidateCredentials(string username, string password);
        User GetUser(string username);
        void Deposit(string username, int amount);
        void DeleteUser(string username);

        void CreateNewLot(string username, string name, string description, int startingPrice, int buyOutPrice, DateTime tradeFinishTime, List<Tag> tags);
        void EditLot(string name, string description, int? buyOutPrice, DateTime? tradeFinishTime, List<Tag> tags);
        Lot GetLot(int lotID);
        Lot GetRandomLot();
        IEnumerable<Lot> GetElapsedLots();
        IEnumerable<Lot> GetOwnedLots(int userID);
        void DeleteLot(int lotID);

        void RaisePrice(string username, int lotID, int newPrice);
        void BuyOut(string username, int lotID);
        void ProcessPurchase(int lotID);

        void ClearUnusedTags(string name);
    }
}
