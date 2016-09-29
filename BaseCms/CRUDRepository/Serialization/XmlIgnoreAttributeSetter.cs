using System;
using System.Linq;
using System.Xml.Serialization;
using BaseCms.CRUDRepository.Serialization.Attributes;

namespace BaseCms.CRUDRepository.Serialization
{
    public static class XmlIgnoreAttributeSetter
    {
        public static void SetXmlIgnoreAttributes(XmlAttributeOverrides overrides, Type[] types, string[] memberNames)
        {
            foreach (var type in types.Distinct())
            {
                foreach (var memberName in memberNames)
                {
                    overrides.Add(type, memberName, XmlIgnoreAttribure.Instance);
                }
            }
        }
    }
}
