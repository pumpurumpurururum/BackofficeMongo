using System;
using System.Collections.Generic;
using BaseCms.CRUDRepository.Core.Intefaces;
using BaseCms.CRUDRepository.Serialization.Interfaces;
using BaseCms.DependencyResolution;
using BaseCms.Logging.Interfaces;
using BaseCms.StateRepository.Base;
using BaseCms.StateRepository.Defaults;

namespace BaseCms.CRUDRepository.Core
{
    public class QueryResolver
    {
        private readonly ILogger _logger = IoC.Container.GetInstance<ILogger>();

        private readonly DefaultState _defaultState = IoC.Container.GetInstance<DefaultState>();

        private readonly Dictionary<QueryObjectKey, Action<IQuery>> _cmdqueries =
            new Dictionary<QueryObjectKey, Action<IQuery>>();


        public TOutput Execute<TOutput>(QueryBase<TOutput> cmdOrQueryBase, string collectionName, StateBase state = null)
        {
            if (state == null) state = _defaultState;
            Action<IQuery> behavior;
            var key = CreateDictionaryKey(cmdOrQueryBase.GetType(), collectionName);
            if (_cmdqueries.TryGetValue(key, out behavior))
            {
                // Смотрим, это команда или запрос
                var command = cmdOrQueryBase as IXmlSerializable;

                // Если это команда, проверяем возможность её выполнения у текущего состояния,
                // в противном случае проверяем у SecurityProvider
                var canExecute = command == null
                                     ? IoC.SecurityProvider.CheckPermission(key.ObjectType, key.CollectionName)
                                     : state.CheckPermission(cmdOrQueryBase, key.CollectionName, command.GetObjectId(key.CollectionName));

                if (!canExecute) throw new Exception("У Вас нет прав на выполнение этой команды");

                behavior(cmdOrQueryBase);

                // Если в команде определены правила сериализации, её можно логировать
                Log(collectionName, command);

                return cmdOrQueryBase.Output;
            }
            throw new Exception("Cannot find command for : " + key);
        }

        /// <summary>
        /// Проверяем определён ли тип запроса в коллекции
        /// </summary>
        /// <param name="queryType">Тип запроса</param>
        /// <param name="collectionName">Имя коллекции</param>
        /// <returns>Да/нет</returns>
        public bool ContainsQuery(Type queryType, string collectionName)
        {
            var key = CreateDictionaryKey(queryType, collectionName);
            return _cmdqueries.ContainsKey(key);
        }

        public void Register<T>(string collectionName, Action<T> behavior) where T : class,IQuery
        {
            var key = CreateDictionaryKey<T>(collectionName);

            if (_cmdqueries.ContainsKey(key))
                _cmdqueries.Remove(key);

            _cmdqueries.Add(key, o =>
            {
                var typedObject = (T)o;
                if (typedObject == null)
                    throw new InvalidCastException();
                behavior(typedObject);
            });
        }

        public void Register<T>(string collectionName, Func<T, object> behavior) where T : class,IQuery
        {
            var key = CreateDictionaryKey<T>(collectionName);

            if (_cmdqueries.ContainsKey(key))
                _cmdqueries.Remove(key);

            _cmdqueries.Add(key, o =>
            {
                var typedObject = (T)o;
                if (typedObject == null)
                    throw new InvalidCastException();
                typedObject.InnerOutput = behavior(typedObject);
            });
        }

        public IEnumerable<QueryObjectKey> GetAllKeys()
        {
            return _cmdqueries.Keys;
        }

        private static QueryObjectKey CreateDictionaryKey<T>(string collection)
        {
            return CreateDictionaryKey(typeof(T), collection);
        }

        private static QueryObjectKey CreateDictionaryKey(Type type, string collection)
        {
            var key = new QueryObjectKey(collection, type);
            return key;
        }

        private void Log(string collectionName, IXmlSerializable cmdOrQueryBase)
        {
            if ((cmdOrQueryBase == null) || (!cmdOrQueryBase.NeedToLog())) return;
            var userId = IoC.SecurityProvider.CurrentUser.Id.ToString();
            var userName = IoC.SecurityProvider.CurrentUser.Login;
            var objectId = cmdOrQueryBase.GetObjectId(collectionName);
            var data = IoC.Container.GetInstance<ISerializer>().Serialize(cmdOrQueryBase);
            _logger.Log(userId, userName, DateTime.Now, collectionName, objectId, data);
        }
    }
}
