namespace BaseCms.Security
{
    public abstract class ExternalSystemPermission : Permission
    {
        public abstract string PermissionKey { get; }
        public string ExternalSystem { get; set; }
        public override string FullDescription
        {
            get { return Description + " (" + ExternalSystem + ")"; }
        }
    }
}
