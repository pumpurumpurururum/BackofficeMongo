using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using BaseCms.Common.Attributes;
using BaseCms.DependencyResolution;
using BaseCms.Security.Metadata;

namespace BaseCms.Security
{
    [CmsMetadataType(typeof(UserMetadata), CollectionTitleSingular = "User", CollectionTitlePlural = "Users")]
    public class User
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }

        [IgnoreDataMember]
        public bool ChangePassword { get; set; }

        [IgnoreDataMember]
        public IEnumerable<UserToRole> UserToRoles
        {
            get { return IoC.SecurityProvider.Model.UserToRoles.Where(p => p.UserId == Id); }
        }

        [IgnoreDataMember]
        public string Password
        {
            get { return String.Empty; }
            set
            {
                if ((ChangePassword) || (Id == Guid.Empty))
                {
                    Salt = IoC.SecurityProvider.GenerateSalt();
                    PasswordHash = IoC.SecurityProvider.GeneratePasswordHash(value, Salt);
                }
            }
        }
    }
}
