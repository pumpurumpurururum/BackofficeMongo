using System;

namespace BaseCms.Common
{
    public static class InstanceCreator
    {
        public static T CreateInstance<T>(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type == null)
            {
                throw new Exception("Impossible to create instance of " + typeName);
            }
            return (T)Activator.CreateInstance(type);
        }

        public static T CreateInstance<T>(Type type)
        {
            return (T)Activator.CreateInstance(type);
        }
    }
}
