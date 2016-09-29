using System.Web.Mvc;

namespace BaseCms.ViewsEngine
{
    public class BaseCmsViewEngine: RazorViewEngine
    {
        public BaseCmsViewEngine()
        {
            var baselocations = ViewLocationFormats;

            var overrideBaseLocations = new string[baselocations.Length + 2];

            baselocations.CopyTo(overrideBaseLocations, 0);
            
            overrideBaseLocations[baselocations.Length] = "~/BaseCms/Views/{1}/{0}.cshtml";
            overrideBaseLocations[baselocations.Length + 1] = "~/BaseCms/Views/Shared/{0}.cshtml";

            ViewLocationFormats = overrideBaseLocations;
            PartialViewLocationFormats = overrideBaseLocations;
        }
    }
}
