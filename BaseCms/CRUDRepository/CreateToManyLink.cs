using BaseCms.CRUDRepository.Core;

namespace BaseCms.CRUDRepository
{
    public class CreateToManyLink : QueryBase<string>
    {
        public string UpperIdentifier { get; set; }
        public string LinkedIdentifier { get; set; }
        public string AdditionalData { get; set; }

        public string DetailViewGuid { get; set; }

        public CreateToManyLink(string upperIdentifier, string linkedIdentifier, string additionalData)
        {
            UpperIdentifier = upperIdentifier;
            LinkedIdentifier = linkedIdentifier;
            AdditionalData = additionalData;
            DetailViewGuid = null;
        }

        public CreateToManyLink(string upperIdentifier, string linkedIdentifier, string additionalData, string detailViewGuid)
        {
            UpperIdentifier = upperIdentifier;
            LinkedIdentifier = linkedIdentifier;
            AdditionalData = additionalData;
            DetailViewGuid = detailViewGuid;
        }

        public CreateToManyLink()
        {
        }
    }
}
