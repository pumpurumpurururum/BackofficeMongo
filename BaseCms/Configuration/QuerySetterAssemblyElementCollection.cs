using System.Configuration;

namespace BaseCms.Configuration
{
    public class QuerySetterAssemblyElementCollection : ConfigurationElementCollection
    {
        public QuerySetterAssemblyElementCollection()
        {
            var myElement = (QuerySetterAssemblyElement)CreateNewElement();
            Add(myElement);
        }

        public void Add(QuerySetterAssemblyElement customElement)
        {
            BaseAdd(customElement);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            base.BaseAdd(element, false);
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new QuerySetterAssemblyElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((QuerySetterAssemblyElement)element).Name;
        }

        public QuerySetterAssemblyElement this[int Index]
        {
            get
            {
                return (QuerySetterAssemblyElement)BaseGet(Index);
            }
            set
            {
                if (BaseGet(Index) != null)
                {
                    BaseRemoveAt(Index);
                }
                BaseAdd(Index, value);
            }
        }

        new public QuerySetterAssemblyElement this[string Name]
        {
            get
            {
                return (QuerySetterAssemblyElement)BaseGet(Name);
            }
        }

        public int indexof(QuerySetterAssemblyElement element)
        {
            return BaseIndexOf(element);
        }

        public void Remove(QuerySetterAssemblyElement url)
        {
            if (BaseIndexOf(url) >= 0)
                BaseRemove(url.Name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }
    }
}
