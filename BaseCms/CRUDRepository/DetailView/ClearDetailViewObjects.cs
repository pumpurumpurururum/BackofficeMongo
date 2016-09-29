using BaseCms.CRUDRepository.Core;

namespace BaseCms.CRUDRepository.DetailView
{
    public class ClearDetailViewObjects : QueryBase<EmptyParameter>
    {
        public ClearDetailViewObjects(string detailViewGuid)
        {
            DetailViewGuid = detailViewGuid;
        }

        public object DetailViewGuid { get; set; }
    }
}
