using System.Collections.Generic;
using BaseCms.CRUDRepository.Core;

namespace BaseCms.CRUDRepository
{
    public class LookupListWithAddDataQueryBase : QueryBase<IEnumerable<object>>
    {
        public LookupListWithAddDataQueryBase(string search, int max, string filter)
        {
            Search = search;
            Max = max;
            Filter = filter;
        }

        public string Search { get; set; }
        public int Max { get; set; }
        public string Filter { get; set; }
    }
}
