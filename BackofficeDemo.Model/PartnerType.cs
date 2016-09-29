using BaseCms.Common.Attributes;
using BackofficeDemo.Model.Metadata;
using BackofficeDemo.MongoBase;

namespace BackofficeDemo.Model
{
    [CmsMetadataType(typeof(PartnerTypeMetadata),
        CollectionTitleSingular = "Partner Type",
        CollectionTitlePlural = "Partner Types",
        CustomEditView = "EditWithTabs",
        TabsWithBlocks = "Main;Tech Info")]
    public class PartnerType: Entity
    {
        public string Name { get; set; }
    }
}
