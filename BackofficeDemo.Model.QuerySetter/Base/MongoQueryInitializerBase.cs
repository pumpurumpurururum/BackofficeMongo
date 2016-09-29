using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using BaseCms.CRUDRepository;
using BaseCms.CRUDRepository.Core;
using BaseCms.CRUDRepository.Core.Intefaces;
using BaseCms.DependencyResolution;
using BaseCms.Helpers;
using BaseCms.Model.Interfaces;
using BackofficeDemo.MongoBase;
using BackofficeDemo.Repository.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BackofficeDemo.Model.QuerySetter.Base
{
    public class MongoQueryInitializerBase<TEntity> : QueryInitializerBase where TEntity : IEntity<Guid>
    {
        public override string CollectionName => typeof(TEntity).Name;

        protected readonly IRepository<TEntity> BaseContext = IoC.Container.GetInstance<IRepository<TEntity>>();

        protected virtual IQueryable<TEntity> GetDataContext(string detailViewGuid, int? upperIdentifier = null)
        {
            if (String.IsNullOrEmpty(detailViewGuid)) return null;

            var sessionData = SessionDataContainer.GetData(detailViewGuid + CollectionName);
            if (sessionData == null)
            {
                if (!upperIdentifier.HasValue) return null;
                var l = QueryResolver.Execute(
                    new GetListQueryBase(0, int.MaxValue, "Id asc", new List<string> { upperIdentifier.ToString() }),
                    CollectionName).ToList();

                SessionDataContainer.SetData(detailViewGuid + CollectionName, l);
                sessionData = l;
            }
            return sessionData.OfType<TEntity>().AsQueryable();
        }

        protected virtual IAlias GetByAlias(string alias, Guid curId)
        {
            var filterAlias = Builders<TEntity>.Filter.And(
                new BsonDocumentFilterDefinition<TEntity>(new BsonDocument(new BsonElement("Alias", alias))),
                new ExpressionFilterDefinition<TEntity>(p => p.Id != curId));
            

            return
                (IAlias)
                    Task.Run(
                        () =>
                            BaseContext.GetFirstByFilter(filterAlias)).Result;
            //new ExpressionFilterDefinition<TEntity>(p => ((IAlias)p).Alias == alias))).Result;
        }

        private String ReplaceSymbols(string str)
        {
            var mass = new[] { "!", "\"", "#", "%", "&", "'", "*", ",", ".", ";", ":", "<", ">", "=", "?", "[", "]", "^", "`", "{", "}", "|", " " };
            str = mass.Aggregate(str, (current, s) => current.Replace(s, "_"));
            return str;

        }

        protected virtual void ValidateAlias(IAlias obj)
        {
            if (String.IsNullOrEmpty(obj.Alias))
            {
                var newalias = Transliteration.Transform(obj.Name);
                if (string.IsNullOrEmpty(newalias))
                {
                    newalias = obj.Id.ToString();
                }
                if (!String.IsNullOrEmpty(newalias))
                {
                    if (newalias.Length > 50)
                    {
                        newalias = newalias.Substring(0, 49);
                    }
                }
                else
                {
                    newalias = obj.Id.ToString();
                }
                obj.Alias = ReplaceSymbols(newalias);
                
            }

            var existItem = GetByAlias(obj.Alias, obj.Id);
            if (existItem != null)
            {
                var id = obj.Id.ToString();
                obj.Alias = (obj.Alias.Length + id.Length) > 50
                                ? obj.Alias.Substring(0, obj.Alias.Length - (1 + id.Length)) + id
                                : obj.Alias + id;
                obj.Alias = ReplaceSymbols(obj.Alias);

              
            }
            BaseContext.Update((TEntity)obj);
        }
        public void Register<T>(Func<BaseEnvelope<T>, object> behavior)
            where T : class, IQuery
        {
            Func<T, object> func = (inp) => behavior(new BaseEnvelope<T>(inp));
            QueryResolver.Register(CollectionName, func);
        }

        public void Register<T>(Action<BaseEnvelope<T>> behavior)
            where T : class, IQuery
        {
            Action<T> action = (inp) => behavior(new BaseEnvelope<T>(inp));
            QueryResolver.Register(CollectionName, action);
        }




        public virtual IQueryable<TEntity> GetResultByFilters(List<string> filters)
        {
            
            return BaseContext;
        }


        protected override void Do()
        {
            Register<GetTypeQueryBase>(t => typeof(TEntity));

            Register<GetIdentifierQueryBase>(
                inp => ((TEntity)inp.Input.Object).Id.ToString());

            Register<GetByIdQueryBase>(inp =>
            {
                var id = Guid.Parse(inp.Input.Id);
                var item = BaseContext.GetById(id);
                inp.Input.Output = item;
            });

            //Register<GetByIdsQueryBase>(inp =>
            //{
            //    var ids = inp.Input.Ids.Select(Guid.Parse).ToArray();
            //    var objs = BaseContext.Where(n => ids.Contains(n.Id)).ToList();
            //    inp.Input.Output = objs;
            //});


            Register<GetListCountQueryBase>(inp => GetResultByFilters(inp.Input.Filters).ToList().Count);



            Register<DeleteObject>(inp =>
            {
                var id = Guid.Parse(inp.Input.Id);
               Task.Run(() => BaseContext.Delete(id));

            });


            Register<InsertObject>(inp =>
            {
                var obj = (TEntity)inp.Input.Object;
                
                var ret = BaseContext.Add(obj);



                if (typeof (TEntity).GetInterfaces().Contains(typeof (IAlias)))
                {
                    ValidateAlias((IAlias)ret);
                }
                inp.Input.Output = ret.Id.ToString();
            });

            Register<UpdateObject>(inp =>
            {
                var obj = (TEntity)inp.Input.Object;
                
                BaseContext.Update(obj);
                if (typeof(TEntity).GetInterfaces().Contains(typeof(IAlias)))
                {
                    ValidateAlias((IAlias)obj);
                }
            });
            Register<GetListQueryBase>(inp =>
            {

                var items = GetResultByFilters(inp.Input.Filters)
                    .OrderBy(inp.Input.SortBy)
                    .Skip(inp.Input.Index * inp.Input.PageSize)
                    .Take(inp.Input.PageSize)
                    .ToList();

                return items;
            });

            

        }
    }
}
