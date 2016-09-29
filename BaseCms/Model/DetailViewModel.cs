using System.Collections.Generic;
using BaseCms.Views.ActionButtons;
using BaseCms.Views.Detail.Interfaces;

namespace BaseCms.Model
{
    public class DetailViewModel
    {
        public DataWithIdentifier<object, ObjectCollectionWithId> Structure { get; set; }

        public IDetailMetadata DefaultDetailMetadata { get; set; }

        public IEnumerable<ActionButton> Buttons { get; set; }
    }
}
