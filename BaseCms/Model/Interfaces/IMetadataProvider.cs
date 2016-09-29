using System;

namespace BaseCms.Model.Interfaces
{
    public interface IMetadataProvider<out T2>
    {
        T2 GetMetadata(Type type);
    }
}
