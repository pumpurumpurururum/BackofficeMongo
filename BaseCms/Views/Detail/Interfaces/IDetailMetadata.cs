using System.Collections.Generic;

namespace BaseCms.Views.Detail.Interfaces
{
    public interface IDetailMetadata
    {
        IEnumerable<DetailMetadataItem> Items { get; set; }
        string CustomEditView { get; set; }
        string CustomReadView { get; set; }
        bool ForbidReadMode { get; set; }

        string ListViewHeaderPlural { get; set; }
        string TabsWithBlocks { get; set; }

        object GetValue(object data, string propertyName);
        DetailMetadataItem GetItem(string propertyName);
    }
}
