using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace BaseCms.Helpers
{
    public class DataContractSerializerHelper
    {
        public static string Serialize<T>(T objectToSerialize) where T : class
        {
            if (objectToSerialize == null) return null;

            var ms = new MemoryStream();
            var xmlWriter = XmlDictionaryWriter.CreateTextWriter(ms);

            var serializer = new DataContractSerializer(objectToSerialize.GetType());

            serializer.WriteObject(xmlWriter, objectToSerialize);
            xmlWriter.Flush();

            return Encoding.UTF8.GetString(ms.ToArray());
        }

        public static T Deserialize<T>(string xml) where T : class
        {
            if (string.IsNullOrEmpty(xml)) return null;

            var serializer = new DataContractSerializer(typeof(T));
            var reader = new StringReader(xml);
            return serializer.ReadObject(XmlReader.Create(reader)) as T;
        }
    }
}
