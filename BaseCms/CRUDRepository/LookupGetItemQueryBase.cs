using BaseCms.CRUDRepository.Core;

namespace BaseCms.CRUDRepository
{
    public class LookupGetItemQueryBase : QueryBase<object>
    {
        public LookupGetItemQueryBase(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}
