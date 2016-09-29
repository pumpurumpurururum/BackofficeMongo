using System;
using BaseCms.CRUDRepository.Core.Intefaces;

namespace BaseCms.CRUDRepository.Core
{
    public abstract class ContextQueryInitializerBase<TContext> : QueryInitializerBase
    {
        protected abstract TContext GetPrimaryContext();
        protected abstract TContext GetSecondaryIndependentContext();

        public void Register<T>(Func<ContextEnvelope<TContext, T>, object> behavior)
            where T : class, IQuery
        {
            Func<T, object> func = (inp) => behavior(new ContextEnvelope<TContext, T>(GetPrimaryContext(), inp));
            QueryResolver.Register(CollectionName, func);
        }

        public void Register<T>(Action<ContextEnvelope<TContext, T>> behavior)
            where T : class, IQuery
        {
            Action<T> action = (inp) => behavior(new ContextEnvelope<TContext, T>(GetPrimaryContext(), inp));
            QueryResolver.Register(CollectionName, action);
        }

        public void RegisterWithAddIndependentContext<T>(Action<DoubleContextEnvelope<TContext, T>> behavior)
            where T : class, IQuery
        {
            Action<T> action = inp => behavior(new DoubleContextEnvelope<TContext, T>(GetPrimaryContext(), GetSecondaryIndependentContext(), inp));
            QueryResolver.Register(CollectionName, action);
        }

        public void RegisterWithIndependentContext<T>(Action<ContextEnvelope<TContext, T>> behavior)
            where T : class, IQuery
        {
            Action<T> action = inp => behavior(new ContextEnvelope<TContext, T>(GetSecondaryIndependentContext(), inp));
            QueryResolver.Register(CollectionName, action);
        }
    }
}
