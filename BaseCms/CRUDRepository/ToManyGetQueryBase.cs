using System.Collections.Generic;
using BaseCms.CRUDRepository.Core;

namespace BaseCms.CRUDRepository
{
    public class ToManyGetQueryBase : QueryBase<IEnumerable<object>>
    {
        public ToManyGetQueryBase(string identifier)
        {
            Identifier = identifier;
        }
        public string Identifier { get; set; }
    }
}
