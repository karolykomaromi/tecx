namespace TecX.Unity.Configuration.Test.TestObjects
{
    internal class ClassThatUsesRepository
    {
        public IRepository<string> Repository { get; set; }

        public ClassThatUsesRepository(IRepository<string> repository)
        {
            Repository = repository;
        }
    }
}