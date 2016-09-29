using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using BaseCms.CRUDRepository.Serialization;
using BaseCms.Common.Attributes;
using BaseCms.Security.Metadata;

namespace BaseCms.Security
{
    [XmlInclude(typeof(GetListPermission))]
    [XmlInclude(typeof(InsertObjectPermission))]
    [XmlInclude(typeof(UpdateObjectPermission))]
    [XmlInclude(typeof(DeleteObjectPermission))]
    [XmlInclude(typeof(ChangeObjectStateIdPermission))]
    [XmlInclude(typeof(ExternalSystemAccessPermission))]
    [CmsMetadataType(typeof(RoleMetadata), CustomEditView = "RoleEdit", CollectionTitleSingular = "Role", CollectionTitlePlural = "Roles")]
    public class Role : XmlSerializableArgumentBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<Permission> Permissions { get; set; }

        public Role()
        {
            Permissions = new List<Permission>();
        }

        [IgnoreDataMember]
        public IEnumerable<Permission> InsertObjectPermissions { get; set; }

        [IgnoreDataMember]
        public IEnumerable<Permission> UpdateObjectPermissions { get; set; }

        [IgnoreDataMember]
        public IEnumerable<Permission> DeleteObjectPermissions { get; set; }

        [IgnoreDataMember]
        public IEnumerable<Permission> ChangeObjectStateIdPermissions { get; set; }

        [IgnoreDataMember]
        public IEnumerable<Permission> ExternalSystemAccessPermissions { get; set; }
    }
}
