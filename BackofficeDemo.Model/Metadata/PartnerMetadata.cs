using System;
using BaseCms.Common.Image.Attributes;
using BaseCms.Common.Validation.Attributes;
using BaseCms.Views.Detail.Attributes;
using BaseCms.Views.Link.Attributes;
using BaseCms.Views.List.Attributes;
using BaseCms.Views.List.ListViewMetadata.Attributes;
using BaseCms.Views.List.ListViewMetadata.Enums;

namespace BackofficeDemo.Model.Metadata
{
    public class PartnerMetadata
    {
        [ListMetadata(Display = "Id", IsHidden = true, Order = 0)]
        [DetailMetadata(IsEditable = false, IsKey = true, Tab = 9, IsHidden = false)]
        public Guid Id { get; set; }

        [ListMetadata(Display = "Name", IsHidden = false, Order = 1)]
        [DetailMetadata(Display = "Name", Order = 1, IsHeaderDisplayProperty = true, Tab = 1)]
        [Required]
        public string Name { get; set; }


        [DetailMetadata(MaxLength = 50, IsEditable = false, Tab = 1, Order = 2)]
        public string Alias { get; set; }
        [DetailMetadata(Tab = 1, Order = 3)]
        public int BonusPercent { get; set; }

        [ListMetadata(Display = "Minimum price", IsHidden = false, Order = 3)]
        [DetailMetadata(Display = "Minimum price", Order = 3, Tab = 1)]
        public decimal MinimumPrice { get; set; }

        [ListMetadata(Display = "Delivery price", IsHidden = false, Order = 4)]
        [DetailMetadata(Display = "Delivery price", Order = 4, Tab = 1)]
        public decimal DeliveryPrice { get; set; }

        
        [DetailMetadata(Display = "Free delivery price", Order = 5, Tab = 1)]
        public decimal FreeDeliveryAfterPrice { get; set; }

        //[DetailMetadata(Display = "Free delivery", Order = 7, Tab = 1)]
        //public Boolean FreeDelivery { get; set; }

        [DetailMetadata(Display = "Delivery time", Order = 6, Tab = 1)]
        public int AverageDeliveryTime { get; set; }

        [DetailMetadata(Display = "Registration date", Order = 2, IsEditable = false, Tab = 4)]
        [Required]
        public DateTime RegisterDate { get; set; }

        [DetailMetadata(Display = "Type", Template = "DropDownList", Order = 6, Tab = 1)]
        [DynamicLink("PartnerType")]
        public Guid PartnerTypeGuid { get; set; }

        [DetailMetadata(Display = "City", Template = "DropDownList", Order = 7, Tab = 1)]
        [DynamicLink("City")]
        public Guid CityGuid { get; set; }


        [ListMetadata(Display = "Image", IsHidden = false, Order = 0, NoSort = true, Template = "Image", Width = 210)]
        [DetailMetadata(Display = "Image", Template = "Image", Order = 1, Tab = 7)]
        [ImageMetadata]
        public string ImageId { get; set; }

        [DetailMetadata(Display = "Notitfication email", Template = "Email", Order = 1, IsEditable = false, Tab = 8)]
        public string NotitficationEmail { get; set; }

        [DetailMetadata(Display = "Notitfication phone", Template = "MaskPhone", Order = 2, IsEditable = false, Tab = 8)]
        public string NotitficationPhone { get; set; }


        [ListMetadata(Order = 6)]
        [DetailMetadata(Order = 7, Tab = 1)]
        public Boolean Active { get; set; }

        [DetailMetadata(Order = 1, Tab = 5, Block = 1)]
        public Boolean WorkingNow { get; set; }

        [DetailMetadata(Order = 1, Tab = 5, Block = 2)]
        public bool WorkingHoursMonday { get; set; }
        [DetailMetadata(Order = 2, Tab = 5, Block = 2)]
        public TimeSpan WorkingHoursMondayOpen { get; set; }
        [DetailMetadata(Order = 3, Tab = 5, Block = 2)]
        public TimeSpan WorkingHoursMondayClose { get; set; }
        [DetailMetadata(Order = 4, Tab = 5, Block = 3)]
        public bool WorkingHoursTuesday { get; set; }
        [DetailMetadata(Order = 5, Tab = 5, Block = 3)]
        public TimeSpan WorkingHoursTuesdayOpen { get; set; }
        [DetailMetadata(Order = 6, Tab = 5, Block = 3)]
        public TimeSpan WorkingHoursTuesdayClose { get; set; }
        [DetailMetadata(Order = 7, Tab = 5, Block = 4)]
        public bool WorkingHoursWednesday { get; set; }
        [DetailMetadata(Order = 8, Tab = 5, Block = 4)]
        public TimeSpan WorkingHoursWednesdayOpen { get; set; }
        [DetailMetadata(Order = 9, Tab = 5, Block = 4)]
        public TimeSpan WorkingHoursWednesdayClose { get; set; }
        [DetailMetadata(Order = 10, Tab = 5, Block = 5)]
        public bool WorkingHoursThursday { get; set; }
        [DetailMetadata(Order = 11, Tab = 5, Block = 5)]
        public TimeSpan WorkingHoursThursdayOpen { get; set; }
        [DetailMetadata(Order = 12, Tab = 5, Block = 5)]
        public TimeSpan WorkingHoursThursdayClose { get; set; }
        [DetailMetadata(Order = 13, Tab = 5, Block = 6)]
        public bool WorkingHoursFriday { get; set; }
        [DetailMetadata(Order = 14, Tab = 5, Block = 6)]
        public TimeSpan WorkingHoursFridayOpen { get; set; }
        [DetailMetadata(Order = 15, Tab = 5, Block = 6)]
        public TimeSpan WorkingHoursFridayClose { get; set; }
        [DetailMetadata(Order = 16, Tab = 5, Block = 7)]
        public bool WorkingHoursSaturday { get; set; }
        [DetailMetadata(Order = 17, Tab = 5, Block = 7)]
        public TimeSpan WorkingHoursSaturdayOpen { get; set; }
        [DetailMetadata(Order = 18, Tab = 5, Block = 7)]
        public TimeSpan WorkingHoursSaturdayClose { get; set; }
        [DetailMetadata(Order = 19, Tab = 5, Block = 8)]
        public bool WorkingHoursSunday { get; set; }
        [DetailMetadata(Order = 20, Tab = 5, Block = 8)]
        public TimeSpan WorkingHoursSundayOpen { get; set; }
        [DetailMetadata(Order = 21, Tab = 5, Block = 8)]
        public TimeSpan WorkingHoursSundayClose { get; set; }

        [DetailMetadata(Template = "Html", Tab = 6)]
        public string Description { get; set; }


        [DetailMetadata(IsHidden = false, Template = "ListView", Tab = 2)]
        [ListViewMetadata(/*FooterPartial = "AddMenuCategoryToPartnerByName", Buttons = ActionButtons.DeleteButton,*/ LinkedCollectionType = typeof(FoodCategory))]
        [DynamicLink("FoodCategory", DetailViewSave = true)]
        public int? Tech_FoodCategories { get; set; }


        [DetailMetadata(IsHidden = false, Template = "ListView", Tab = 3)]
        [ListViewMetadata(FooterPartial = "LinkedCollectionDropDown", Buttons = ActionButtons.UnlinkButton, LinkedCollectionType = typeof(PartnerCategory))]
        [DynamicLink("PartnerCategory", DetailViewSave = true)]
        public int? Tech_PartnerCategories { get; set; }

        [DetailMetadata(IsHidden = false, Template = "ListView", Tab = 4)]
        [ListViewMetadata(Buttons = ActionButtons.DeleteButton, LinkedCollectionType = typeof(PartnerUser))]
        [DynamicLink("PartnerUser", DetailViewSave = true)]
        public int? Tech_PartnerUsers { get; set; }

    }
}
