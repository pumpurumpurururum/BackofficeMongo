using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BaseCms.Common.Attributes;
using BackofficeDemo.Model.Metadata;
using BackofficeDemo.MongoBase;
using Microsoft.AspNet.Identity;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace BackofficeDemo.Model
{
    [CmsMetadataType(typeof(PartnerUserMetadata),
        CollectionTitleSingular = "User",
        CollectionTitlePlural = "Users",
        CustomEditView = "EditWithTabs",
        TabsWithBlocks = "Main;Roles;Tech Info")]
    public class PartnerUser : Entity, IUser<Guid>, IUser<String>
    {
        public PartnerUser()
        {
            Roles = new List<Guid>();
            Logins = new List<UserLoginInfo>();
            Claims = new List<IdentityUserClaim>();
        }

        //[BsonId(IdGenerator = typeof(CombGuidGenerator))]
        //public virtual Guid Id { get; set; }

        public Guid PartnerGuid { get; set; }



        #region Identity Admin Panel

        [BsonIgnore]
        string IUser<string>.Id => Id.ToString();

        public string UserName { get; set; }

        public virtual string SecurityStamp { get; set; }

        public virtual string Email { get; set; }

        public virtual bool EmailConfirmed { get; set; }

        public virtual string PhoneNumber { get; set; }

        public virtual bool PhoneNumberConfirmed { get; set; }

        public virtual bool TwoFactorEnabled { get; set; }

        public virtual DateTime? LockoutEndDateUtc { get; set; }

        public virtual bool LockoutEnabled { get; set; }

        public virtual int AccessFailedCount { get; set; }

        [BsonIgnoreIfNull]
        public List<Guid> Roles { get; set; }

        public virtual void AddRole(Guid role)
        {
            Roles.Add(role);
        }

        public virtual void RemoveRole(Guid role)
        {
            Roles.Remove(role);
        }

        [BsonIgnoreIfNull]
        public virtual string PasswordHash { get; set; }

        [BsonIgnoreIfNull]
        public List<UserLoginInfo> Logins { get; set; }

        public virtual void AddLogin(UserLoginInfo login)
        {
            Logins.Add(login);
        }

        public virtual void RemoveLogin(UserLoginInfo login)
        {
            var loginsToRemove = Logins
                .Where(l => l.LoginProvider == login.LoginProvider)
                .Where(l => l.ProviderKey == login.ProviderKey);

            Logins = Logins.Except(loginsToRemove).ToList();
        }

        public virtual bool HasPassword()
        {
            return false;
        }

        [BsonIgnoreIfNull]
        public List<IdentityUserClaim> Claims { get; set; }

        

        public virtual void AddClaim(Claim claim)
        {
            Claims.Add(new IdentityUserClaim(claim));
        }

        public virtual void RemoveClaim(Claim claim)
        {
            var claimsToRemove = Claims
                .Where(c => c.Type == claim.Type)
                .Where(c => c.Value == claim.Value);

            Claims = Claims.Except(claimsToRemove).ToList();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<PartnerUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
        #endregion
    }
}
