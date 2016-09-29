using System.Configuration;

namespace BaseCms.Configuration
{
    public class QuerySetterAssemblyElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("assembly", IsRequired = true)]
        public string Assembly
        {
            get { return (string)this["assembly"]; }
            set { this["assembly"] = value; }
        }

        [ConfigurationProperty("shouldrun", IsRequired = true)]
        public bool ShouldRun
        {
            get { return (bool)this["shouldrun"]; }
            set { this["shouldrun"] = value; }
        }
    }
}
