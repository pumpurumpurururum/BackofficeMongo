using System.Collections.Generic;
using BaseCms.CRUDRepository.Core;

namespace BaseCms.CRUDRepository
{
    public class GetByIdsQueryBase : QueryBase<IEnumerable<object>>
    {
        public string[] Ids { get; set; }
        public string DetailViewGuid { get; set; }

        public GetByIdsQueryBase(string[] id)
        {
            Ids = id;
            DetailViewGuid = null;
        }

        public GetByIdsQueryBase(string[] id, string detailViewGuid)
        {
            Ids = id;
            DetailViewGuid = detailViewGuid;
        }
    }
}
