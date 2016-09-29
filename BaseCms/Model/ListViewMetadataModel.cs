using System;
using BaseCms.Views.Detail;

namespace BaseCms.Model
{
    public class ListViewMetadataModel
    {
        public string DataTableId { get; set; }
        public string DataTableName { get; set; }
        public string DataTableUrl { get; set; }

        public string CollectionName { get; set; }

        public string UpperCollectionName { get; set; }
        public string UpperIdentifier { get; set; }

        public string DetailViewGuid { get; set; }

        public Type ListMetadataProviderType { get; set; }
        public Type PopupMetadataProviderType { get; set; }
        public string UploadImagesUrl { get; set; }

        // Настройки Popup
        public PopupMetadata PopupSettings { get; set; }
    }
}
