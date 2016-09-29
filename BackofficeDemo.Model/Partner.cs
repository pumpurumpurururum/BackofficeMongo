using System;
using System.Collections.Generic;
using BaseCms.Common.Attributes;
using BaseCms.Model.Interfaces;
using BackofficeDemo.Model.Enums;
using BackofficeDemo.Model.Metadata;
using BackofficeDemo.MongoBase;

namespace BackofficeDemo.Model
{
    [CmsMetadataType(typeof(PartnerMetadata),
        CollectionTitleSingular = "Partner",
        CollectionTitlePlural = "Partners",
        CustomEditView = "EditWithTabs",
        TabsWithBlocks = "Main;Food categories;Partner categories;Users;Working Hours|,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday;Description;Logo;Notification;Tech Info")]
    public class Partner:Entity, IAlias
    {
        public PaymentType PaymentTypeAvailable { get; set; }

        public Guid CityGuid { get; set; }

        public string Alias { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal MinimumPrice { get; set; }

        public decimal DeliveryPrice { get; set; }

        public decimal FreeDeliveryAfterPrice { get; set; }
        

        public int AverageDeliveryTime { get; set; }

        public DateTime RegisterDate { get; set; }

        public Guid PartnerTypeGuid { get; set; }

        public Boolean Active { get; set; }

        public Boolean WorkingNow { get; set; }

        public List<Guid> PartnerCategoryGuids { get; set; }

        public string NotitficationEmail { get; set; }


        public string NotitficationPhone { get; set; }


        public string ImageId { get; set; }

        public int NegativeRating { get; set; }
        public int PositiveRating { get; set; }

        public int BonusPercent { get; set; }

        #region Working hours
        public bool WorkingHoursMonday { get; set; }

        public TimeSpan WorkingHoursMondayOpen { get; set; }

        public TimeSpan WorkingHoursMondayClose { get; set; }

        public bool WorkingHoursTuesday { get; set; }

        public TimeSpan WorkingHoursTuesdayOpen { get; set; }

        public TimeSpan WorkingHoursTuesdayClose { get; set; }

        public bool WorkingHoursWednesday { get; set; }

        public TimeSpan WorkingHoursWednesdayOpen { get; set; }

        public TimeSpan WorkingHoursWednesdayClose { get; set; }

        public bool WorkingHoursThursday { get; set; }

        public TimeSpan WorkingHoursThursdayOpen { get; set; }

        public TimeSpan WorkingHoursThursdayClose { get; set; }

        public bool WorkingHoursFriday { get; set; }

        public TimeSpan WorkingHoursFridayOpen { get; set; }

        public TimeSpan WorkingHoursFridayClose { get; set; }

        public bool WorkingHoursSaturday { get; set; }

        public TimeSpan WorkingHoursSaturdayOpen { get; set; }

        public TimeSpan WorkingHoursSaturdayClose { get; set; }

        public bool WorkingHoursSunday { get; set; }

        public TimeSpan WorkingHoursSundayOpen { get; set; }

        public TimeSpan WorkingHoursSundayClose { get; set; }

        #endregion


        public string AddressForMap { get; set; }
        public string TwitterLink { get; set; }
        public string GooglePlusLink { get; set; }
        public string FacebookLink { get; set; }
        public string DeliveryDescription { get; set; }


    }
}
