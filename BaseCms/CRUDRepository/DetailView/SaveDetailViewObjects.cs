using BaseCms.CRUDRepository.Core;

namespace BaseCms.CRUDRepository.DetailView
{
    public class SaveDetailViewObjects : QueryBase<EmptyParameter>
    {
        public SaveDetailViewObjects(string detailViewGuid, string newObjectId)
        {
            DetailViewGuid = detailViewGuid;
            NewObjectId = newObjectId;
        }

        public object DetailViewGuid { get; set; }
        public string NewObjectId { get; set; }
    }
}
