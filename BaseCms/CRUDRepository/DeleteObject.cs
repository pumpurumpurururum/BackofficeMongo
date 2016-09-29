using System;
using System.Xml.Serialization;
using BaseCms.CRUDRepository.Core;
using BaseCms.CRUDRepository.Serialization;
using IXmlSerializable = BaseCms.CRUDRepository.Serialization.Interfaces.IXmlSerializable;

namespace BaseCms.CRUDRepository
{
    public class DeleteObject : QueryBase<EmptyParameter>, IXmlSerializable
    {
        public DeleteObject()
        {

        }

        public DeleteObject(string id)
        {
            Id = id;
            UpperIdentifier = null;
            DetailViewGuid = null;
        }

        public DeleteObject(string id, string detailViewGuid, string upperIdentifier)
        {
            Id = id;
            DetailViewGuid = detailViewGuid;
            UpperIdentifier = upperIdentifier;
        }

        public DeleteObject(string id, string detailViewGuid, string upperIdentifier, string linkedDetailViewGuid)
        {
            Id = id;
            DetailViewGuid = detailViewGuid;
            UpperIdentifier = upperIdentifier;
            LinkedDetailViewGuid = linkedDetailViewGuid;
        }

        public string Id { get; set; }
        public string UpperIdentifier { get; set; }
        public string DetailViewGuid { get; set; }
        public string LinkedDetailViewGuid { get; set; }

        public XmlAttributeOverrides GetXmlOverrides()
        {
            var xmlOverrides = new XmlAttributeOverrides();

            XmlIgnoreAttributeSetter.SetXmlIgnoreAttributes(xmlOverrides, new[] { GetType() },
                                                            new[]
                                                                {
                                                                    "Object"
                                                                });

            XmlIgnoreAttributeSetter.SetXmlIgnoreAttributes(xmlOverrides, new[] { GetType() },
                                                            new[] { "UpperIdentifier", "DetailViewGuid", "LinkedDetailViewGuid" });

            return xmlOverrides;
        }

        public string GetObjectId(string collectionName)
        {
            return Id;
        }

        public bool NeedToLog()
        {
            return String.IsNullOrEmpty(DetailViewGuid);
        }
    }
}
