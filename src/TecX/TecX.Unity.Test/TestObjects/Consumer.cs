namespace TecX.Unity.Test.TestObjects
{
    public class Consumer
    {
        public Consumer(UnitOfWorkFactory factory)
        {
            this.Factory = factory;
        }

        public UnitOfWorkFactory Factory { get; set; }
    }
}