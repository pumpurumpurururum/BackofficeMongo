

namespace BaseCms.Security
{
    public class GetListPermission : CollectionPermission
    {
        public override string Description
        {
            get { return "Чтение"; }
        }
    }
}
