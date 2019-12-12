using System;
using System.Collections.Generic;
using System.Text;

namespace Storage
{
    public class TrackedLot
    {
        public int Id { get; set; }
        public User User { get; set; }

        public List<Lot> Lots { get; set; }

        public TrackedLot()
        {
            Lots = new List<Lot>();
        }
    }
}
