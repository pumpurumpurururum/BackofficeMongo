using System;
using Helper;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace BackofficeDemo.MongoBase
{
    [Serializable]
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class Entity : IEntity
    {
        //[BsonRepresentation(BsonType.ObjectId)]
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        public virtual Guid Id { get; set; }


        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        [BsonIgnore]
        public DateTime CreatedOnLocal => CreatedOn.ToSiteLocal();

        [BsonIgnore]
        public DateTime CreatedDateOnLocal => CreatedOn.ToSiteLocal().Date;

        [BsonIgnore]
        public DateTime UpdatedOnLocal => UpdatedOn.ToSiteLocal();

    }
}
