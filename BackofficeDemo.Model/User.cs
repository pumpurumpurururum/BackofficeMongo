using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BackofficeDemo.MongoBase;
using Microsoft.AspNet.Identity;
using MongoDB.Bson.Serialization.Attributes;

namespace BackofficeDemo.Model
{
    public class User : Entity, IUser<Guid>, IUser<string>
    {

        public User()
        {
            Claims = new List<IdentityUserClaim>();
            Addresses = new List<UserAddress>();
        }

        public DateTime CreatedOnUtc { get; set; }

        public string UserName { get; set; }
        public string Name { get; set; }
        public virtual string SecurityStamp { get; set; }

        public virtual string Email { get; set; }
  

        [BsonIgnoreIfNull]
        public virtual string PasswordHash { get; set; }

        [BsonIgnoreIfNull]
        public List<IdentityUserClaim> Claims { get; set; }

        [BsonIgnoreIfNull]
        public List<UserAddress> Addresses { get; set; }

        [BsonIgnore]
        string IUser<string>.Id => Id.ToString();

        public string PhoneNumber { get; set; }

        public string PhoneNumberToDial { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public DateTimeOffset? LockoutEndDateUtc { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Decimal Bonus { get; set; }

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

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }

        public bool HasPassword()
        {
            return false;
        }
    }
}
