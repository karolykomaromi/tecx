namespace TecX.Unity.Configuration.Test.TestObjects
{
    public class MyClassWithCtorParams : IMyInterface
    {
        public string Name { get; set; }

        public MyClassWithCtorParams()
        {
        }

        public MyClassWithCtorParams(string name)
        {
            Name = name;
        }
    }
}