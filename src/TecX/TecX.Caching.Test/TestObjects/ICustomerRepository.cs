namespace TecX.Caching.Test.TestObjects
{
    using System.Linq;

    public interface ICustomerRepository
    {
        IQueryable<Customer> Customers { get; }

        void Add(Customer customer);
    }
}
