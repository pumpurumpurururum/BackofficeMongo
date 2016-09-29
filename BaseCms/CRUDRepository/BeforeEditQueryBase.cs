using BaseCms.CRUDRepository.Core;

namespace BaseCms.CRUDRepository
{
    public class BeforeEditQueryBase : QueryBase<EmptyParameter>
    {
        public BeforeEditQueryBase(object beingEditedObject)
        {
            BeingEditedObject = beingEditedObject;
        }

        public object BeingEditedObject { get; set; }
    }
}
