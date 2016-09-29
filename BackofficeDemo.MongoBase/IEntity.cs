using System;
using MongoDB.Bson.Serialization.Attributes;

namespace BackofficeDemo.MongoBase
{
    public interface IEntity<TKey>
    {
        [BsonId]
        TKey Id { get; set; }

        string CreatedBy { get; set; }

        DateTime CreatedOn { get; set; }

        string UpdatedBy { get; set; }

        DateTime UpdatedOn { get; set; }

    }

    public interface IEntity : IEntity<Guid>
    {
    }
}
