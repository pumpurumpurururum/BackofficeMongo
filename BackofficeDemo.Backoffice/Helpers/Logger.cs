using System;
using System.Collections.Generic;
using System.Linq;
using BaseCms.Logging;
using BaseCms.Logging.Interfaces;
using BackofficeDemo.Repository.Generic;

namespace BackofficeDemo.Backoffice.Helpers
{
    public class Logger : ILogger
    {
        private static readonly Repository<Audit> Context = new Repository<Audit>();

        public void Log(string userId, string userName, DateTime date, string collectionName, string objectId,
                        string data)
        {
            Context.Add(new Audit
            {
                CollectionName = collectionName,
                CommandData = data,
                Date = date,
                ObjectId = objectId,
                UserId = userId,
                UserName = userName
            });
            
        }

        public IEnumerable<Audit> GetHistory(string collectionName, string objectId)
        {
            return Context.Where(a => a.CollectionName == collectionName && a.ObjectId == objectId);
        }

        public IEnumerable<Audit> GetHistory(string collectionName, string[] objectId)
        {
            return Context.Where(a => a.CollectionName == collectionName && objectId.Contains(a.ObjectId));
        }
    }
}
