using System;
using System.Collections.Generic;

namespace BaseCms.Logging.Interfaces
{
    public interface ILogger
    {
        void Log(string userId, string userName, DateTime date, string collectionName, string objectId, string data);

        IEnumerable<Audit> GetHistory(string collectionName, string objectId);

        IEnumerable<Audit> GetHistory(string collectionName, string[] objectId);
    }
}
