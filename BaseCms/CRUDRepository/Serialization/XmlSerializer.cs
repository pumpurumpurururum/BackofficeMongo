using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using BaseCms.CRUDRepository.Serialization.Attributes;
using BaseCms.CRUDRepository.Serialization.Interfaces;
using BaseCms.Model.Interfaces;

namespace BaseCms.CRUDRepository.Serialization
{
    public class XmlSerializer : ISerializer
    {
        public string Serialize(Interfaces.IXmlSerializable serializableObject)
        {
            var overrides = serializableObject.GetXmlOverrides();
            var serializer = new System.Xml.Serialization.XmlSerializer(serializableObject.GetType(), overrides);

            var writer = new StringWriter();
            serializer.Serialize(writer, serializableObject);
            return writer.ToString();
        }

        public T Deserialize<T>(string serializableObject) where T : class
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            return serializer.Deserialize(new StringReader(serializableObject)) as T;
        }

        public object CopyObject(XmlSerializableArgumentBase objSource)
        {
            if (objSource == null) return null;
            using (var stream = new MemoryStream())
            {
                var overrides = new XmlAttributeOverrides();
                SetIgnoreAttributes(objSource, overrides);
                objSource.SetSpecificXmlAttributes(overrides);

                var serializer = new System.Xml.Serialization.XmlSerializer(objSource.GetType(), overrides);
                serializer.Serialize(stream, objSource);
                stream.Position = 0;
                return serializer.Deserialize(stream);
            }
        }

        public void SetIgnoreAttributes(object obj, XmlAttributeOverrides overrides)
        {
            var type = obj.GetType();

            var baseTypes = new List<Type>();
            var baseType = obj.GetType().BaseType;

            if (baseType != null)
            {
                baseTypes.Add(baseType);
                while ((baseType.BaseType != typeof(XmlSerializableArgumentBase)) && (baseType.BaseType != null))
                {
                    baseType = baseType.BaseType;
                    baseTypes.Add(baseType);
                }
            }

            if (obj is IDetailViewObject)
            {
                foreach (var detailViewProperty in typeof(IDetailViewObject).GetProperties())
                {
                    if (overrides[type, detailViewProperty.Name] != null) break;
                    SetAttribute(overrides, type, baseTypes, detailViewProperty.Name, XmlIgnoreAttribure.Instance);
                }
            }

            foreach (var property in type.GetProperties())
            {
                if ((property.PropertyType.IsInterface) && (overrides[type, property.Name] == null))
                {
                    SetAttribute(overrides, type, baseTypes, property.Name, XmlIgnoreAttribure.Instance);
                    continue;
                }

                if ((property.PropertyType.IsClass) && (property.PropertyType != typeof(String)) && (property.PropertyType != typeof(DateTime)))
                {
                    if (overrides[type, property.Name] != null) continue;

                    /* Можем сохранять и внутренние объекты 
                    var innerObj = property.GetValue(obj);
                    if (innerObj != null)
                    {
                        var innerObjAtts = new XmlAttributes();
                        innerObjAtts.XmlElements.Add(new XmlElementAttribute(innerObj.GetType()));
                        SetAttribute(overrides, type, baseType, property.Name, innerObjAtts);

                        SetIgnoreAttributes(innerObj, overrides);
                    }
                    else*/
                    SetAttribute(overrides, type, baseTypes, property.Name, XmlIgnoreAttribure.Instance);
                }
            }
        }

        private static void SetAttribute(XmlAttributeOverrides overrides, Type type, IEnumerable<Type> baseTypes, string propertyName, XmlAttributes attributes)
        {
            overrides.Add(type, propertyName, attributes);
            foreach (var baseType in baseTypes)
            {
                overrides.Add(baseType, propertyName, attributes);
            }
        }
    }
}
