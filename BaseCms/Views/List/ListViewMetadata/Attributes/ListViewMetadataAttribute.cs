using System;

namespace BaseCms.Views.List.ListViewMetadata.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ListViewMetadataAttribute : Attribute
    {
        public ListViewMetadataAttribute()
        {
            Buttons = 0;
            CustomButtonsContent = string.Empty;
            CustomButtonsWidth = 0;
            FilterPartial = string.Empty;
            FooterPartial = string.Empty;
            JDataTableDom = "tr<'row'<'col-md-4'l><'col-md-4'i><'col-md-4'p>>";
            JScriptFuctionPartial = string.Empty;
            LinkedCollectionType = null;
            ListMetadataProviderType = null;
            ViewName = "ListView";
            Paginate = true;
            PopupMetadataProviderType = null;
            UploadImagesUrl = string.Empty;
            UploadFileUrl = string.Empty;
        }

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
    }
}
