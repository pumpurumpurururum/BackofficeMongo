using System;
using BaseCms.Common.Image.Attributes;
using BaseCms.Common.Validation.Attributes;
using BaseCms.Views.Detail.Attributes;
using BaseCms.Views.Link.Attributes;
using BaseCms.Views.List.Attributes;

namespace BackofficeDemo.Model.Metadata
{
    public class ProductMetadata
    {
        [ListMetadata(Display = "Id", IsHidden = true, Order = 0)]
        [DetailMetadata(IsEditable = false, IsKey = true, Tab = 3, IsHidden = false)]
        public Guid Id { get; set; }

        [ListMetadata(Display = "Name", IsHidden = false, Order = 1)]
        [DetailMetadata(Display = "Name", Order = 1, IsHeaderDisplayProperty = true, Tab = 1)]
        [Required]
        public string Name { get; set; }

        [DetailMetadata(MaxLength = 50, IsEditable = false, Tab = 1, Order = 2)]
        public string Alias { get; set; }


        [ListMetadata(Display = "Price", IsHidden = false, Order = 3)]
        [DetailMetadata(Display = "Price", Order = 2, Tab = 1)]
        [Required]
        public decimal Price { get; set; }

        [ListMetadata(Display = "Order", IsHidden = true, Order = 4)]
        [DetailMetadata(Display = "Order", Order = 3, Tab = 1)]
        public int Order { get; set; }

        [ListMetadata(Display = "Popular", IsHidden = false, Order = 4)]
        [DetailMetadata(Display = "Popular", Order = 3, Tab = 1)]
        public Boolean IsPopular { get; set; }


        [ListMetadata(Order = 6)]
        [DetailMetadata(Order = 5, Tab = 1)]
        public Boolean Active { get; set; }


        [DetailMetadata(Display = "Description", Template = "Html", Order = 1, Tab = 2)]
        public string Description { get; set; }

        [ListMetadata(Display = "Weight in grams", IsHidden = false, Order = 5)]
        [DetailMetadata(Display = "Weight in grams", Order = 5, Tab = 1)]
        public int WeightInGrams { get; set; }


        [DetailMetadata(Display = "Food Category", Template = "DropDownList", Order = 4, Tab = 1)]
        [DynamicLink("FoodCategory")]
        public Guid FoodCategoryGuid { get; set; }

        [DetailMetadata(Display = "Partner", Template = "DropDownList", Order = 3, Tab = 1)]
        [DynamicLink("Partner")]
        public Guid PartnerGuid { get; set; }


        [ListMetadata(Display = "Image", IsHidden = false, Order = 1, NoSort = true, Template = "Image", Width = 210)]
        [DetailMetadata(Display = "Image", Template = "Image", Order = 1, Tab = 1)]
        [ImageMetadata]
        public string ImageId { get; set; }
    }
}
