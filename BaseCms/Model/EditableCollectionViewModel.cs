using System;
using System.Collections.Generic;

namespace BaseCms.Model
{
    public class EditableCollectionViewModel
    {
        public IEnumerable<object> Collection { get; set; }
        public Type Type { get; set; }

        public string UpperIdentifier { get; set; }

        public string DetailViewGuid { get; set; }
    }
}
