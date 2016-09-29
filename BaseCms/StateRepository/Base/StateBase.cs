using System.Collections.Generic;
using BaseCms.CRUDRepository.Core.Intefaces;
using BaseCms.Views.ActionButtons;
using BaseCms.Views.Detail.Interfaces;

namespace BaseCms.StateRepository.Base
{
    public abstract class StateBase
    {
        public int Value { get; set; }
        public string Name { get; set; }

        public abstract bool CheckPermission(IQuery query, string collectionName, string objectId);
        public abstract IEnumerable<ActionButton> GetButtons(string collectionName, string identifier, IDetailMetadata metadata = null);
    }
}
