using System.Collections.Generic;
using BaseCms.CRUDRepository.Core;

namespace BaseCms.CRUDRepository
{
    public class GetListCountQueryBase : QueryBase<int>
    {
        public GetListCountQueryBase()
        {
            Filters = new List<string>();
            FilterContext = null;
            DetailViewGuid = null;
        }

        public GetListCountQueryBase(List<string> filters)
        {
            Filters = filters;
            FilterContext = null;
            DetailViewGuid = null;
        }

        public GetListCountQueryBase(List<string> filters, string detailViewGuid)
        {
            Filters = filters;
            FilterContext = null;
            DetailViewGuid = detailViewGuid;
        }

        public GetListCountQueryBase(List<string> filters, string filterContext, string detailViewGuid)
        {
            Filters = filters;
            FilterContext = filterContext;
            DetailViewGuid = detailViewGuid;
        }

        public List<string> Filters { get; set; }
        public string FilterContext { get; set; }
        public string DetailViewGuid { get; set; }
    }
}
