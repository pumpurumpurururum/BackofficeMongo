using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BaseCms.CRUDRepository.Serialization
{
    public class XmlSerializableArgumentBase
    {
        public virtual void SetSpecificXmlAttributes(XmlAttributeOverrides overrides) { }
    }
}
