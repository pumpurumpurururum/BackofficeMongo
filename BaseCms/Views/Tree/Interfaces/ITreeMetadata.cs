using System.Collections.Generic;

namespace BaseCms.Views.Tree.Interfaces
{
    public interface ITreeMetadata
    {
        IEnumerable<TreeMetadataItem> Items { get; }
        Dictionary<string, object> Serialize(IEnumerable<object> objects);
    }
}
