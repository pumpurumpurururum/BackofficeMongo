using System;
using System.Collections.Generic;
using System.Linq;
using BackofficeDemo.Model.QuerySetter.Base;

namespace BackofficeDemo.Model.QuerySetter
{
    public class PartnerUserQuerySetter : MongoQueryInitializerBase<PartnerUser>
    {
        public override IQueryable<PartnerUser> GetResultByFilters(List<string> filters)
        {
            if (filters.Count > 0)
            {
                var partnerId = Guid.Empty;
                if (Guid.TryParse(filters[0], out partnerId) && partnerId != Guid.Empty)
                {

                    return BaseContext.Where(p => p.PartnerGuid == partnerId);
                }
            }

            return BaseContext;
        }
    }
}
