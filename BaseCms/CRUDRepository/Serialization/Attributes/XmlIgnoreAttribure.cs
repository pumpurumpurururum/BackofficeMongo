using System.Xml.Serialization;

namespace BaseCms.CRUDRepository.Serialization.Attributes
{
    public static class XmlIgnoreAttribure
    {
        private readonly static XmlAttributes IgnoreAttribute = new XmlAttributes
        {
            XmlIgnore = true
        };

        public static XmlAttributes Instance
        {
            get { return IgnoreAttribute; }
        }
    }
}
