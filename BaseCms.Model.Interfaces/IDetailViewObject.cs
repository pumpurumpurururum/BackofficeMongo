namespace BaseCms.Model.Interfaces
{
    public interface IDetailViewObject
    {
        bool Changed { get; set; }
        bool Deleted { get; set; }
    }
}
