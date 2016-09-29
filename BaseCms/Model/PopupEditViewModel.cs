namespace BaseCms.Model
{
    public class PopupEditViewModel
    {
        public DataWithIdentifier<object, ObjectCollectionWithId> DataWithIdentifier { get; set; }
        public string DetailViewGuid { get; set; }
        public string UpperCollectionName { get; set; }
        public string UpperIdentifier { get; set; }

        public string CustomDetailMetadataProvider { get; set; }

        public string Title { get; set; }

        public string Height { get; set; }
        public string Width { get; set; }

        public string Top { get; set; }
        public string Left { get; set; }

        public bool NoScroll { get; set; }

        public int ScreenWidth { get; set; }
    }
}
