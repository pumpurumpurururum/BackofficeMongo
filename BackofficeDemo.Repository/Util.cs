using System;
using System.Configuration;
using BackofficeDemo.Model.Attributes;
using BackofficeDemo.MongoBase;
using MongoDB.Driver;

namespace BackofficeDemo.Repository
{
    internal static class Util<TU>
    {
        private const string DefaultConnectionstringName = "MongoServerSettings";
        public static string GetDefaultConnectionString()
        {
            return ConfigurationManager.ConnectionStrings[DefaultConnectionstringName].ConnectionString;
        }

        public static IMongoDatabase GetDatabase()
        {
            return GetDatabaseFromUrl(new MongoUrl(GetDefaultConnectionString()));
        }

        private static IMongoDatabase GetDatabaseFromUrl(MongoUrl url)
        {
            var client = new MongoClient(url);
            var server = client.GetDatabase(url.DatabaseName);
            return server; 
        }

        public static IMongoCollection<T> GetCollectionFromConnectionString<T>(string connectionString)
            where T : IEntity<TU>
        {
            return GetCollectionFromConnectionString<T>(connectionString, GetCollectionName<T>());
        }

        public static IMongoCollection<T> GetCollectionFromConnectionString<T>(string connectionString, string collectionName)
            where T : IEntity<TU>
        {
            return GetDatabaseFromUrl(new MongoUrl(connectionString))
                .GetCollection<T>(collectionName);
        }

        public static IMongoCollection<T> GetCollectionFromUrl<T>(MongoUrl url)
            where T : IEntity<TU>
        {
            return GetCollectionFromUrl<T>(url, GetCollectionName<T>());
        }

        public static IMongoCollection<T> GetCollectionFromUrl<T>(MongoUrl url, string collectionName)
            where T : IEntity<TU>
        {
            return GetDatabaseFromUrl(url)
                .GetCollection<T>(collectionName);
        }
        private static string GetCollectionName<T>() where T : IEntity<TU>
        {
            var baseType = typeof(T).BaseType;
            var collectionName = baseType != null && baseType == typeof(object) ? GetCollectioNameFromInterface<T>() : GetCollectionNameFromType(typeof(T));

            if (string.IsNullOrEmpty(collectionName))
            {
                throw new ArgumentException("Collection name cannot be empty for this entity");
            }
            return collectionName;
        }

        private static string GetCollectioNameFromInterface<T>()
        {
            var att = Attribute.GetCustomAttribute(typeof(T), typeof(CollectionName));
            var collectionname = att != null ? ((CollectionName)att).Name : typeof(T).Name;

            return collectionname;
        }

        private static string GetCollectionNameFromType(Type entitytype)
        {
            string collectionname;
            var att = Attribute.GetCustomAttribute(entitytype, typeof(CollectionName));
            if (att != null)
            {
                collectionname = ((CollectionName)att).Name;
            }
            else
            {
                if (typeof(Entity).IsAssignableFrom(entitytype))
                {
                    while (entitytype.BaseType != null && entitytype.BaseType != typeof(Entity))
                    {
                        entitytype = entitytype.BaseType;
                    }
                }
                collectionname = entitytype.Name;
            }

            return collectionname;
        }
    }
}
