using System.Configuration;

namespace BaseCms.Configuration
{
    public class QuerySetterAssemblyElementSection : ConfigurationSection
    {
        QuerySetterAssemblyElement _element;
        public QuerySetterAssemblyElementSection()
        {
            _element = new QuerySetterAssemblyElement();
        }

        [ConfigurationProperty("elements", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(QuerySetterAssemblyElementCollection), AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public QuerySetterAssemblyElementCollection Elements
        {
            get
            {
                return (QuerySetterAssemblyElementCollection)base["elements"];
            }
        }
    }
}
