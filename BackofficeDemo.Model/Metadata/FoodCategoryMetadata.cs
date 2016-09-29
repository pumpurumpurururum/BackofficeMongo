using System;
using BaseCms.Common.Validation.Attributes;
using BaseCms.Views.Detail.Attributes;
using BaseCms.Views.List.Attributes;

namespace BackofficeDemo.Model.Metadata
{
    public class FoodCategoryMetadata
    {
        [ListMetadata(Display = "Id", IsHidden = true, Order = 0)]
        [DetailMetadata(IsEditable = false, IsKey = true, Tab = 2, IsHidden = false)]
        public Guid Id { get; set; }

        [ListMetadata(Display = "Name", IsHidden = false, Order = 1)]
        [DetailMetadata(Display = "Name", Order = 1, IsHeaderDisplayProperty = true, Tab = 1)]
        [Required]
        public string Name { get; set; }

        [ListMetadata(Display = "Alias", IsHidden = false, Order = 2)]
        [DetailMetadata(MaxLength = 50, IsEditable = false, Tab = 1, Order = 2)]
        public string Alias { get; set; }

        public Guid PartnerGuid { get; set; }

        [ListMetadata(Order = 6)]
        [DetailMetadata(Order = 7, Tab = 1)]
        public Boolean Active { get; set; }
    }
}
