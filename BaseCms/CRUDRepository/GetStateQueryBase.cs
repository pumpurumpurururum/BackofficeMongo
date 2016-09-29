using BaseCms.CRUDRepository.Core;
using BaseCms.StateRepository.Base;

namespace BaseCms.CRUDRepository
{
    public class GetStateQueryBase : QueryBase<StateBase>
    {
        public GetStateQueryBase(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}
