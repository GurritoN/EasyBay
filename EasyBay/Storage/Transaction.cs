using System;
using System.Collections.Generic;

namespace Storage
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public Lot Lot { get; set; }
        public User Customer { get; set; }
        public decimal FinalPrice { get; set; }
    }
}
