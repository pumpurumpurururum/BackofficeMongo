namespace BaseCms.Security
{
    public class DeleteObjectPermission : CollectionPermission
    {
        public override string Description
        {
            get { return "Удаление"; }
        }
    }
}
