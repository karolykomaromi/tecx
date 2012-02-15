namespace TecX.Unity.TypedFactory.Test.TestObjects
{
    public interface ICustomerRepositoryFactory
    {
        ICustomerRepository Create(string connectionString);
    }
}