using System;
using System.Collections.Generic;
using BaseCms.Common.Attributes;
using BaseCms.Model.Interfaces;
using BackofficeDemo.Model.Metadata;
using BackofficeDemo.MongoBase;

namespace BackofficeDemo.Model
{
    [CmsMetadataType(typeof(ProductMetadata),
        CollectionTitleSingular = "Product",
        CollectionTitlePlural = "Products",
        CustomEditView = "EditWithTabs",
        TabsWithBlocks = "Main;Description;Tech Info")]
    public class Product : Entity, IAlias
    {
        public string Name { get; set; }

        public string Alias { get; set; }

        public string Description { get; set; }

        public int WeightInGrams { get; set; }

        public Guid FoodCategoryGuid { get; set; }

        public Guid PartnerGuid { get; set; }

        public Boolean IsPopular { get; set; }

        public string ImageId { get; set; }

        public decimal Price { get; set; }

        public int Order { get; set; }

        public Boolean Active { get; set; }

        public List<Guid> ProductExtensionGroupGuids { get; set; }
    }
}
