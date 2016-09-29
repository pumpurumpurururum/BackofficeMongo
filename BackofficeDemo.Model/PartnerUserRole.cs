using System;
using BaseCms.Common.Attributes;
using BackofficeDemo.Model.Metadata;
using BackofficeDemo.MongoBase;
using Microsoft.AspNet.Identity;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace BackofficeDemo.Model
{
    [CmsMetadataType(typeof(PartnerUserRoleMetadata),
       CollectionTitleSingular = "Role",
       CollectionTitlePlural = "Roles",
       CustomEditView = "EditWithTabs",
       TabsWithBlocks = "Main;Tech Info")]
    public class PartnerUserRole : Entity, IRole<Guid>, IRole<String>
    {
        public PartnerUserRole()
        {
            
        }

        public PartnerUserRole(string roleName) 
		{
            Name = roleName;
        }

        //[BsonId(IdGenerator = typeof(CombGuidGenerator))]
        //public virtual Guid Id { get; set; }

        [BsonIgnore]
        string IRole<string>.Id => Id.ToString();
        public string Name { get; set; }
    }
}
