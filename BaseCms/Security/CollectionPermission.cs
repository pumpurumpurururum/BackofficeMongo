namespace BaseCms.Security
{
    public abstract class CollectionPermission : Permission
    {
        public string Collection { get; set; }
        public override string FullDescription
        {
            get { return Description + " (" + Collection + ")"; }
        }
    }
}
