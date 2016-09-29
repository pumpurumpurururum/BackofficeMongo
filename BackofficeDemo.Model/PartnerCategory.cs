using BaseCms.Common.Attributes;
using BaseCms.Model.Interfaces;
using BackofficeDemo.Model.Enums;
using BackofficeDemo.Model.Metadata;
using BackofficeDemo.MongoBase;

namespace BackofficeDemo.Model
{
    [CmsMetadataType(typeof(PartnerCategoryMetadata),
        CollectionTitleSingular = "Partner category",
        CollectionTitlePlural = "Partner Categories",
        CustomEditView = "EditWithTabs",
        TabsWithBlocks = "Main;Image;Tech Info")]
    public class PartnerCategory : Entity, IAlias
    {
        public string Alias { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }

        public string ImageId { get; set; }

        
    }
}
