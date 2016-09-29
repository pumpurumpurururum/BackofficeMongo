using System.Collections.Generic;
using System.Web;

namespace BaseCms.CRUDRepository
{
    public static class SessionDataContainer
    {
        public static List<object> GetData(string sessionKey)
        {
            return HttpContext.Current.Session[sessionKey] as List<object>;
        }

        public static void SetData(string sessionKey, List<object> objects)
        {
            HttpContext.Current.Session[sessionKey] = objects;
        }

        public static void ClearData(string sessionKey)
        {
            HttpContext.Current.Session[sessionKey] = null;
        }
    }
}
