using System;
using BackofficeDemo.MongoBase;
using Helper;
using MongoDB.Bson.Serialization.Attributes;

namespace BackofficeDemo.Model
{
    public class ProductExtension : Entity
    {
        public Guid ProductExtensionGroupId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Weight { get; set; }

        //public bool Selected { get; set; }

        public bool Published { get; set; }

        public int Order { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        [BsonIgnore]
        public DateTime CreatedOnLocal => CreateDate.ToSiteLocal();

        [BsonIgnore]
        public DateTime UpdatedOnLocal => UpdateDate.ToSiteLocal();
    }
}
