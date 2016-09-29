using BaseCms.Views.Detail.Interfaces;

namespace BaseCms.Model
{
    public class XmlEditorViewModel
    {
        public DataWithIdentifier<object, string> Structure { get; set; }

        public IDetailMetadata XmlDetailMetadata { get; set; }
    }
}
