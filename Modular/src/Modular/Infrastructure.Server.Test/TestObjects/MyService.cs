namespace Infrastructure.Server.Test.TestObjects
{
    internal class MyService
    {
        public IMatchByConvention Convention { get; set; }

        public MyService(IMatchByConvention convention)
        {
            this.Convention = convention;
        }
    }
}