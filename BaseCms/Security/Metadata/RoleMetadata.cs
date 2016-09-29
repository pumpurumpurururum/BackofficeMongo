using System;
using System.Collections.Generic;
using BaseCms.Common.Validation.Attributes;
using BaseCms.Views.Detail.Attributes;
using BaseCms.Views.Link.Attributes;
using BaseCms.Views.List.Attributes;

namespace BaseCms.Security.Metadata
{
    public class RoleMetadata
    {
        [ListMetadata(Display = "Id", Order = 10)]
        [DetailMetadata(IsHidden = true, IsKey = true)]
        public Guid Id { get; set; }

        [ListMetadata(Display = "Name", Order = 20)]
        [DetailMetadata(Display = "Name", Order = 20, IsHeaderDisplayProperty = true)]
        [Required]
        public string Name { get; set; }

        [DetailMetadata(Display = "Permissions", Order = 30, IsHidden = false)]
        [DynamicLink("Security_GetListPermission")]
        public IEnumerable<Permission> Permissions { get; set; }

        [DetailMetadata(Display = "", Order = 40, IsHidden = false)]
        [DynamicLink("Security_InsertObjectPermission")]
        public IEnumerable<Permission> InsertObjectPermissions { get; set; }

        [DetailMetadata(Display = "", Order = 50, IsHidden = false)]
        [DynamicLink("Security_UpdateObjectPermission")]
        public IEnumerable<Permission> UpdateObjectPermissions { get; set; }

        [DetailMetadata(Display = "", Order = 60, IsHidden = false)]
        [DynamicLink("Security_DeleteObjectPermission")]
        public IEnumerable<Permission> DeleteObjectPermissions { get; set; }

        [DetailMetadata(Display = "", Order = 70, IsHidden = false)]
        [DynamicLink("Security_ChangeObjectStateIdPermission")]
        public IEnumerable<Permission> ChangeObjectStateIdPermissions { get; set; }

        [DetailMetadata(Display = "", Order = 70, IsHidden = false)]
        [DynamicLink("Security_ExternalSystemAccessPermission")]
        public IEnumerable<Permission> ExternalSystemAccessPermissions { get; set; }
    }
}
