using BaseCms.CRUDRepository.Core;

namespace BaseCms.CRUDRepository
{
    public class MoveUpDown : QueryBase<EmptyParameter>
    {
        public MoveUpDown(string identifier, string upperIdentifier, string detailViewGuid, bool up)
        {
            Identifier = identifier;
            UpperIdentifier = upperIdentifier;
            DetailViewGuid = detailViewGuid;
            Up = up;
        }

        public string Identifier { get; set; }
        public string UpperIdentifier { get; set; }
        public string DetailViewGuid { get; set; }
        public bool Up { get; set; }
    }
}
