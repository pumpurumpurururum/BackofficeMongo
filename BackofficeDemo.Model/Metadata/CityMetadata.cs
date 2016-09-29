using System;
using BaseCms.Common.Validation.Attributes;
using BaseCms.Views.Detail.Attributes;
using BaseCms.Views.List.Attributes;

namespace BackofficeDemo.Model.Metadata
{
    public class CityMetadata
    {
        [ListMetadata(Display = "Id", IsHidden = true, Order = 0)]
        [DetailMetadata(IsEditable = false, IsKey = true, Tab = 2, IsHidden = false)]
        public Guid Id { get; set; }

        [ListMetadata(Display = "Name", IsHidden = false, Order = 1)]
        [DetailMetadata(Display = "Name", Order = 1, IsHeaderDisplayProperty = true, Tab = 1)]
        [Required]
        public string LocationName { get; set; }

        [DetailMetadata(Order = 1, IsEditable = false, IsHidden = true, Tab = 1)]
        public string Name { get; set; }


        [ListMetadata(Display = "State", IsHidden = false, Order = 2)]
        [DetailMetadata(Display = "State", Order = 2, IsHeaderDisplayProperty = true, Tab = 1)]
        public string StateKey { get; set; }

        [ListMetadata(Display = "Country", IsHidden = false, Order = 2)]
        [DetailMetadata(Display = "Country", Order = 2, IsHeaderDisplayProperty = true, Tab = 1)]
        public string Country { get; set; }
    }
}
