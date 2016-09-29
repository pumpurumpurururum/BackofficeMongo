namespace BaseCms.Security
{
    public class ExternalSystemAccessPermission : ExternalSystemPermission
    {
        public override string Description
        {
            get { return "Запуск"; }
        }

        public override string PermissionKey
        {
            get { return "Access"; }
        }
    }
}
