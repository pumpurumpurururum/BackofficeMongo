using System;
using BaseCms.CRUDRepository.Core.Intefaces;

namespace BaseCms.CRUDRepository.Core
{
    public abstract class QueryInitializerBase : IQueryInitializer
    {
        public abstract string CollectionName { get; }
        public QueryResolver QueryResolver { get; protected set; }

        public void Init(QueryResolver queryResolver)
        {
            QueryResolver = queryResolver;
            RegisterDefaults();
            Do();
        }

        protected virtual void RegisterDefaults()
        {
            QueryResolver.Register<NewObjectQueryBase>(CollectionName,
                                                       c =>
                                                       Activator.CreateInstance(
                                                           QueryResolver.Execute(new GetTypeQueryBase(), CollectionName)));
        }

        public void Register<T>(Func<T, object> func)
            where T : class,IQuery
        {
            QueryResolver.Register(CollectionName, func);
        }

        public void Register<T>(Action<T> func)
            where T : class,IQuery
        {
            QueryResolver.Register(CollectionName, func);
        }

        protected abstract void Do();

        //partial void PartialDo();
    }
}
