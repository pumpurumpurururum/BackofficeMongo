using System;
using System.Collections.Generic;
using BackofficeDemo.MongoBase;
using Helper;
using MongoDB.Bson.Serialization.Attributes;

namespace BackofficeDemo.Model
{
    public class ShoppingCartItem: Entity
    {
        public Guid ProductId { get; set; }

        public List<Guid> ProductExtensionIds { get; set; }

        public int Quantity { get; set; }

        public string Instructions { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }


        public Decimal ProductPrice { get; set; }


        public Decimal ExtensionsPrice { get; set; }


        [BsonIgnore]
        public DateTime CreatedOnLocal => CreatedDate.ToSiteLocal();

        [BsonIgnore]
        public DateTime UpdatedOnLocal => UpdatedDate.ToSiteLocal();

        [BsonIgnore]
        public decimal Price => PricePerItem * Quantity;

        [BsonIgnore]
        public decimal PricePerItem => ProductPrice + ExtensionsPrice;
    }
}
