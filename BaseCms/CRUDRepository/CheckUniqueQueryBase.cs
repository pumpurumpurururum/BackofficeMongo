using BaseCms.CRUDRepository.Core;

namespace BaseCms.CRUDRepository
{
    public class CheckUniqueQueryBase : QueryBase<bool>
    {
        public CheckUniqueQueryBase(string propertyName, string propertyValue, string identifier)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
            Identifier = identifier;
        }

        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
        public string Identifier { get; set; }
    }
}
