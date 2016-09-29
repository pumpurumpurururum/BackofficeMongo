using System;
using BaseCms.Common.Validation.Attributes;
using BaseCms.Views.Detail.Attributes;
using BaseCms.Views.Link.Attributes;
using BaseCms.Views.List.Attributes;
using BaseCms.Views.List.ListViewMetadata.Attributes;
using BaseCms.Views.List.ListViewMetadata.Enums;

namespace BackofficeDemo.Model.Metadata
{
    public class PartnerUserMetadata
    {
        [ListMetadata(Display = "Id", IsHidden = true, Order = 0)]
        [DetailMetadata(IsEditable = false, IsKey = true, Tab = 3, IsHidden = false)]
        public Guid Id { get; set; }

        [ListMetadata(Display = "Name", IsHidden = false, Order = 1)]
        [DetailMetadata(Display = "Name", Order = 1, IsHeaderDisplayProperty = true, Tab = 1)]
        [Required]
        public string UserName { get; set; }

        
        public string SecurityStamp { get; set; }

        [ListMetadata(Display = "Email", IsHidden = true, Order = 1)]
        [DetailMetadata(Display = "Email", Order = 2, IsHeaderDisplayProperty = true, Tab = 1)]
        public string Email { get; set; }

        [ListMetadata(Display = "Email Confirmed", IsHidden = false, Order = 3)]
        [DetailMetadata(Display = "Email Confirmed", Order = 3, IsHeaderDisplayProperty = true, Tab = 1)]
        public bool EmailConfirmed { get; set; }

        [ListMetadata(Display = "Phone Number", IsHidden = false, Order = 2)]
        [DetailMetadata(Display = "Phone Number", Order = 4, IsHeaderDisplayProperty = true, Tab = 1)]
        [Required]
        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public DateTime? LockoutEndDateUtc { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }
        [ListMetadata(Display = "Password Hash", IsHidden = true, Order = 2)]
        [DetailMetadata(Display = "Password Hash", Order = 6, IsHeaderDisplayProperty = true, Tab = 1)]
        [Required]
        public string PasswordHash { get; set; }

        [DetailMetadata(Display = "Partner", Template = "DropDownList", Order = 0, Tab = 1)]
        [DynamicLink("Partner")]
        [Required]
        public Guid PartnerGuid { get; set; }

        [DetailMetadata(IsHidden = false, Template = "ListView", Tab = 2)]
        [ListViewMetadata(FooterPartial = "LinkedCollectionDropDown", Buttons = ActionButtons.UnlinkButton, LinkedCollectionType = typeof(PartnerUserRole))]
        [DynamicLink("PartnerUserRole", DetailViewSave = true)]
        public int? RolesCollection { get; set; }


    }
}
