using System;

namespace BaseCms.Views.Detail.Interfaces
{
    public interface ITypeItem
    {
        Type Type { get; }
        Func<string, object> ParseMethod { get; set; }
        string Template { get; set; }
        string ListViewName { get; set; }
    }
}
