using System;
using System.Collections.Generic;
using System.Text;

namespace Storage
{
    public class LotTag
    {
        public int Id { get; set; }
        public int LotId { get; set; }
        public Lot Lot { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
