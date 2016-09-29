namespace BaseCms.MetadataValueProviders.Interfaces
{
    public interface IMetadataValueProvider<in T1, out T2>
    {
        T2 GetValue(T1 key);
    }
}
