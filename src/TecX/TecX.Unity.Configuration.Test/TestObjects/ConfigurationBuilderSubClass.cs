namespace TecX.Unity.Configuration.Test.TestObjects
{
    public class ConfigurationBuilderSubClass : ConfigurationBuilder
    {
        public ConfigurationBuilderSubClass()
        {
            For<IRepository<int>>().Use<Repository<int>>();
        }
    }
}
