namespace BaseCms.Security
{
    public class AuthentificationPermission : Permission
    {
        public override string Description
        {
            get { return "Доступ на вход"; }
        }
        public override string FullDescription
        {
            get { return Description; }
        }
    }
}
