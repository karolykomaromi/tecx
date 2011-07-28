namespace TecX.Unity.Configuration.Test.TestObjects
{
    public class RegistrySubClass : Registry
    {
        public RegistrySubClass()
        {
            For<IRepository<int>>().Use<Repository<int>>();
        }
    }
}
