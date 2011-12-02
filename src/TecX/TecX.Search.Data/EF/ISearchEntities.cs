namespace TecX.Search.Data.EF
{
    using System.Data.Entity;

    public interface ISearchEntities
    {
        IDbSet<Customer> Customers { get; set; }
    }
}