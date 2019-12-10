using System;

namespace Storage
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string Exception { get; set; }
        public string Comment { get; set; }
        public Transaction Transaction { get; set; }
    }
}
