using System;

namespace BaseCms.Model.Interfaces
{
    public interface IAlias
    {
        Guid Id { get; set; }
        string Alias { get; set; }
        string Name { get; set; }
    }
}
