using System;
using BaseCms.Common.Attributes;
using BaseCms.Model.Interfaces;
using BackofficeDemo.Model.Enums;
using BackofficeDemo.Model.Metadata;
using BackofficeDemo.MongoBase;

namespace BackofficeDemo.Model
{
    [CmsMetadataType(typeof(FoodCategoryMetadata),
        CollectionTitleSingular = "Food category",
        CollectionTitlePlural = "Food category",
        CustomEditView = "EditWithTabs",
        TabsWithBlocks = "Main;Tech Info")]
    public class FoodCategory : Entity, IAlias
    {
        public string Alias { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }

        public Boolean Active { get; set; }
        public Guid PartnerGuid { get; set; }
        public CategoryType Type { get; set; }

        public Guid? ParentCategoryGuid { get; set; }
    }
}