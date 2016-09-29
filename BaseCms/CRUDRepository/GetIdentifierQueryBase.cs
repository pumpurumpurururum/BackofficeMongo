using BaseCms.CRUDRepository.Core;

namespace BaseCms.CRUDRepository
{
    public class GetIdentifierQueryBase : QueryBase<string>
    {
        public GetIdentifierQueryBase(object o)
        {
            Object = o;
        }

        public object Object { get; set; }
    }
}
