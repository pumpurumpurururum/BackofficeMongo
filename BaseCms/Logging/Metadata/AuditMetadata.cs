using System;
using BaseCms.Views.List.Attributes;

namespace BaseCms.Logging.Metadata
{
    public class AuditMetadata
    {
        [ListMetadata(Display = "Дата", Order = 1, Format = "dd.MM.yyyy HH:mm:ss")]
        public DateTime Date { get; set; }

        [ListMetadata(Display = "Пользователь", Order = 2)]
        public string UserName { get; set; }

        [ListMetadata(Display = "Тип изменения", Order = 3)]
        public string ChangeType { get; set; }
    }
}
