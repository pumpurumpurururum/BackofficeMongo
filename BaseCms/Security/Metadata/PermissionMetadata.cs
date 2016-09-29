using System;
using BaseCms.Views.Detail.Attributes;
using BaseCms.Views.List.Attributes;

namespace BaseCms.Security.Metadata
{
    public class PermissionMetadata
    {

        [ListMetadata(Display = "Идентификатор", Order = 10)]
        [DetailMetadata(IsHidden = true)]
        public Guid Id { get; set; }

        [ListMetadata(Display = "Разрешить", Order = 100)]
        [DetailMetadata(Display = "Разрешить", Order = 100)]
        public bool Grant { get; set; }


        [ListMetadata(Display = "Запретить", Order = 100)]
        [DetailMetadata(Display = "Запретить", Order = 100)]
        public bool Deny { get; set; }


    }
}
