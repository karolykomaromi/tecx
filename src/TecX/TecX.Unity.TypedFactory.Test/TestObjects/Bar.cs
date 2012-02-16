namespace TecX.Unity.TypedFactory.Test.TestObjects
{
    public class Bar : IFoo
    {
        public string Name { get; set; }

        public Bar(string name)
        {
            Name = name;
        }
    }
}