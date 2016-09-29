using System.Collections.Generic;

namespace BaseCms.Model
{
    public class Item
    {
        public string Id { get; set; }
        public Dictionary<string, object> Properties { get; set; }
    }
}
