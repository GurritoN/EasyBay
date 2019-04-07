using Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyBay.Interfaces
{
    interface IDBController
    {
        void AddUser(User user);
        void DeleteUser(User user);
        IQueryable<User> Users { get; }

        void AddLot(Lot lot);
        void DeleteLot(Lot lot);
        IQueryable<Lot> Lots { get; }

        void AddTag(Tag tag);
        void DeleteTag(Tag tag);
        IQueryable<Tag> Tags { get; }

        void SaveChanges();
    }
}
