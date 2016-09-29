using System;
using System.Runtime.Serialization;

namespace BaseCms.Security
{
    public class ChangeObjectStateIdPermission : CollectionPermission
    {
        [IgnoreDataMember]
        public string StateName { get; set; }
        public int StateId { get; set; }

        public override string Description
        {
            get { return String.Format("Изменение состояния на \"{0}\"", StateName); }
        }
    }
}
