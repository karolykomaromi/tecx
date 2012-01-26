namespace TecX.Unity.Configuration.Test.TestObjects
{
    public class MyService : IMyService
    {
        public IFoo SomeFoo { get; set; }

        public IMyInterface SomeInterface { get; set; }

        public MyService(IFoo someFoo, IMyInterface someInterface)
        {
            this.SomeFoo = someFoo;
            this.SomeInterface = someInterface;
        }
    }
}