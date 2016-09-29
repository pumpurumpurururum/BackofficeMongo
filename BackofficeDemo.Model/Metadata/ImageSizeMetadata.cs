using System;
using BaseCms.Common.Validation.Attributes;
using BaseCms.Views.Detail.Attributes;
using BaseCms.Views.List.Attributes;

namespace BackofficeDemo.Model.Metadata
{
    public class ImageSizeMetadata
    {
        [ListMetadata(Display = "Id", IsHidden = true, Order = 0)]
        [DetailMetadata(IsEditable = false, IsKey = true, Tab = 2, IsHidden = false)]
        public Guid Id { get; set; }

        [ListMetadata(Display = "Key", IsHidden = false, Order = 1)]
        [DetailMetadata(Display = "Key", Order = 1, IsHeaderDisplayProperty = true, Tab = 1)]
        [Required]
        public string Key { get; set; }

        [ListMetadata(Display = "Width", IsHidden = false, Order = 2)]
        [DetailMetadata(Display = "Width", Order = 2, IsHeaderDisplayProperty = true, Tab = 1)]
        public int Width { get; set; }

        [ListMetadata(Display = "Height", IsHidden = false, Order = 3)]
        [DetailMetadata(Display = "Height", Order = 3, IsHeaderDisplayProperty = true, Tab = 1)]
        public int Height { get; set; }
    }
}
