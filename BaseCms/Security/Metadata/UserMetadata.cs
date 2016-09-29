using System;
using System.Collections.Generic;
using BaseCms.Common.Validation.Attributes;
using BaseCms.Views.Detail.Attributes;
using BaseCms.Views.Link.Attributes;
using BaseCms.Views.List.Attributes;

namespace BaseCms.Security.Metadata
{
    public class UserMetadata
    {

        [ListMetadata(Display = "Id", Order = 10)]
        [DetailMetadata(Display = "Id", IsHidden = true, IsKey = true)]
        public Guid Id { get; set; }

        [ListMetadata(Display = "Login", Order = 20)]
        [DetailMetadata(Display = "Login", Order = 20, IsHeaderDisplayProperty = true)]
        [Required(Order = 0)]
        [Unique("Security_User", "Login", Order = 1)]
        public string Login { get; set; }

        [DetailMetadata(Order = 25)]
        [Required(Order = 0)]
        [Unique("Security_User", "Email", Order = 1)]
        public string Email { get; set; }

        [DetailMetadata(IsHidden = true)]
        public string PasswordHash { get; set; }

        [DetailMetadata(IsHidden = true)]
        public string Salt { get; set; }

        [DetailMetadata(Display = "Roles", Order = 30, IsHidden = false)]
        [DynamicLink("Security_UserToRole")]
        public IEnumerable<UserToRole> UserToRoles { get; set; }

        [DetailMetadata(Display = "Password", IsPassword = true, Order = 40)]
        [CustomJavaScriptFunction("checkPasswordIsNecessary", "Must enter your password")]
        public string Password { get; set; }

        [DetailMetadata(Display = "Change password", IsPassword = true, Order = 50)]
        public bool ChangePassword { get; set; }
    }
}
