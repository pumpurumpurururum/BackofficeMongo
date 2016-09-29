﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using BaseCms.CRUDRepository;
using BackofficeDemo.Model.QuerySetter.Base;

namespace BackofficeDemo.Model.QuerySetter
{
    public class CityQuerySetter : MongoQueryInitializerBase<City>
    {
        protected override void Do()
        {
            base.Do();

            Register<GetListQueryBase>(inp =>
            {

                var items = GetResultByFilters(inp.Input.Filters)
                    .OrderBy(inp.Input.SortBy)
                    .Skip(inp.Input.Index * inp.Input.PageSize)
                    .Take(inp.Input.PageSize)
                    .ToList();

                return items;
            });

            Register<InsertObject>(inp =>
            {
                //var id = inp.UpperIdentifier;

                var obj = (City)inp.Input.Object;

                var ret = BaseContext.Add(obj);
                
                inp.Input.Output = ret.Id.ToString();
            });

            Register<LookupListQueryBase>(inp =>
            {
                var query = BaseContext;
                var str = inp.Input.Search ?? "";
                inp.Input.Output = query
                    .Where(p => string.IsNullOrEmpty(str) || p.LocationName.StartsWith(str))
                    .Take(inp.Input.Max)
                    .OrderBy(p => p.LocationName).AsEnumerable()
                    .Select(
                        p =>
                        new LookupItem
                        {
                            Id = p.Id.ToString(),
                            Name = p.Name
                        })
                    .ToList();
            });

            Register<LookupGetItemQueryBase>(inp =>
            {
                var parsedId = Guid.Parse(inp.Input.Id);
                inp.Input.Output =
                    BaseContext.Where(p => p.Id == parsedId).AsEnumerable()
                       .Select(
                           p =>
                           new LookupItem { Id = p.Id.ToString(), Name = p.Name })
                       .FirstOrDefault();
            });
        }

        
    }
}
