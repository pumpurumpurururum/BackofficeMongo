using System.Collections.Generic;
using BaseCms.CRUDRepository.Core;

namespace BaseCms.CRUDRepository
{
    public class GetLinkedListQueryBase : QueryBase<IEnumerable<object>>
    {
        public GetLinkedListQueryBase(string search, int max, string upperIdentifier, string detailViewGuid, string filter)
        {
            Search = search;
            Max = max;
            UpperIdentifier = upperIdentifier;
            DetailViewGuid = detailViewGuid;
            Filter = filter;
        }

        public string Search { get; set; }
        public int Max { get; set; }

        public string UpperIdentifier { get; set; }
        public string DetailViewGuid { get; set; }

        public string Filter { get; set; }
    }
}
