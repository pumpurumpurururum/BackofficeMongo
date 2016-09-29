using System.Runtime.Serialization;
using BaseCms.Common.Attributes;
using BaseCms.Security.Metadata;

namespace BaseCms.Security
{
    [KnownType(typeof(GetListPermission))]
    [KnownType(typeof(InsertObjectPermission))]
    [KnownType(typeof(UpdateObjectPermission))]
    [KnownType(typeof(DeleteObjectPermission))]
    [KnownType(typeof(ChangeObjectStateIdPermission))]
    [KnownType(typeof(ExternalSystemAccessPermission))]
    [CmsMetadataType(typeof(PermissionMetadata), ForbidReadMode = true)]
    public abstract class Permission
    {
        public bool Grant { get; set; }
        public bool WithGrant { get; set; }
        public bool Deny { get; set; }

        [IgnoreDataMember]
        public abstract string Description { get; }
        [IgnoreDataMember]
        public abstract string FullDescription { get; }
    }
}
