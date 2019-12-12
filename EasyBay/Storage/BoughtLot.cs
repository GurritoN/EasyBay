using System;
using System.Collections.Generic;
using System.Text;

namespace Storage
{
    public class BoughtLot
    {
        public int Id { get; set; }
        public User User { get; set; }

        public List<Lot> Lots { get; set; }

        public BoughtLot()
        {
            Lots = new List<Lot>();
        }
    }
}
