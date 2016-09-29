using System;

namespace BaseCms.Common.Typograph.Interfaces
{
    public interface ITypograph
    {
        string ProcessText(string text, Type typographHelperType);
    }
}
