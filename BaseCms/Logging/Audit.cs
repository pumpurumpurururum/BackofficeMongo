using System;
using BaseCms.Common.Attributes;
using BaseCms.Logging.Metadata;
using BackofficeDemo.MongoBase;

namespace BaseCms.Logging
{
    [CmsMetadataType(typeof(AuditMetadata))]
    public class Audit : Entity
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        public string CollectionName { get; set; }
        public string ObjectId { get; set; }
        public string CommandData { get; set; }
        public string ChangeType { get; set; }
    }
}
