using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BackofficeDemo.MongoBase;
using MongoDB.Driver;

namespace BackofficeDemo.Repository.Generic
{
    public interface IRepository<T> : IQueryable<T>
        where T : IEntity<Guid>
    {
        //IMongoCollection<T> Collection { get; }

        List<T> GetByFilter(FilterDefinition<T> filter);

        List<T> GetAll();

        T GetById(Guid id);


        T GetFirstByFilter(FilterDefinition<T> filter);

        T Add(T entity);

        IEnumerable<T> Add(IEnumerable<T> entities);

        void Update(T entity);

        void Update(IEnumerable<T> entities);

        void Delete(Guid id);

        void Delete(T entity);

        void Delete(Expression<Func<T, bool>> predicate);


        long Count();

        bool Exists(Expression<Func<T, bool>> predicate);

        
    }


}
