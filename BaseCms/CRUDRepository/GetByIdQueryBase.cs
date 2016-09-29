using BaseCms.CRUDRepository.Core;

namespace BaseCms.CRUDRepository
{
    public class GetByIdQueryBase : QueryBase<object>
    {
        public string Id { get; set; }
        public string DetailViewGuid { get; set; }

        public GetByIdQueryBase(string id)
        {
            Id = id;
            DetailViewGuid = null;
        }

        public GetByIdQueryBase(string id, string detailViewGuid)
        {
            Id = id;
            DetailViewGuid = detailViewGuid;
        }
    }
}
