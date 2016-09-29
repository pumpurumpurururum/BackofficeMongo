using System;

namespace BaseCms.Model
{
    public class DataWithIdentifier<T2, T3> where T3 : class
    {
        public readonly Type Type;
        public readonly T2 Data;
        public readonly T3 Identifier;
        public string Url;

        public DataWithIdentifier(Type type, T2 data, T3 identifier, string url = null)
        {
            Data = data;
            Type = type;
            Identifier = identifier;
            Url = url;
        }
    }
}
