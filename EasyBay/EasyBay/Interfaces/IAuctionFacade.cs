using Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EasyBay.Interfaces
{
    public interface IAuctionFacade
    {
        void AddImageToLot(int lotId, Stream image);
        Stream GetLotImage(int lotId);

        void CreateNewUser(string username, string password, string email);
        void EditUser(string username, string password, string email);
        bool ValidateCredentials(string username, string password);
        User GetUser(string username);
        void Deposit(string username, decimal amount);
        void DeleteUser(string username);

        Lot CreateNewLot(string username, string name, string description, decimal startingPrice, decimal buyOutPrice, DateTime tradeFinishTime, List<string> tags);
        void EditLot(int lotID, string name, string description, decimal? buyOutPrice, DateTime? tradeFinishTime, List<string> tags);
        Lot GetLot(int lotID);
        Lot GetRandomLot();
        IEnumerable<Lot> GetElapsedLots();
        IEnumerable<Lot> GetActualLots();
        IEnumerable<Lot> GetOwnedLots(string username);
        void DeleteLot(int lotID);

        void RaisePrice(string username, int lotID, decimal newPrice);
        void BuyOut(string username, int lotID);
        void ProcessPurchase(int lotID);

        void ClearUnusedTags(string name);
        IEnumerable<Tag> GetTagsByPrefix(string prefix);
    }
}
