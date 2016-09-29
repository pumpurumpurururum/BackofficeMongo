namespace BackofficeDemo.Model.Interfaces
{
    public interface ICustomerInfo
    {
        int SeId { get; set; }
        string Email { get; set; }

        string Name { get; set; }

        string PhoneNumber { get; set; }

        string Address { get; set; }

        
        string City { get; set; }

        string State { get; set; }

        string ZipCode { get; set; }
    }
}
