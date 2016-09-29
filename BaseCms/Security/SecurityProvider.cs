using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using BaseCms.CRUDRepository;

namespace BaseCms.Security
{
    public class SecurityProvider
    {
        private readonly SecurityRepository _repository;
        private SecurityModel _securityModel;

        public SecurityProvider(SecurityRepository repository)
        {
            _repository = repository;
            Refresh();
        }

        public SecurityModel Model
        {
            get { return _securityModel; }
        }

        public void Refresh()
        {
            var newSecurityModel = _repository.GetSecurityModel();
            if (newSecurityModel == null)
            {
                newSecurityModel = CreateNewSecurityModel();
                _repository.Save(newSecurityModel);
                _securityModel = newSecurityModel;
            }
            else
            {
                _securityModel = newSecurityModel;
            }
        }

        private SecurityModel CreateNewSecurityModel()
        {
            return new SecurityModel()
            {
                Users = new List<User>()
                        {
                            new User(){Id = Guid.NewGuid(),Login = "admin"}
                        },
                Roles = new List<Role>(),
                UserToRoles = new List<UserToRole>(),
            };
        }

        public IEnumerable<Permission> GetPermissions(Guid userId)
        {
            var permissions = new List<Permission>();
            foreach (var role in Model.UserToRoles.Where(p => p.UserId == userId))
            {
                permissions.AddRange(Model.Roles.Where(p => p.Id == role.RoleId)
                                          .SelectMany(r => r.Permissions));
            }
            return permissions;
        }

        public IEnumerable<CollectionPermission> GetCollectionPermissions(string collectionName)
        {
            var userId = CurrentUser.Id;
            return
                Model.UserToRoles.Where(ur => ur.UserId == userId)
                     .Select(ur => Model.Roles.First(r => r.Id == ur.RoleId))
                     .SelectMany(r => r.Permissions)
                     .OfType<CollectionPermission>()
                     .Where(cp => cp.Grant && cp.Collection == collectionName);
        }

        public bool CheckPermission(Type queryType, string collectionName)
        {
            var userId = CurrentUser.Id;
            if (queryType == typeof(InsertObject))
            {
                return
                    (Model.UserToRoles.Where(p => p.UserId == userId)
                          .Any(role => Model.Roles.Where(p => p.Id == role.RoleId)
                                            .SelectMany(r => r.Permissions)
                                            .OfType<InsertObjectPermission>()
                                            .Any(p => p.Grant && p.Collection == collectionName)));
            }
            if (queryType == typeof(UpdateObject))
            {
                return
                    (Model.UserToRoles.Where(p => p.UserId == userId)
                          .Any(role => Model.Roles.Where(p => p.Id == role.RoleId)
                                            .SelectMany(r => r.Permissions)
                                            .OfType<UpdateObjectPermission>()
                                            .Any(p => p.Grant && p.Collection == collectionName)));
            }
            if (queryType == typeof(DeleteObject))
            {
                return
                    (Model.UserToRoles.Where(p => p.UserId == userId)
                          .Any(role => Model.Roles.Where(p => p.Id == role.RoleId)
                                            .SelectMany(r => r.Permissions)
                                            .OfType<DeleteObjectPermission>()
                                            .Any(p => p.Grant && p.Collection == collectionName)));
            }
            return true;
        }

        public User CurrentUser
        {
            get { return Model.Users.First(p => p.Id == Guid.Parse(HttpContext.Current.User.Identity.Name)); }
        }


        public User GetUserByLoginAndPassword(string login, string password)
        {
            var user = _securityModel.Users.FirstOrDefault(f => f.Login == login);
            if (user == null)
                return null;
            var salt = user.Salt;
            var passwordHash = GeneratePasswordHash(password, salt);
            if (user.PasswordHash == passwordHash || user.PasswordHash == null)
                return user;
            return null;
        }

        public string GeneratePasswordHash(string password, string salt)
        {
            using (SHA512 shaM = new SHA512Managed())
            {
                var data = Encoding.UTF8.GetBytes(password + salt);
                var hash = shaM.ComputeHash(data);
                return BitConverter.ToString(hash);
            }
        }


        public string GenerateSalt()
        {
            return Guid.NewGuid().ToString("N");
        }
        public void Save()
        {
            _repository.Save(_securityModel);
        }

    }

    public abstract class SecurityRepository
    {
        public abstract SecurityModel GetSecurityModel();

        public abstract void Save(SecurityModel securityModel);
    }

    public class XmlFileSecurityRepository : SecurityRepository
    {
        private readonly string _filePath = ConfigurationManager.AppSettings["Security.XmlFileRepository.Path"];

        public override SecurityModel GetSecurityModel()
        {
            var ser =
                new DataContractSerializer(typeof(SecurityModel));
            var filename = GetFullPath();
            var ret = new SecurityModel();
            if (!File.Exists(filename))
                throw new FileNotFoundException("Cannot find xml security file " + filename);
            using (var fileStream = File.OpenRead(filename))
                ret = (SecurityModel)ser.ReadObject(fileStream);
            return ret;



            


        }

        private string GetFullPath()
        {
            return Path.IsPathRooted(_filePath) ? Path.GetFullPath(_filePath) : HttpContext.Current.Server.MapPath(_filePath);
        }

        public override void Save(SecurityModel securityModel)
        {
            var ser =
                new DataContractSerializer(typeof(SecurityModel));
            var filename = GetFullPath();
            Stream fileStream;
            if (!File.Exists(filename))
                using (fileStream = File.Create(filename))
                    ser.WriteObject(fileStream, securityModel);
            else
                using (fileStream = File.OpenWrite(filename))
                {
                    fileStream.SetLength(0);
                    ser.WriteObject(fileStream, securityModel);
                }
        }
    }
}
