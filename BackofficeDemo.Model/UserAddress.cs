using System;
using BackofficeDemo.MongoBase;

namespace BackofficeDemo.Model
{
    public class UserAddress: Entity
    {
        public DateTime LastUsageDateTime { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}
