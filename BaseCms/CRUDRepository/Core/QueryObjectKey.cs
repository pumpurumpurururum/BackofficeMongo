using System;

namespace BaseCms.CRUDRepository.Core
{
    public class QueryObjectKey
    {
        public QueryObjectKey(string collectionName, Type objectType)
        {
            CollectionName = collectionName;
            ObjectType = objectType;
        }

        public string CollectionName { get; set; }
        public Type ObjectType { get; set; }

        protected bool Equals(QueryObjectKey other)
        {
            return string.Equals(CollectionName, other.CollectionName) && ObjectType == other.ObjectType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((QueryObjectKey)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((CollectionName != null ? CollectionName.GetHashCode() : 0) * 397) ^ (ObjectType != null ? ObjectType.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return string.Format("Collection : {0}, Type : {1}", CollectionName, ObjectType);
        }
    }
}
