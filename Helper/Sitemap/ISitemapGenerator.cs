using System.Collections.Generic;
using System.Xml.Linq;

namespace Helper.Sitemap
{
    public interface ISitemapGenerator
    {
        XDocument GenerateSiteMap(IEnumerable<ISitemapItem> items);
    }
}
