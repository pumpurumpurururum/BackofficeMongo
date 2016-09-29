using System.Collections.Generic;

namespace BaseCms.Security
{
    //[DataContract]
    public class SecurityModel
    {
        
        public List<User> Users { get; set; }
        
        public List<Role> Roles { get; set; }
        
        public List<UserToRole> UserToRoles { get; set; }

        //[OnDeserializing]
        //private void OnDeserializing(StreamingContext context)
        //{
        //    Initialize();
        //}

        //public SecurityModel()
        //{
        //    Initialize();
        //}

        //private void Initialize()
        //{
        //    Users = new List<User>();
        //    Roles = new List<Role>();
        //    UserToRoles = new List<UserToRole>();
        //}
    }
}
