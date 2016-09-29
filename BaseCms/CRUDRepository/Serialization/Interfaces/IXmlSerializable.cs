using System.Xml.Serialization;

namespace BaseCms.CRUDRepository.Serialization.Interfaces
{
    public interface IXmlSerializable
    {
        XmlAttributeOverrides GetXmlOverrides();

        string GetObjectId(string collectionName);

        bool NeedToLog();
    }
}
