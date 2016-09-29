using System.Collections.Generic;
using BaseCms.CRUDRepository.Core;

namespace BaseCms.CRUDRepository
{
    public class ToManySave : QueryBase<EmptyParameter>
    {
        public ToManySave(string identifier, IEnumerable<string> identifiers)
        {
            Identifier = identifier;
            Identifiers = identifiers;
        }

        public string Identifier { get; set; }
        public IEnumerable<string> Identifiers { get; set; }
    }
}
