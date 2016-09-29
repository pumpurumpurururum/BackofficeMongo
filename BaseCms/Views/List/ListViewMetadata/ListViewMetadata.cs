using System;

namespace BaseCms.Views.List.ListViewMetadata
{
    public class ListViewMetadata
    {
        public Enums.ActionButtons Buttons { get; set; }
        public string CustomButtonsContent { get; set; }
        public int CustomButtonsWidth { get; set; }
        public string FilterPartial { get; set; }
        public string FooterPartial { get; set; }
        public string JDataTableDom { get; set; }
        public string JScriptFuctionPartial { get; set; }
        public Type LinkedCollectionType { get; set; }
        public Type ListMetadataProviderType { get; set; }
        public string ViewName { get; set; }
        public bool Paginate { get; set; }
        public Type PopupMetadataProviderType { get; set; }
        public string UploadImagesUrl { get; set; }
        public string UploadFileUrl { get; set; }
        public ListViewGroupingMetadata GroupingMetadata { get; set; }
    }
}
