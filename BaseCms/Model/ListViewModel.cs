using BaseCms.Views.Detail;
using BaseCms.Views.List.ListViewMetadata;

namespace BaseCms.Model
{
    public class ListViewModel
    {
        public bool IsEditable { get; set; }

        public string Url { get; set; }
        public DataWithIdentifier<object, string> Structure { get; set; }
        public string UpperIdentifier { get; set; }
        public string UpperCollectionName { get; set; }

        // Guid редактора
        public string DetailViewGuid { get; set; }

        // Настройки ListView
        public ListViewMetadata ListViewSettings { get; set; }

        // Настройки Popup
        public PopupMetadata PopupSettings { get; set; }
    }
}
