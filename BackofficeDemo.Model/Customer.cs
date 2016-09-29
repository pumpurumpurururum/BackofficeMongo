using System;
using BackofficeDemo.Model.Interfaces;
using BackofficeDemo.MongoBase;
using MongoDB.Bson.Serialization.Attributes;

namespace BackofficeDemo.Model
{
    public class Customer: Entity, ICustomerInfo
    {
        public int SeId { get; set; }

        [BsonRequired]
        public string Email { get; set; }

        [BsonRequired]
        public string Password { get; set; }

        [BsonRequired]
        public string Name { get; set; }

       
        public string PhoneNumber { get; set; }

        public string PhoneExt { get; set; }

        public string Address { get; set; }

        

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
