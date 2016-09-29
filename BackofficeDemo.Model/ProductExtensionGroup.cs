using System;
using System.Collections.Generic;
using BackofficeDemo.Model.Enums;
using BackofficeDemo.MongoBase;
using Helper;
using MongoDB.Bson.Serialization.Attributes;

namespace BackofficeDemo.Model
{
    public class ProductExtensionGroup : Entity
    {
        public Guid PartnerGuid { get; set; }


        public String Title { get; set; }


        public string Name { get; set; }

        public string Description { get; set; }

        //public bool Required { get; set; }

        public GroupSelectionType SelectionType { get; set; }

        public bool Published { get; set; }

        public int Order { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        //[BsonIgnore()]
        //public List<ProductExtension> Extensions { get; set; }

        [BsonIgnore]
        public DateTime CreatedOnLocal => CreateDate.ToSiteLocal();

        [BsonIgnore]
        public DateTime UpdatedOnLocal => UpdateDate.ToSiteLocal();
    }
}
