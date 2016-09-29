using System.Linq;
using System.Web.Mvc;

namespace BaseCms.Common.Binders
{
    public class JDataTableModelBinder : IModelBinder
    {
        private const string MDataPropKey = "mDataProp_";
        private const string SSearchKey = "sSearch_";
        private const string BSearchableKey = "bSearchable_";
        private const string BRegexKey = "bRegex_";
        private const string SortColKey = "iSortCol_";
        private const string SSortDir = "sSortDir_";
        private const string BSortableKey = "bSortable_";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var jDataTableSettings = new JDataTableSettings();

            var requestParams = (controllerContext.RequestContext.HttpContext).Request.Params;

            jDataTableSettings.Echo = requestParams["sEcho"];
            jDataTableSettings.ColumnsStr = requestParams["sColumns"];
            jDataTableSettings.DisplayStart = int.Parse(requestParams["iDisplayStart"]);
            jDataTableSettings.DisplayLength = int.Parse(requestParams["iDisplayLength"]);
            jDataTableSettings.Search = requestParams["sSearch"];
            jDataTableSettings.Regex = bool.Parse(requestParams["bRegex"]);

            int columnLength = int.Parse(requestParams["iColumns"]);
            jDataTableSettings.DataProperties = new int[columnLength];
            jDataTableSettings.ColumnSearch = new string[columnLength];
            jDataTableSettings.ColumnSearchable = new bool[columnLength];
            jDataTableSettings.ColumnRegex = new bool[columnLength];
            jDataTableSettings.ColumnSortable = new bool[columnLength];

            int sortingColumnsLength = 0;
            if (requestParams.AllKeys.Any(k => k.Equals("iSortingCols")))
                sortingColumnsLength = int.Parse(requestParams["iSortingCols"]); //for new jDataTable version
            if (requestParams.AllKeys.Any(k => k.Equals("IEntitySortedCols")))
                sortingColumnsLength = int.Parse(requestParams["IEntitySortedCols"]);
            jDataTableSettings.SortIndexes = new int[sortingColumnsLength];
            jDataTableSettings.SortDirections = new string[sortingColumnsLength];

            foreach (var param in requestParams.Keys)
            {
                string paramName = param.ToString();

                if (paramName.StartsWith(MDataPropKey))
                {
                    // Находим индекс столбца из его имени, например получаем индекс 0 из имени iSortCol_0
                    int index = int.Parse(paramName.Remove(0, MDataPropKey.Length));

                    jDataTableSettings.DataProperties[index] = int.Parse(requestParams.GetValues(paramName)[0]);
                }

                if (paramName.StartsWith(SSearchKey))
                {
                    // Находим индекс столбца из его имени, например получаем индекс 0 из имени iSortCol_0
                    int index = int.Parse(paramName.Remove(0, SSearchKey.Length));

                    jDataTableSettings.ColumnSearch[index] = requestParams.GetValues(paramName)[0];
                }

                if (paramName.StartsWith(BSearchableKey))
                {
                    // Находим индекс столбца из его имени, например получаем индекс 0 из имени iSortCol_0
                    int index = int.Parse(paramName.Remove(0, BSearchableKey.Length));

                    jDataTableSettings.ColumnSearchable[index] = bool.Parse(requestParams.GetValues(paramName)[0]);
                }

                if (paramName.StartsWith(BRegexKey))
                {
                    // Находим индекс столбца из его имени, например получаем индекс 0 из имени iSortCol_0
                    int index = int.Parse(paramName.Remove(0, BRegexKey.Length));

                    jDataTableSettings.ColumnRegex[index] = bool.Parse(requestParams.GetValues(paramName)[0]);
                }

                if (paramName.StartsWith(SortColKey))
                {
                    // Находим индекс столбца из его имени, например получаем индекс 0 из имени iSortCol_0
                    int index = int.Parse(paramName.Remove(0, SortColKey.Length));
                    var paramValue = requestParams.GetValues(paramName)[0];
                    var val = 0;
                    if (int.TryParse(paramValue, out val))
                    {
                        jDataTableSettings.SortIndexes[index] = val;
                    }
                }

                if (paramName.StartsWith(SSortDir))
                {
                    // Находим индекс столбца из его имени, например получаем индекс 0 из имени sSortDir_0
                    int index = int.Parse(paramName.Remove(0, SSortDir.Length));

                    jDataTableSettings.SortDirections[index] = requestParams.GetValues(paramName)[0];
                }

                if (paramName.StartsWith(BSortableKey))
                {
                    // Находим индекс столбца из его имени, например получаем индекс 0 из имени iSortCol_0
                    int index = int.Parse(paramName.Remove(0, BSortableKey.Length));

                    jDataTableSettings.ColumnSearchable[index] = bool.Parse(requestParams.GetValues(paramName)[0]);
                }
            }

            return jDataTableSettings;
        }
    }
}
