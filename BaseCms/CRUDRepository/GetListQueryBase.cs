using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCms.CRUDRepository.Core;

namespace BaseCms.CRUDRepository
{
    public class GetListQueryBase : QueryBase<IEnumerable<object>>
    {
        public GetListQueryBase(int index, int pageSize, string sortBy, List<string> filters)
        {
            Index = index;
            PageSize = pageSize;
            SortBy = sortBy;
            Filters = filters;
            FilterContext = null;
            DetailViewGuid = null;
        }

        public GetListQueryBase(int index, int pageSize, string sortBy, List<string> filters, string detailViewGuid)
        {
            Index = index;
            PageSize = pageSize;
            SortBy = sortBy;
            Filters = filters;
            FilterContext = null;
            DetailViewGuid = detailViewGuid;
        }

        public GetListQueryBase(int index, int pageSize, string sortBy, List<string> filters, string filterContext, string detailViewGuid)
        {
            Index = index;
            PageSize = pageSize;
            SortBy = sortBy;
            Filters = filters;
            FilterContext = filterContext;
            DetailViewGuid = detailViewGuid;
        }

        public int Index { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }

        public List<string> Filters { get; set; }
        public string FilterContext { get; set; }

        public string DetailViewGuid { get; set; }
    }
}
