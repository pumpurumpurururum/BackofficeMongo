using System;
using BaseCms.Common.Attributes;
using BackofficeDemo.Model.Metadata;
using BackofficeDemo.MongoBase;
using MongoDB.Bson.Serialization.Attributes;

namespace BackofficeDemo.Model
{
    [CmsMetadataType(typeof(CityMetadata),
    CollectionTitleSingular = "City",
    CollectionTitlePlural = "Cities",
    CustomEditView = "EditWithTabs",
    TabsWithBlocks = "Main;Tech Info")]
    public class City : Entity
    {
        public string StateKey { get; set; }

        [BsonIgnore]
        public string Name => $"{LocationName}, {StateKey}, {Country}";

        public string LocationName { get; set; }

        public string Country { get; set; }
    }
}
