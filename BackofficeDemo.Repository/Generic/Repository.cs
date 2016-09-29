using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BackofficeDemo.MongoBase;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BackofficeDemo.Repository.Generic
{
    public class Repository<T> : IRepository<T>
        where T : IEntity<Guid>
    {

        protected internal IMongoCollection<T> Collection;

        public Repository()
            : this(Util<Guid>.GetDefaultConnectionString())
        {
        }

        public Repository(string connectionString)
        {
            Collection = Util<Guid>.GetCollectionFromConnectionString<T>(connectionString);
        }

        public Repository(string connectionString, string collectionName)
        {
            Collection = Util<Guid>.GetCollectionFromConnectionString<T>(connectionString, collectionName);
        }

        public Repository(MongoUrl url)
        {
            Collection = Util<Guid>.GetCollectionFromUrl<T>(url);
        }

        public Repository(MongoUrl url, string collectionName)
        {
            Collection = Util<Guid>.GetCollectionFromUrl<T>(url, collectionName);
        }

        public T GetById(Guid id)
        {
            return Task.Run(()=>Collection.Find(Builders<T>.Filter.Where(x => x.Id == id)).FirstOrDefaultAsync()).GetAwaiter().GetResult();
            
        }

        public T GetFirstByFilter(FilterDefinition<T> filter)
        {

            
            
            return Task.Run(()=> Collection.Find(filter).FirstOrDefaultAsync()).Result;
        }

        public List<T> GetByFilter(FilterDefinition<T> filter)
        {
            filter = filter ?? new BsonDocument();
            return Task.Run(() => Collection.Find(filter).ToListAsync()).Result;
        }


        public virtual List<T> GetAll()
        {
            return Task.Run(() => Collection.Find(new BsonDocument()).ToListAsync()).Result;
        }

        public virtual T Add(T entity)
        {

            var curuser = System.Web.HttpContext.Current?.User.Identity;
            if (curuser != null)
            {
                entity.CreatedOn = DateTime.UtcNow;
                entity.UpdatedOn = DateTime.UtcNow;
                if (curuser.IsAuthenticated)
                {
                    entity.CreatedBy = curuser.Name;
                    entity.UpdatedBy = curuser.Name;
                }
                else
                {
                    entity.CreatedBy = "Guest";
                    entity.UpdatedBy = "Guest";
                }
            }
            Task.Run(() => Collection.InsertOneAsync(entity)).Wait();

            return entity;
        }

        public virtual IEnumerable<T> Add(IEnumerable<T> entities)
        {
            
            Task.Run(()=>Collection.InsertManyAsync(entities));
            return entities;
        }

        public virtual void Update(T entity)
        {
            var curuser = System.Web.HttpContext.Current?.User.Identity;

            if (curuser != null)
            {
                entity.UpdatedOn = DateTime.UtcNow;
                entity.UpdatedBy = curuser.IsAuthenticated ? curuser.Name : "Guest";
            }
            var filter = Builders<T>.Filter.Eq(s => s.Id, entity.Id);
            Task.Run(()=>Collection.ReplaceOneAsync(filter, entity)).Wait();
        }

        public virtual void Update(IEnumerable<T> entities)
        {
            Task.Run(() => Collection.InsertManyAsync(entities)).Wait();
            
        }

        public virtual void Delete(Guid id)
        {
            Task.Run(() => Collection.DeleteOneAsync(Builders<T>.Filter.Where(x => x.Id == id))).Wait();
        }

        public virtual void Delete(T entity)
        {
            Task.Run(()=>Delete(entity.Id)).Wait();
        }

        public virtual void Delete(Expression<Func<T, bool>> predicate)
        {
            Task.Run(() => Collection.DeleteManyAsync(predicate)).Wait();
        }


        public virtual long Count()
        {
            return Task.Run(() => Collection.CountAsync(p => true)).Result;
        }

       
        public virtual bool Exists(Expression<Func<T, bool>> predicate)
        {
            return Collection.AsQueryable().Any(predicate);
        }

        public virtual IEnumerator<T> GetEnumerator()
        {
            return Collection.AsQueryable().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Collection.AsQueryable().GetEnumerator();
        }

        public virtual Type ElementType => Collection.AsQueryable().ElementType;

        public virtual Expression Expression => Collection.AsQueryable().Expression;


        public virtual IQueryProvider Provider => Collection.AsQueryable().Provider;

    }

    
}
