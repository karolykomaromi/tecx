namespace TecX.Unity.Test.TestObjects
{
    public class MyService
    {
        public IMatchByConvention Convention { get; set; }

        public MyService(IMatchByConvention convention)
        {
            this.Convention = convention;
        }
    }
}