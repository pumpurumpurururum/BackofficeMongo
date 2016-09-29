using System;
using BaseCms.Common.Image.Attributes;
using BaseCms.Common.Validation.Attributes;
using BaseCms.Views.Detail.Attributes;
using BaseCms.Views.List.Attributes;

namespace BackofficeDemo.Model.Metadata
{
    public class PartnerCategoryMetadata
    {
        [ListMetadata(Display = "Id", IsHidden = true, Order = 0)]
        [DetailMetadata(IsEditable = false, IsKey = true, Tab = 3, IsHidden = false)]
        public Guid Id { get; set; }

        [ListMetadata(Display = "Name", IsHidden = false, Order = 1)]
        [DetailMetadata(Display = "Name", Order = 1, IsHeaderDisplayProperty = true, Tab = 1)]
        [Required]
        public string Name { get; set; }

        [ListMetadata(IsHidden = false, Order = 2)]
        [DetailMetadata(MaxLength = 50, Tab = 1, Order = 2)]
        public string Alias { get; set; }

        [ListMetadata(Display = "Image", IsHidden = false, Order = 0, NoSort = true, Template = "Image", Width = 210)]
        [DetailMetadata(Display = "Image", Template = "Image", Order = 1, Tab = 2)]
        [ImageMetadata]
        public string ImageId { get; set; }
    }
}
