using System.Xml.Serialization;

namespace BaseCms.CRUDRepository.Serialization.Interfaces
{
    public interface ISerializer
    {
        string Serialize(IXmlSerializable serializableObject);
        T Deserialize<T>(string serializableObject) where T : class;

        object CopyObject(XmlSerializableArgumentBase o);

        void SetIgnoreAttributes(object obj, XmlAttributeOverrides overrides);
    }
}
