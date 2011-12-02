namespace TecX.Search.Data
{
    using System.Linq;

    public interface ICustomerRepository
    {
        IQueryable<Customer> Customers { get; } 
    }
}
