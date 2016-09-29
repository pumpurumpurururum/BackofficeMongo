using System.Xml.Serialization;
using BaseCms.CRUDRepository.Core;
using BaseCms.CRUDRepository.Serialization;
using BaseCms.StateRepository.Base;
using IXmlSerializable = BaseCms.CRUDRepository.Serialization.Interfaces.IXmlSerializable;

namespace BaseCms.CRUDRepository
{
    public class ChangeObjectStateId : QueryBase<StateBase>, IXmlSerializable
    {
        public ChangeObjectStateId()
        {

        }

        public ChangeObjectStateId(string id, string stateId)
        {
            Id = id;
            StateId = stateId;
        }

        public string Id { get; set; }
        public string StateId { get; set; }

        public XmlAttributeOverrides GetXmlOverrides()
        {
            var xmlOverrides = new XmlAttributeOverrides();

            XmlIgnoreAttributeSetter.SetXmlIgnoreAttributes(xmlOverrides, new[] { typeof(QueryBase<StateBase>) },
                                                            new[] { "Output" });
            return xmlOverrides;
        }

        public string GetObjectId(string collectionName)
        {
            return Id;
        }

        public bool NeedToLog()
        {
            return true;
        }
    }
}
