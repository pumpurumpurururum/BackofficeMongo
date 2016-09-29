using System;
using System.Xml.Serialization;
using BaseCms.CRUDRepository.Core;
using BaseCms.CRUDRepository.Serialization;
using BaseCms.CRUDRepository.Serialization.Interfaces;
using BaseCms.DependencyResolution;
using IXmlSerializable = BaseCms.CRUDRepository.Serialization.Interfaces.IXmlSerializable;

namespace BaseCms.CRUDRepository
{
    public class InsertObject : QueryBase<string>, IXmlSerializable
    {
        public InsertObject()
        {

        }

        public InsertObject(object o)
        {
            Object = o;
            DetailViewGuid = null;
        }

        public InsertObject(object o, string detailViewGuid)
        {
            Object = o;
            DetailViewGuid = detailViewGuid;
        }

        public InsertObject(object o, string detailViewGuid, string linkedDetailViewGuid)
        {
            Object = o;
            DetailViewGuid = detailViewGuid;
            LinkedDetailViewGuid = linkedDetailViewGuid;
        }

        //public InsertObject(object o, string detailViewGuid, string linkedDetailViewGuid, string upperIdentifier)
        //{
        //    Object = o;
        //    DetailViewGuid = detailViewGuid;
        //    LinkedDetailViewGuid = linkedDetailViewGuid;
        //    UpperIdentifier = upperIdentifier;
        //}

        public object Object { get; set; }
        public string DetailViewGuid { get; set; }
        public string LinkedDetailViewGuid { get; set; }

        //public string UpperIdentifier { get; set; }

        public XmlAttributeOverrides GetXmlOverrides()
        {
            var xmlOverrides = new XmlAttributeOverrides();

            // Сообщаем xml-сериализатору, какой тип имеет поле Object
            var objAtts = new XmlAttributes();
            objAtts.XmlElements.Add(new XmlElementAttribute(Object.GetType()));
            xmlOverrides.Add(GetType(), "Object", objAtts);

            // Устанавливаем атрибуты Ignore для полей типа Интерфейс и Класс
            IoC.Container.GetInstance<ISerializer>().SetIgnoreAttributes(Object, xmlOverrides);

            // Сообщаем xml-сериализатору дополнительные сведения об объекте Object
            // (например, какие поля задействовать в сериализации, какие игнорировать)
            var specificXmlOverrides = Object as XmlSerializableArgumentBase;
            specificXmlOverrides?.SetSpecificXmlAttributes(xmlOverrides);

            XmlIgnoreAttributeSetter.SetXmlIgnoreAttributes(xmlOverrides, new[] { GetType() },
                                                            new[] { "DetailViewGuid", "LinkedDetailViewGuid" });

            return xmlOverrides;
        }

        public string GetObjectId(string collectionName)
        {
            return IoC.Container.GetInstance<QueryResolver>().Execute(new GetIdentifierQueryBase(Object), collectionName);
        }

        public bool NeedToLog()
        {
            return String.IsNullOrEmpty(DetailViewGuid);
        }
    }
}
