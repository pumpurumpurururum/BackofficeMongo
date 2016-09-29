using BaseCms.Views.Detail.Enums;

namespace BaseCms.Views.Detail
{
    public class LinkMetadata
    {
        public LinkMultiplier Multiplier { get; set; }
        public bool IsPreloaded { get; set; }
        public string CollectionName { get; set; }
        public bool DetailViewSave { get; set; }
        //my
        public string RelatedList { get; set; }
        public string GroupName { get; set; }
    }
}
